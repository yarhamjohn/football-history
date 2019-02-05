using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using FootballHistory.Api.Models;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Repositories.LeagueSeasonRepository
{
    public class LeagueSeasonRepository : ILeagueSeasonRepository
    {
        private LeagueSeasonContext Context { get; }

        public LeagueSeasonRepository(LeagueSeasonContext context)
        {
            Context = context;
        }

        public PlayOffs GetPlayOffMatches(int tier, string season)
        {
            var seasonStartYear = season.Substring(0, 4);
            var seasonEndYear = season.Substring(7, 4);

            using(var conn = Context.Database.GetDbConnection())
            {
                var matchDetails = GetPlayOffMatchDetails(conn, tier, seasonStartYear, seasonEndYear);
                return CreatePlayOffs(matchDetails);
            }
        }

        public List<LeagueTableRow> GetLeagueTable(int tier, string season)
        {
            var table = new List<LeagueTableRow>();

            using(var conn = Context.Database.GetDbConnection())
            {
                var seasonStartYear = season.Substring(0, 4);
                var seasonEndYear = season.Substring(7, 4);

                var leagueMatchDetails = GetLeagueMatchDetails(conn, tier, seasonStartYear, seasonEndYear);
                var playOffMatchDetails = GetPlayOffMatchDetails(conn, tier, seasonStartYear, seasonEndYear);
                var leagueDetail = GetLeagueDetail(conn, tier, season);
                var pointDeductions = GetPointDeductions(conn, tier, season);

                AddLeagueRows(table, leagueMatchDetails);
                IncludePointDeductions(table, pointDeductions);

                table = SortLeagueTable(table);

                SetLeaguePosition(table);
                AddTeamStatus(table, leagueDetail, playOffMatchDetails);            
            }

            return table;
        }

        public LeagueRowDrillDown GetDrillDown(int tier, string season, string team)
        {
            var result = new LeagueRowDrillDown();

            using(var conn = Context.Database.GetDbConnection())
            {
                result.Form = GetLeagueForm(conn, tier, season, team);
                result.Positions = GetIncrementalLeaguePositions(conn, tier, season, team);
            }

            return result;
        }

        private List<MatchResultOld> GetLeagueForm(DbConnection conn, int tier, string season, string team)
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
            
            var form = new List<MatchResultOld>();

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
                        new MatchResultOld
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

        private void AddTeamStatus(List<LeagueTableRow> leagueTable, LeagueDetail leagueDetail, List<MatchDetailModel> playOffMatchDetails)
        {
            var playOffFinal = playOffMatchDetails.Where(m => m.Round == "Final").ToList();
            
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

        private void AddLeagueRows(List<LeagueTableRow> leagueTable, List<MatchDetailModel> leagueMatchDetails)
        {
            var filteredHomeTeams = leagueMatchDetails.Select(m => m.HomeTeam).ToList();
            var filteredAwayTeams = leagueMatchDetails.Select(m => m.AwayTeam).ToList();
            var teams = filteredHomeTeams.Union(filteredAwayTeams).ToList();
            
            foreach (var team in teams)
            {
                var homeGames = leagueMatchDetails.Where(m => m.HomeTeam == team).ToList();
                var awayGames = leagueMatchDetails.Where(m => m.AwayTeam == team).ToList();

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

        private PlayOffs CreatePlayOffs(List<MatchDetailModel> matchDetails)
        {
            var playOffMatches = matchDetails
                .Select(m => new MatchDetailModel 
                    {
                        Competition = m.Competition,
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

        private void AddSemiFinal(PlayOffs playOffs, MatchDetailModel match)
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

        private List<MatchDetailModel> GetLeagueMatchDetails(DbConnection conn, int tier, string seasonStartYear, string seasonEndYear)
        {
            var sql = @"
SELECT d.Name AS CompetitionName
    ,lm.matchDate
    ,hc.Name AS HomeTeam
    ,hc.Abbreviation AS HomeAbbreviation
    ,ac.Name as AwayTeam
    ,ac.Abbreviation as AwayAbbreviation
    ,lm.HomeGoals
    ,lm.AwayGoals
FROM dbo.LeagueMatches AS lm
INNER JOIN dbo.Divisions d ON d.Id = lm.DivisionId
INNER JOIN dbo.Clubs AS hc ON hc.Id = lm.HomeClubId
INNER JOIN dbo.Clubs AS ac ON ac.Id = lm.AwayClubId
WHERE d.Tier = @Tier
    AND lm.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)
";

            var matchDetails = new List<MatchDetailModel>();

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
                    matchDetails.Add(
                        new MatchDetailModel
                        {
                            Competition = reader.GetString(0),
                            Date = reader.GetDateTime(1),
                            HomeTeam = reader.GetString(2),
                            HomeTeamAbbreviation = reader.GetString(3),
                            AwayTeam = reader.GetString(4),
                            AwayTeamAbbreviation = reader.GetString(5),
                            HomeGoals = reader.GetByte(6),
                            AwayGoals = reader.GetByte(7),
                            ExtraTime = false,
                            PenaltyShootout = false,
                            Round = "League"
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

        private List<MatchDetailModel> GetPlayOffMatchDetails(DbConnection conn, int tier, string seasonStartYear, string seasonEndYear)
        {
            var sql = @"
SELECT d.Name AS CompetitionName
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

            var matchDetails = new List<MatchDetailModel>();

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
                    var extraTime = reader.GetBoolean(9); // == 1 ? true : false;
                    var penaltyShootout = reader.GetBoolean(12); // == 1 ? true : false;

                    matchDetails.Add(
                        new MatchDetailModel
                        {
                            Competition = reader.GetString(0),
                            Round = reader.GetString(1),
                            Date = reader.GetDateTime(2),
                            HomeTeam = reader.GetString(3),
                            HomeTeamAbbreviation = reader.GetString(4),
                            AwayTeam = reader.GetString(5),
                            AwayTeamAbbreviation = reader.GetString(6),
                            HomeGoals = reader.GetByte(7),
                            AwayGoals = reader.GetByte(8),
                            ExtraTime = extraTime,
                            HomeGoalsET = extraTime ? reader.GetByte(10) : (int?) null,
                            AwayGoalsET = extraTime ? reader.GetByte(11) : (int?) null,
                            PenaltyShootout = penaltyShootout,
                            HomePenaltiesTaken = penaltyShootout ? reader.GetByte(13) : (int?) null,
                            HomePenaltiesScored = penaltyShootout ? reader.GetByte(14) : (int?) null,
                            AwayPenaltiesTaken = penaltyShootout ? reader.GetByte(15) : (int?) null,
                            AwayPenaltiesScored = penaltyShootout ? reader.GetByte(16) : (int?) null
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

            var matchDetails = GetLeagueMatchDetails(conn, tier, seasonStartYear, seasonEndYear);
            var pointDeductions = GetPointDeductions(conn, tier, season);

            var teams = matchDetails.Select(m => m.HomeTeam).Distinct().ToList();

            var positions = new List<LeaguePosition>();

            var dates = matchDetails.Select(m => m.Date).Distinct().OrderBy(m => m.Date).ToList();
            var lastDate = dates.Last().AddDays(1);
            var firstDate = dates.First();

            for (var dt = firstDate; dt <= lastDate; dt = dt.AddDays(1))
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
                    .ThenByDescending(t => t.GoalDifference) // Goal ratio was used prior to 1976-77
                    .ThenByDescending(t => t.GoalsFor)
                    // head to head
                    .ThenBy(t => t.Team) // unless it affects a promotion/relegation spot at the end of the season in which case a play-off occurs (this has never happened)
                    .ToList();
        }
    }
}
