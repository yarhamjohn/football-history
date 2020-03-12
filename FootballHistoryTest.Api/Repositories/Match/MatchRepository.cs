using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using FootballHistoryTest.Api.Controllers;
using FootballHistoryTest.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballHistoryTest.Api.Repositories.Match
{
    public class MatchRepository : IMatchRepository
    {
        private MatchRepositoryContext Context { get; }

        public MatchRepository(MatchRepositoryContext context)
        {
            Context = context;
        }

        public List<MatchModel> GetLeagueMatchModels(List<int> seasonStartYears, List<int> tiers, List<string> teams)
        {
            using var conn = Context.Database.GetDbConnection();
            
            var cmd = GetLeagueMatchDbCommand(conn, seasonStartYears, tiers, teams);
            return GetLeagueMatches(cmd);
        }

        public List<MatchModel> GetPlayOffMatchModels(int seasonStartYear, int tier)
        {
            using var conn = Context.Database.GetDbConnection();
            
            var cmd = GetPlayOffMatchDbCommand(conn, seasonStartYear, tier);
            return GetPlayOffMatches(cmd);
        }

        public List<MatchModel> GetLeagueHeadToHeadMatchModels(List<int> seasonStartYears, List<int> tiers, string teamOne, string teamTwo)
        {
            using var conn = Context.Database.GetDbConnection();

            var cmd = GetLeagueMatchDbCommand(conn, seasonStartYears, tiers, new List<string> {teamOne, teamTwo});
            return GetLeagueMatches(cmd)
                .Where(m => m.HomeTeam == teamOne && m.AwayTeam == teamTwo || m.HomeTeam == teamTwo && m.AwayTeam == teamOne)
                .ToList();
        }

        private List<MatchModel> GetLeagueMatches(DbCommand cmd)
        {
            var result = new List<MatchModel>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new MatchModel
                {
                    Tier = reader.GetByte(0),
                    Division = reader.GetString(1),
                    Date = reader.GetDateTime(2),
                    HomeTeam = reader.GetString(3),
                    HomeTeamAbbreviation = reader.GetString(4),
                    AwayTeam = reader.GetString(5),
                    AwayTeamAbbreviation = reader.GetString(6),
                    HomeGoals = reader.GetByte(7),
                    AwayGoals = reader.GetByte(8)
                });
            }

            return result;
        }
        
        private List<MatchModel> GetPlayOffMatches(DbCommand cmd)
        {
            var result = new List<MatchModel>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new MatchModel
                {
                    Tier = reader.GetByte(0),
                    Division = reader.GetString(1),
                    Date = reader.GetDateTime(2),
                    Round = reader.GetString(3),
                    HomeTeam = reader.GetString(4),
                    HomeTeamAbbreviation = reader.GetString(5),
                    AwayTeam = reader.GetString(6),
                    AwayTeamAbbreviation = reader.GetString(7),
                    HomeGoals = reader.GetByte(8),
                    AwayGoals = reader.GetByte(9),
                    ExtraTime = reader.GetBoolean(10),
                    HomeGoalsExtraTime = reader.GetByte(11),
                    AwayGoalsExtraTime = reader.GetByte(12),
                    PenaltyShootout = reader.GetBoolean(13),
                    HomePenaltiesTaken = reader.GetByte(14),
                    HomePenaltiesScored = reader.GetByte(15),
                    AwayPenaltiesTaken = reader.GetByte(16),
                    AwayPenaltiesScored = reader.GetByte(17)
                });
            }

            return result;
        }

        private static DbCommand GetLeagueMatchDbCommand(DbConnection conn, List<int> seasonStartYears, List<int> tiers, List<string> teams)
        {            
            conn.Open();
            var cmd = conn.CreateCommand();
            
            var whereClause = BuildWhereClause(seasonStartYears, tiers, teams);
            var sql = $@"
SELECT d.Tier
      ,d.Name
      ,lm.MatchDate
      ,hc.Name
      ,hc.Abbreviation
      ,ac.Name
      ,ac.Abbreviation
      ,lm.HomeGoals
      ,lm.AwayGoals
FROM [dbo].[LeagueMatches] AS lm
INNER JOIN [dbo].[Divisions] AS d
  ON lm.DivisionId = d.Id
INNER JOIN [dbo].[Clubs] AS hc
  ON lm.HomeClubId = hc.Id
INNER JOIN [dbo].[Clubs] AS ac
  ON lm.AwayClubId = ac.Id
{whereClause}
";

            cmd.CommandText = sql;

            for (var i = 0; i < tiers.Count; i++)
            {
                cmd.Parameters.Add(new SqlParameter {ParameterName = $"@Tier{i}", Value = tiers[i]});
            }

            for (var i = 0; i < teams.Count; i++)
            {
                cmd.Parameters.Add(new SqlParameter {ParameterName = $"@Team{i}", Value = teams[i]});
            }
            
            for (var i = 0; i < seasonStartYears.Count; i++)
            {
                cmd.Parameters.Add(new SqlParameter {ParameterName = $"@SeasonStartYear{i}", Value = seasonStartYears[i]});
                cmd.Parameters.Add(new SqlParameter {ParameterName = $"@SeasonEndYear{i}", Value = seasonStartYears[i] + 1});
            }

            return cmd;
        }
        
        private static DbCommand GetPlayOffMatchDbCommand(DbConnection conn, int seasonStartYear, int tier)
        {            
            conn.Open();
            var cmd = conn.CreateCommand();

            var sql = $@"
SELECT d.Tier
      ,d.Name
      ,p.MatchDate
      ,p.Round
      ,hc.Name
      ,hc.Abbreviation
      ,ac.Name
      ,ac.Abbreviation
      ,p.HomeGoals
      ,p.AwayGoals
      ,p.ExtraTime
      ,p.HomeGoalsET
      ,p.AwayGoalsET
      ,p.PenaltyShootout
      ,p.HomePenaltiesTaken
      ,p.HomePenaltiesScored
      ,p.AwayPenaltiesTaken
      ,p.AwayPenaltiesScored
FROM [dbo].[PlayOffMatches] AS p
INNER JOIN [dbo].[Divisions] AS d
  ON p.DivisionId = d.Id
INNER JOIN [dbo].[Clubs] AS hc
  ON p.HomeClubId = hc.Id
INNER JOIN [dbo].[Clubs] AS ac
  ON p.AwayClubId = ac.Id
WHERE d.Tier = @Tier 
  AND p.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)
";

            cmd.CommandText = sql;

            cmd.Parameters.Add(new SqlParameter {ParameterName = "@Tier", Value = tier});
            cmd.Parameters.Add(new SqlParameter {ParameterName = "@SeasonStartYear", Value = seasonStartYear});
            cmd.Parameters.Add(new SqlParameter {ParameterName = "@SeasonEndYear", Value = seasonStartYear + 1});

            return cmd;
        }
        
        private static string BuildWhereClause(List<int> seasonStartYears, List<int> tiers, List<string> teams)
        {
            var clauses = new List<string>();
            for (var i = 0; i < tiers.Count; i++)
            {
                clauses.Add($"d.Tier = @Tier{i}");
            }
            
            for (var i = 0; i < teams.Count; i++)
            {
                clauses.Add($"(hc.Name = @Team{i} OR ac.Name = @Team{i})");
            }
            
            for (var i = 0; i < seasonStartYears.Count; i++)
            {
                clauses.Add($"lm.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear{i}, 7, 1) AND DATEFROMPARTS(@SeasonEndYear{i}, 6, 30)");
            }

            return clauses.Count > 0 ? $"WHERE {string.Join(" AND ", clauses)}" : "";
        }
    }
}
