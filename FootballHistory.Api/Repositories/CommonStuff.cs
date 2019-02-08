using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Repositories
{
    public static class CommonStuff
    {
        public static void AddLeagueRows(List<LeagueTableRow> leagueTable, List<MatchDetailModel> leagueMatchDetails)
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

        public static void IncludePointDeductions(List<LeagueTableRow> table, List<PointDeduction> pointDeductions)
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
        
        public static void SetLeaguePosition(List<LeagueTableRow> table)
        {
            table = table.Select((t, i) => { 
                t.Position = i + 1; 
                return t; 
            }).ToList();
        }
        
        public static List<MatchDetailModel> GetLeagueMatchDetails(DbConnection conn, int tier, string seasonStartYear, string seasonEndYear)
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

        public static List<PointDeduction> GetPointDeductions(DbConnection conn, int tier, string season)
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

        public static List<LeagueTableRow> SortLeagueTable(List<LeagueTableRow> leagueTable)
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
