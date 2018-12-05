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

        public LeagueSeason GetLeagueSeason(int tier, string season)
        {
            var leagueSeason = new LeagueSeason() { Season = season, Tier = tier};
            
            using(var conn = m_Context.Database.GetDbConnection())
            {
                var seasonStartYear = season.Substring(0, 4);
                var seasonEndYear = season.Substring(7, 4);

                var matchDetails = GetMatchDetails(conn, tier, seasonStartYear, seasonEndYear);
                var leagueDetail = GetLeagueDetail(conn, tier, season);
                var pointDeductions = GetPointDeductions(conn, tier, season);

                CreateLeagueSeason(leagueSeason, matchDetails, leagueDetail, pointDeductions);
            }

            return leagueSeason;
        }

        public FilterOptions GetFilterOptions()
        {
            var filterOptions = new FilterOptions
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
FROM dbo.Divisions
WHERE [From] >= 1992;
";
            using(var conn = m_Context.Database.GetDbConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        filterOptions.AllSeasons.Add(
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

                        AddDivision(tier, division, filterOptions);
                    }
                } 
                else 
                {
                    System.Console.WriteLine("No rows found");
                }
                reader.Close();
            }

            return filterOptions;
        }

        private void AddDivision(int tier, Division division, FilterOptions leagueFilterOptions)
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

        private List<LeagueTableRow> CreateLeagueTable(List<MatchDetail> matchDetails, LeagueDetail leagueDetail, List<PointDeduction> pointDeductions)
        {
            var leagueTable = new List<LeagueTableRow>();

            AddLeagueRows(leagueTable, matchDetails);
            IncludePointDeductions(leagueTable, pointDeductions);

            leagueTable = SortLeagueTable(leagueTable);

            SetLeaguePosition(leagueTable);
            AddTeamStatus(leagueTable, leagueDetail, matchDetails);

            return leagueTable;
        }

        public LeagueRowDrillDown GetDrillDown(int tier, string season, string team)
        {
            var result = new LeagueRowDrillDown();

            using(var conn = m_Context.Database.GetDbConnection())
            {
                result.Form = GetLeagueForm(conn, tier, season, team);
                result.Positions = GetIncrementalLeaguePositions(conn, tier, season, team);
            }

            return result;
        }

        private List<MatchResult> GetLeagueForm(DbConnection conn, int tier, string season, string team)
        {
            var seasonStartYear = season.Substring(0, 4);
            var seasonEndYear = season.Substring(7, 4);

            var sql = @"
SELECT lm.MatchDate
	,CASE WHEN lm.HomeGoals > lm.AwayGoals THEN 'W'
		  WHEN lm.AwayGoals > lm.HomeGoals THEN 'L' 
		  ELSE 'D' END AS Result
FROM dbo.LeagueMatches AS lm
INNER JOIN dbo.Divisions d ON d.Id = lm.DivisionId
INNER JOIN dbo.Clubs AS hc ON hc.Id = lm.HomeClubId
WHERE d.Tier = @Tier
    AND (hc.Name = @Team)
    AND lm.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)

UNION ALL

SELECT lm.MatchDate
	,CASE WHEN lm.HomeGoals < lm.AwayGoals THEN 'W'
		  WHEN lm.AwayGoals < lm.HomeGoals THEN 'L' 
		  ELSE 'D' END AS Result
FROM dbo.LeagueMatches AS lm
INNER JOIN dbo.Divisions d ON d.Id = lm.DivisionId
INNER JOIN dbo.Clubs AS ac ON ac.Id = lm.AwayClubId
WHERE d.Tier = @Tier
    AND (ac.Name = @Team)
    AND lm.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)

ORDER BY MatchDate
";
            
            var form = new List<MatchResult>();

            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@Tier", tier));
            cmd.Parameters.Add(new SqlParameter("@SeasonStartYear", seasonStartYear));
            cmd.Parameters.Add(new SqlParameter("@SeasonEndYear", seasonEndYear));
            cmd.Parameters.Add(new SqlParameter("@Team", team));

            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    form.Add(
                        new MatchResult
                        {
                            MatchDate = reader.GetDateTime(0),
                            Result = reader.GetString(1)
                        }
                    );
                }
            } 
            else 
            {
                System.Console.WriteLine("No rows found");
            }
            reader.Close();
            conn.Close();

            return form;
        }

        private void AddTeamStatus(List<LeagueTableRow> leagueTable, LeagueDetail leagueDetail, List<MatchDetail> matchDetails)
        {
            var playOffFinal = matchDetails.Where(m => m.Type == "PlayOff" && m.Round == "Final").ToList();
            
            foreach (var row in leagueTable)
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

        private void AddLeagueRows(List<LeagueTableRow> leagueTable, List<MatchDetail> matchDetails)
        {
            var filteredHomeTeams = matchDetails.Select(m => m.HomeTeam).ToList();
            var filteredAwayTeams = matchDetails.Select(m => m.AwayTeam).ToList();
            var teams = filteredHomeTeams.Union(filteredAwayTeams).ToList();
            
            foreach (var team in teams)
            {
                var homeGames = matchDetails.Where(m => m.HomeTeam == team && m.Type == "League").ToList();
                var awayGames = matchDetails.Where(m => m.AwayTeam == team && m.Type == "League").ToList();

                leagueTable.Add(
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

            foreach (var row in leagueTable)
            {
                row.Played = row.Won + row.Drawn + row.Lost;
                row.GoalDifference = row.GoalsFor - row.GoalsAgainst;
                row.Points = (row.Won * 3) + row.Drawn;
            }
        }

        private void IncludePointDeductions(List<LeagueTableRow> table, List<PointDeduction> pointDeductions)
        {
            foreach (var row in table)
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
        
        private void SetLeaguePosition(List<LeagueTableRow> table)
        {
            table = table.Select((t, i) => { 
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

            conn.Open();
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
            conn.Close();

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

            conn.Open();
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
            conn.Close();

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
            
            conn.Open();
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
            conn.Close();

            return pointDeductions;
        }

        private List<LeaguePosition> GetIncrementalLeaguePositions(DbConnection conn, int tier, string season, string team)
        {
            var seasonStartYear = season.Substring(0, 4);
            var seasonEndYear = season.Substring(7, 4);

            var matchDetails = GetMatchDetails(conn, tier, seasonStartYear, seasonEndYear);
            var pointDeductions = GetPointDeductions(conn, tier, season);

            var teams = matchDetails.Select(m => m.HomeTeam).Distinct().ToList();

            var positions = new List<LeaguePosition>();

            var dates = matchDetails.Select(m => m.Date).Distinct().OrderBy(m => m.Date).ToList();
            dates.Add(dates.Last().AddDays(1));

            foreach (var dt in dates)
            {
                var leagueTable = new List<LeagueTableRow>();

                var filteredMatchDetails = matchDetails.Where(m => m.Date < dt).ToList();
                var filteredHomeTeams = filteredMatchDetails.Select(m => m.HomeTeam).ToList();
                var filteredAwayTeams = filteredMatchDetails.Select(m => m.AwayTeam).ToList();
                var filteredTeams = filteredHomeTeams.Union(filteredAwayTeams).ToList();

                var missingTeams = teams.Where(p => filteredTeams.All(p2 => p2 != p)).ToList();

                foreach (var t in missingTeams)
                {
                    leagueTable.Add(new LeagueTableRow
                    {
                        Team = t,
                        Won = 0,
                        Drawn = 0,
                        Lost = 0,
                        GoalsFor = 0,
                        GoalsAgainst = 0
                    });
                }
                
                AddLeagueRows(leagueTable, filteredMatchDetails);
                IncludePointDeductions(leagueTable, pointDeductions);

                leagueTable = SortLeagueTable(leagueTable);

                SetLeaguePosition(leagueTable);

                positions.Add(
                    new LeaguePosition
                    {
                        Date = dt,
                        Position = leagueTable.Where(l => l.Team == team).Select(r => r.Position).Single()
                    }
                );
            }

            return positions;
        }

        private List<LeagueTableRow> SortLeagueTable(List<LeagueTableRow> leagueTable)
        {
            return leagueTable
                    .OrderByDescending(t => t.Points)
                    .ThenByDescending(t => t.GoalDifference)
                    .ThenByDescending(t => t.GoalsFor)
                    // head to head
                    .ThenBy(t => t.Team) // unless it affects a promotion/relgeation spot at the end of the season in which case a play-off occurs (this has never happened)
                    .ToList();
        }
    }
}