using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using FootballHistoryTest.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballHistoryTest.Api.Repositories.Match
{
    public class MatchRepository : IMatchRepository
    {
        private readonly DatabaseContext _context;

        public MatchRepository(DatabaseContext context)
        {
            _context = context;
        }

        public List<MatchModel> GetLeagueMatchModels(int seasonStartYear, int tier)
        {
            return GetLeagueMatchModels(new List<int> {seasonStartYear}, new List<int> {tier}, new List<string>());
        }
        
        public List<MatchModel> GetLeagueMatchModels(List<int> seasonStartYears, List<int> tiers)
        {
            return GetLeagueMatchModels(seasonStartYears, tiers, new List<string>());
        }

        public List<MatchModel> GetLeagueMatchModels(List<int> seasonStartYears, List<int> tiers, List<string> teams)
        {
            var conn = _context.Database.GetDbConnection();
            var cmd = GetLeagueMatchDbCommand(conn, seasonStartYears, tiers, teams);
            var result = GetLeagueMatches(cmd);
            conn.Close();
            return result;
        }

        public List<MatchModel> GetPlayOffMatchModels(int seasonStartYear, int tier)
        {
            return GetPlayOffMatchModels(new List<int> { seasonStartYear }, new List<int> {tier});
        }

        public List<MatchModel> GetPlayOffMatchModels(List<int> seasonStartYears, List<int> tiers)
        {
            var conn = _context.Database.GetDbConnection();
            var cmd = GetPlayOffMatchDbCommand(conn, seasonStartYears, tiers);
            var result = GetPlayOffMatches(cmd);
            conn.Close();
            return result;
        }

        public List<MatchModel> GetLeagueHeadToHeadMatchModels(List<int> seasonStartYears, List<int> tiers, string teamOne, string teamTwo)
        {
            var conn = _context.Database.GetDbConnection();
            var cmd = GetLeagueMatchDbCommand(conn, seasonStartYears, tiers, new List<string> {teamOne, teamTwo});
            var result = GetLeagueMatches(cmd)
                .Where(m => m.HomeTeam == teamOne && m.AwayTeam == teamTwo || m.HomeTeam == teamTwo && m.AwayTeam == teamOne)
                .ToList();
            
            conn.Close();
            return result;
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
      ,m.MatchDate
      ,hc.Name
      ,hc.Abbreviation
      ,ac.Name
      ,ac.Abbreviation
      ,m.HomeGoals
      ,m.AwayGoals
FROM [dbo].[LeagueMatches] AS m
INNER JOIN [dbo].[Divisions] AS d
  ON m.DivisionId = d.Id
INNER JOIN [dbo].[Clubs] AS hc
  ON m.HomeClubId = hc.Id
INNER JOIN [dbo].[Clubs] AS ac
  ON m.AwayClubId = ac.Id
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
        
        private static DbCommand GetPlayOffMatchDbCommand(DbConnection conn, List<int> seasonStartYears, List<int> tiers)
        {            
            conn.Open();
            var cmd = conn.CreateCommand();
            
            var whereClause = BuildWhereClause(seasonStartYears, tiers, new List<string>());
            var sql = $@"
SELECT d.Tier
      ,d.Name
      ,m.MatchDate
      ,m.Round
      ,hc.Name
      ,hc.Abbreviation
      ,ac.Name
      ,ac.Abbreviation
      ,m.HomeGoals
      ,m.AwayGoals
      ,m.ExtraTime
      ,m.HomeGoalsET
      ,m.AwayGoalsET
      ,m.PenaltyShootout
      ,m.HomePenaltiesTaken
      ,m.HomePenaltiesScored
      ,m.AwayPenaltiesTaken
      ,m.AwayPenaltiesScored
FROM [dbo].[PlayOffMatches] AS m
INNER JOIN [dbo].[Divisions] AS d
  ON m.DivisionId = d.Id
INNER JOIN [dbo].[Clubs] AS hc
  ON m.HomeClubId = hc.Id
INNER JOIN [dbo].[Clubs] AS ac
  ON m.AwayClubId = ac.Id
{whereClause}
";

            cmd.CommandText = sql;

            for (var i = 0; i < tiers.Count; i++)
            {
                cmd.Parameters.Add(new SqlParameter {ParameterName = $"@Tier{i}", Value = tiers[i]});
            }
            
            for (var i = 0; i < seasonStartYears.Count; i++)
            {
                cmd.Parameters.Add(new SqlParameter {ParameterName = $"@SeasonStartYear{i}", Value = seasonStartYears[i]});
                cmd.Parameters.Add(new SqlParameter {ParameterName = $"@SeasonEndYear{i}", Value = seasonStartYears[i] + 1});
            }

            return cmd;
        }
        
        private static string BuildWhereClause(List<int> seasonStartYears, List<int> tiers, List<string> teams)
        {
            var clauses = new List<string>();
            var tierClauses = new List<string>();
            for (var i = 0; i < tiers.Count; i++)
            {
                tierClauses.Add($"d.Tier = @Tier{i}");
            }

            if (tierClauses.Count > 1)
            {
                clauses.Add("(" + string.Join(" OR ", tierClauses) + ")");
            }

            if (tierClauses.Count == 1)
            {
                clauses.Add(tierClauses.Single());
            }

            var teamClauses = new List<string>();
            for (var i = 0; i < teams.Count; i++)
            {
                teamClauses.Add($"(hc.Name = @Team{i} OR ac.Name = @Team{i})");
            }

            if (teamClauses.Count > 1)
            {
                clauses.Add("(" + string.Join(" OR ", teamClauses) + ")");
            }

            if (teamClauses.Count == 1)
            {
                clauses.Add(teamClauses.Single());
            }
            
            var seasonClauses = new List<string>();
            for (var i = 0; i < seasonStartYears.Count; i++)
            {
                seasonClauses.Add($"m.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear{i}, 7, 1) AND DATEFROMPARTS(@SeasonEndYear{i}, 6, 30)");
            }

            if (seasonClauses.Count > 1)
            {
                clauses.Add("(" + string.Join(" OR ", seasonClauses) + ")");
            }

            if (seasonClauses.Count == 1)
            {
                clauses.Add(seasonClauses.Single());
            }
            
            return clauses.Count > 0 ? $"WHERE {string.Join(" AND ", clauses)}" : "";
        }
    }
}
