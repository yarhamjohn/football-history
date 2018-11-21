using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace football_history.Server.Repositories
{
    public class FootballHistoryRepository : IFootballHistoryRepository
    {
        private FootballHistoryContext m_Context { get; }

        public FootballHistoryRepository(FootballHistoryContext context)
        {
            m_Context = context;
        }

        public LeagueSeason GetLeagueSeason(int? tier, string season)
        {
            var leagueSeason = new LeagueSeason();
            
            using(var conn = m_Context.Database.GetDbConnection())
            {
                conn.Open();
                leagueSeason.LeagueFilterOptions = GetFilterOptions(conn);

                var highestTier = leagueSeason.LeagueFilterOptions.AllTiers.First().Level;
                var selectedTier = tier == null ? highestTier : (int) tier;

                var latestSeason = leagueSeason.LeagueFilterOptions.AllSeasons.First();
                var selectedSeason = season == null ? latestSeason : season;

                leagueSeason.Season = selectedSeason;
                leagueSeason.Tier = selectedTier;

                var seasonStartYear = selectedSeason.Substring(0, 4);
                var seasonEndYear = selectedSeason.Substring(7, 4);

                var matchDetails = GetMatchDetails(conn, selectedTier, seasonStartYear, seasonEndYear);
                var leagueDetail = GetLeagueDetail(conn, selectedTier, selectedSeason);
                var pointDeductions = GetPointDeductions(conn, selectedTier, selectedSeason);

                CreateLeagueSeason(leagueSeason, matchDetails, leagueDetail, pointDeductions);
            }

            return leagueSeason;
        }

        private LeagueFilterOptions GetFilterOptions(DbConnection conn)
        {
            var leagueFilterOptions = new LeagueFilterOptions
            {
                AllSeasons = new List<string>(),
                AllTiers = new List<Tier>()
            };

            var sql = @"
SELECT Season
FROM (
    SELECT CASE WHEN MONTH(MatchDate) >= 7 
                THEN CAST(YEAR(MatchDate) AS CHAR(4)) + ' - ' + CAST(YEAR(MatchDate) + 1 AS CHAR(4))
                ELSE CAST(YEAR(MatchDate) - 1 AS CHAR(4)) + ' - ' + CAST(YEAR(MatchDate) AS CHAR(4))
                END AS Season
    FROM dbo.LeagueMatches
) m
GROUP BY Season;

SELECT Name, Tier, [From], [To]
FROM dbo.Divisions;
";

            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    leagueFilterOptions.AllSeasons.Add(
                        reader.GetString(0)
                    );
                }

                reader.NextResult();

                while (reader.Read())
                {
                    var tier = reader.GetByte(1);
                    var division = new Division
                        {
                            Name = reader.GetString(0),
                            SeasonStartYear = reader.GetInt16(2),
                            SeasonEndYear = reader.IsDBNull(3) ? DateTime.UtcNow.Year : reader.GetInt16(3)
                        };

                    AddDivision(tier, division, leagueFilterOptions);
                }
            } 
            else 
            {
                System.Console.WriteLine("No rows found");
            }
            reader.Close();

            leagueFilterOptions.AllTiers = leagueFilterOptions.AllTiers.OrderBy(t => t.Level).ToList();
            leagueFilterOptions.AllSeasons = leagueFilterOptions.AllSeasons.OrderByDescending(s => s).ToList();
            return leagueFilterOptions;
        }

        private void AddDivision(int tier, Division division, LeagueFilterOptions leagueFilterOptions)
        {
            var tierExists = leagueFilterOptions.AllTiers.Where(t => t.Level == tier).ToList().Count == 1;
            if (tierExists)
            {
                leagueFilterOptions.AllTiers = leagueFilterOptions.AllTiers
                    .Select(t => {
                        if (t.Level == tier) {
                            t.Divisions.Add(division);
                        }; 
                        return t;
                    }).ToList();
            }
            else 
            {
                leagueFilterOptions.AllTiers.Add(
                    new Tier
                    {
                        Level = tier,
                        Divisions = new List<Division> { division }
                    }
                );
            }
        }

        private void CreateLeagueSeason(LeagueSeason leagueSeason, List<MatchDetail> matchDetails, LeagueDetail leagueDetail, List<PointDeduction> pointDeductions)
        {
            leagueSeason.CompetitionName = leagueDetail.Competition;
            leagueSeason.LeagueTable = CreateLeagueTable(matchDetails, leagueDetail, pointDeductions);
            leagueSeason.PlayOffs = CreatePlayOffs(matchDetails);
            leagueSeason.ResultMatrix = CreateResultMatrix(matchDetails);
        }

        private LeagueTable CreateLeagueTable(List<MatchDetail> matchDetails, LeagueDetail leagueDetail, List<PointDeduction> pointDeductions)
        {
            var leagueTable = new LeagueTable { Rows = new List<LeagueTableRow>() };

            AddLeagueRows(leagueTable, matchDetails);
            IncludePointDeductions(leagueTable, pointDeductions);
            SortLeagueTable(leagueTable);
            SetLeaguePosition(leagueTable);
            AddTeamStatus(leagueTable, leagueDetail, matchDetails);
            AddLeagueForm(leagueTable, matchDetails);

            return leagueTable;
        }

        private void AddLeagueForm(LeagueTable leagueTable, List<MatchDetail> matchDetails)
        {
            foreach (var row in leagueTable.Rows)
            {
                var matches = matchDetails.Where(m => m.Type == "League" && (m.HomeTeam == row.Team || m.AwayTeam == row.Team)).OrderBy(m => m.Date).ToList();
                var form = matches.Select(m => new MatchResult
                {
                    MatchDate = m.Date,
                    Result = row.Team == m.HomeTeam ? (m.HomeGoals > m.AwayGoals ? "W" : m.HomeGoals < m.AwayGoals ? "L" : "D") : (m.HomeGoals < m.AwayGoals ? "W" : m.HomeGoals > m.AwayGoals ? "L" : "D")
                }).ToList();

                row.Form = form;
            }
        }

        private void AddTeamStatus(LeagueTable leagueTable, LeagueDetail leagueDetail, List<MatchDetail> matchDetails)
        {
            var playOffFinal = matchDetails.Where(m => m.Type == "PlayOff" && m.Round == "Final").ToList();
            
            foreach (var row in leagueTable.Rows)
            {
                if (row.Position == 1)
                {
                    row.Status = "C";
                }
                else if (row.Position <= leagueDetail.PromotionPlaces)
                {
                    row.Status = "P";
                }
                else if (playOffFinal.Count == 1 && row.Position <= leagueDetail.PlayOffPlaces + leagueDetail.PromotionPlaces)
                {
                    var final = playOffFinal.Single();
                    var winner = final.PenaltyShootout 
                        ? (final.HomePenaltiesScored > final.AwayPenaltiesScored ? final.HomeTeam : final.AwayTeam) 
                        : final.ExtraTime
                            ? (final.HomeGoalsET > final.AwayGoalsET ? final.HomeTeam : final.AwayTeam) 
                            : (final.HomeGoals > final.AwayGoals ? final.HomeTeam : final.AwayTeam);
                    
                    if (row.Team == winner)
                    {
                        row.Status = "PO (P)";
                    }
                    else
                    {
                        row.Status = "PO";
                    }
                }
                else if (row.Position > leagueDetail.TotalPlaces - leagueDetail.RelegationPlaces)
                {
                    row.Status = "R";
                }
                else
                {
                    row.Status = string.Empty;
                }
            }
        }

        private void AddLeagueRows(LeagueTable leagueTable, List<MatchDetail> matchDetails)
        {
            var teams = matchDetails.Select(m => m.HomeTeam).Distinct().ToList();
            foreach (var team in teams)
            {
                var homeGames = matchDetails.Where(m => m.HomeTeam == team).ToList();
                var awayGames = matchDetails.Where(m => m.AwayTeam == team).ToList();

                leagueTable.Rows.Add(
                    new LeagueTableRow
                    {
                        Team = team,
                        Won = homeGames.Count(g => g.HomeGoals > g.AwayGoals) + awayGames.Count(g => g.AwayGoals > g.HomeGoals),
                        Drawn = homeGames.Count(g => g.HomeGoals == g.AwayGoals) + awayGames.Count(g => g.AwayGoals == g.HomeGoals),
                        Lost = homeGames.Count(g => g.HomeGoals < g.AwayGoals) + awayGames.Count(g => g.AwayGoals < g.HomeGoals),
                        GoalsFor = homeGames.Sum(g => g.HomeGoals) + awayGames.Sum(g => g.AwayGoals),
                        GoalsAgainst = homeGames.Sum(g => g.AwayGoals) + awayGames.Sum(g => g.HomeGoals),
                    }
                );
            }

            foreach (var row in leagueTable.Rows)
            {
                row.Played = row.Won + row.Drawn + row.Lost;
                row.GoalDifference = row.GoalsFor - row.GoalsAgainst;
                row.Points = (row.Won * 3) + row.Drawn;
            }
        }

        private void IncludePointDeductions(LeagueTable table, List<PointDeduction> pointDeductions)
        {
            foreach (var row in table.Rows)
            {
                var deduction = pointDeductions.Where(d => d.Team == row.Team).ToList();

                if (deduction.Count == 0)
                {
                    row.PointsDeducted = 0;
                    row.PointsDeductionReason = string.Empty;
                } 
                else 
                {
                    var d = deduction.Single();

                    row.PointsDeducted = d.PointsDeducted;
                    row.PointsDeductionReason = d.Reason;
                    row.Points -= d.PointsDeducted;
                }
            }
        }
        
        private void SortLeagueTable(LeagueTable table)
        {
            table.Rows = table.Rows
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalDifference)
                .ThenByDescending(t => t.GoalsFor) // head to head, alphabetical unless P/R spots, then a play off (never happened)
                .ToList();
        }

        private void SetLeaguePosition(LeagueTable table)
        {
            table.Rows = table.Rows.Select((t, i) => { 
                t.Position = i + 1; 
                return t; 
            }).ToList();
        }

        private PlayOffs CreatePlayOffs(List<MatchDetail> matchDetails)
        {
            var playOffMatches = matchDetails
                .Where(m => m.Type == "PlayOff")
                .Select(m => new MatchDetail 
                    {
                        Competition = m.Competition,
                        Type = m.Type,
                        Round = m.Round,
                        Date = m.Date,
                        HomeTeam = m.HomeTeam,
                        HomeTeamAbbreviation = m.HomeTeamAbbreviation,
                        AwayTeam = m.AwayTeam,
                        AwayTeamAbbreviation = m.AwayTeamAbbreviation,
                        HomeGoals = m.HomeGoals,
                        AwayGoals = m.AwayGoals,
                        ExtraTime = m.ExtraTime,
                        HomeGoalsET = m.ExtraTime ? m.HomeGoalsET : (int?) null,
                        AwayGoalsET = m.ExtraTime ? m.AwayGoalsET : (int?) null,
                        PenaltyShootout = m.PenaltyShootout,
                        HomePenaltiesTaken = m.PenaltyShootout ? m.HomePenaltiesTaken : (int?) null,
                        HomePenaltiesScored = m.PenaltyShootout ? m.HomePenaltiesScored : (int?) null,
                        AwayPenaltiesTaken = m.PenaltyShootout ? m.AwayPenaltiesTaken : (int?) null,
                        AwayPenaltiesScored = m.PenaltyShootout ? m.AwayPenaltiesScored : (int?) null
                    })
                .OrderBy(m => m.Date)
                .ToList();
            
            var playOffs = new PlayOffs { SemiFinals = new List<SemiFinal>() };
            foreach(var match in playOffMatches)
            {
                if (match.Round == "Final")
                {
                    playOffs.Final = match;
                }
                else
                {
                    AddSemiFinal(playOffs, match);
                }
            }

            return playOffs;
        }

        private void AddSemiFinal(PlayOffs playOffs, MatchDetail match)
        {
            if (playOffs.SemiFinals.Count == 0)
            {
                playOffs.SemiFinals.Add(new SemiFinal { FirstLeg = match });
            } 
            else if (playOffs.SemiFinals.Count == 1)
            {
                if (match.HomeTeam == playOffs.SemiFinals[0].FirstLeg.AwayTeam)
                {
                    playOffs.SemiFinals[0].SecondLeg = match;
                }
                else
                {
                    playOffs.SemiFinals.Add(new SemiFinal { FirstLeg = match });
                }
            }
            else
            {
                if (match.HomeTeam == playOffs.SemiFinals[0].FirstLeg.AwayTeam)
                {
                    playOffs.SemiFinals[0].SecondLeg = match;
                }
                else
                {
                    playOffs.SemiFinals[1].SecondLeg = match;
                }
            }
        }

        private ResultMatrix CreateResultMatrix(List<MatchDetail> matchDetails)
        {
            var teams = matchDetails.Select(m => (m.HomeTeam, m.HomeTeamAbbreviation)).Distinct().ToList();

            var resultMatrix = new ResultMatrix { Rows = new List<ResultMatrixRow>() };
            foreach (var team in teams)
            {
                resultMatrix.Rows.Add(
                    new ResultMatrixRow
                    {
                        HomeTeam = team.Item1,
                        HomeTeamAbbreviation = team.Item2,
                        Scores = GetScores(matchDetails, team.Item1, team.Item1)
                    }
                );
            }

            return resultMatrix;
        }
        private List<ResultScore> GetScores(List<MatchDetail> matchDetails, string awayTeam, string homeTeam)
        {
            var homeGames = matchDetails.Where(m => m.HomeTeam == awayTeam).ToList();

            var resultScores = new List<ResultScore> { new ResultScore { AwayTeam = homeTeam, Score = null, Result = null } };
            foreach(var game in homeGames)
            {
                resultScores.Add(
                    new ResultScore
                    {
                        AwayTeam = game.AwayTeam,
                        Score = $"{game.HomeGoals}-{game.AwayGoals}",
                        Result = game.HomeGoals > game.AwayGoals ? "W" : game.HomeGoals < game.AwayGoals ? "L" : "D"
                    }
                );
            }

            return resultScores;
        }

        private List<MatchDetail> GetMatchDetails(DbConnection conn, int tier, string seasonStartYear, string seasonEndYear)
        {
            var leagueMatchesSql = @"
SELECT d.Name AS CompetitionName
    ,'League' AS Type
    ,'Regular Season' AS Round
    ,lm.matchDate
    ,hc.Name AS HomeTeam
    ,hc.Abbreviation AS HomeAbbreviation
    ,ac.Name as AwayTeam
    ,ac.Abbreviation as AwayAbbreviation
    ,lm.HomeGoals
    ,lm.AwayGoals
    ,0 AS ExtraTime
    ,NULL AS HomeGoalsET
    ,NULL AS AwayGoalsET
    ,0 AS PenaltyShootout
    ,NULL AS HomePenaltiesTaken
    ,NULL AS HomePenaltiesScored
    ,NULL AS AwayPenaltiesTaken
    ,NULL AS AwayPenaltiesScored
FROM dbo.LeagueMatches AS lm
INNER JOIN dbo.Divisions d ON d.Id = lm.DivisionId
INNER JOIN dbo.Clubs AS hc ON hc.Id = lm.HomeClubId
INNER JOIN dbo.Clubs AS ac ON ac.Id = lm.AwayClubId
WHERE d.Tier = @Tier
    AND lm.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)
";

            var playOffMatchesSql = @"
SELECT d.Name AS CompetitionName
    ,'PlayOff' AS Type
    ,pom.Round
    ,pom.matchDate
    ,hc.Name AS HomeTeam
    ,hc.Abbreviation AS HomeAbbreviation
    ,ac.Name as AwayTeam
    ,ac.Abbreviation as AwayAbbreviation
    ,pom.HomeGoals
    ,pom.AwayGoals
    ,pom.ExtraTime AS ExtraTime
    ,pom.HomeGoalsET
    ,pom.AwayGoalsET
    ,pom.PenaltyShootout
    ,pom.HomePenaltiesTaken
    ,pom.HomePenaltiesScored
    ,pom.AwayPenaltiesTaken
    ,pom.AwayPenaltiesScored
FROM dbo.PlayOffMatches AS pom
INNER JOIN dbo.Divisions d ON d.Id = pom.DivisionId
INNER JOIN dbo.Clubs AS hc ON hc.Id = pom.HomeClubId
INNER JOIN dbo.Clubs AS ac ON ac.Id = pom.AwayClubId
WHERE d.Tier = @Tier
    AND pom.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)
";

            var sql = $@"{leagueMatchesSql} UNION ALL {playOffMatchesSql}";

            var matchDetails = new List<MatchDetail>();

            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@Tier", tier));
            cmd.Parameters.Add(new SqlParameter("@SeasonStartYear", seasonStartYear));
            cmd.Parameters.Add(new SqlParameter("@SeasonEndYear", seasonEndYear));

            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var extraTime = reader.GetInt32(10) == 1 ? true : false;
                    var penaltyShootout = reader.GetInt32(13) == 1 ? true : false;

                    matchDetails.Add(
                        new MatchDetail
                        {
                            Competition = reader.GetString(0),
                            Type = reader.GetString(1),
                            Round = reader.GetString(2),
                            Date = reader.GetDateTime(3),
                            HomeTeam = reader.GetString(4),
                            HomeTeamAbbreviation = reader.GetString(5),
                            AwayTeam = reader.GetString(6),
                            AwayTeamAbbreviation = reader.GetString(7),
                            HomeGoals = reader.GetByte(8),
                            AwayGoals = reader.GetByte(9),
                            ExtraTime = extraTime,
                            HomeGoalsET = extraTime ? reader.GetByte(11) : (int?) null,
                            AwayGoalsET = extraTime ? reader.GetByte(12) : (int?) null,
                            PenaltyShootout = penaltyShootout,
                            HomePenaltiesTaken = penaltyShootout ? reader.GetByte(14) : (int?) null,
                            HomePenaltiesScored = penaltyShootout ? reader.GetByte(15) : (int?) null,
                            AwayPenaltiesTaken = penaltyShootout ? reader.GetByte(16) : (int?) null,
                            AwayPenaltiesScored = penaltyShootout ? reader.GetByte(17) : (int?) null
                        }
                    );
                }
            }
            else 
            {
                System.Console.WriteLine("No rows found");
            }
            reader.Close();

            return matchDetails;
        }
        
        private LeagueDetail GetLeagueDetail(DbConnection conn, int tier, string season)
        {
            var sql = @"
SELECT d.Name AS CompetitionName
    ,ls.TotalPlaces
    ,ls.PromotionPlaces
    ,ls.PlayOffPlaces
    ,ls.RelegationPlaces
FROM dbo.LeagueStatuses AS ls
INNER JOIN dbo.Divisions d ON d.Id = ls.DivisionId
WHERE d.Tier = @Tier AND ls.Season = @Season
";

            var leagueDetails = new LeagueDetail();

            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@Tier", tier));
            cmd.Parameters.Add(new SqlParameter("@Season", season));

            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    leagueDetails = new LeagueDetail
                    {
                        Competition = reader.GetString(0),
                        TotalPlaces = reader.GetByte(1),
                        PromotionPlaces = reader.GetByte(2),
                        PlayOffPlaces = reader.GetByte(3),
                        RelegationPlaces = reader.GetByte(4)
                    };
                }
            }
            else 
            {
                System.Console.WriteLine("No rows found");
            }
            reader.Close();

            return leagueDetails;
        }

        private List<PointDeduction> GetPointDeductions(DbConnection conn, int tier, string season)
        {
            var sql = @"
SELECT d.Name AS Competition
    ,c.Name AS TeamName
    ,pd.PointsDeducted
    ,pd.Reason
FROM dbo.PointDeductions AS pd
INNER JOIN dbo.Divisions d ON d.Id = pd.DivisionId
INNER JOIN dbo.Clubs AS c ON c.Id = pd.ClubId
WHERE d.Tier = @Tier AND pd.Season = @Season
";

            var pointDeductions = new List<PointDeduction>();

            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@Tier", tier));
            cmd.Parameters.Add(new SqlParameter("@Season", season));

            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    pointDeductions.Add(
                        new PointDeduction
                        {
                            Competition = reader.GetString(0),
                            Team = reader.GetString(1),
                            PointsDeducted = reader.GetByte(2),
                            Reason = reader.GetString(3)
                        }
                    );
                }
            }
            else 
            {
                System.Console.WriteLine("No rows found");
            }
            reader.Close();

            return pointDeductions;
        }
    }
}