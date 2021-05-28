using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace football.history.api.Repositories.Competition
{
    public interface ICompetitionCommandBuilder
    {
        public DbCommand Build(
            IDatabaseConnection connection,
            long? competitionId = null,
            long? seasonId = null,
            int? tier = null);
        
        public DbCommand BuildForCompetitionId(IDatabaseConnection connection, long seasonId, long teamId);
    }

    public class CompetitionCommandBuilder : ICompetitionCommandBuilder
    {
        public DbCommand Build(
            IDatabaseConnection connection, 
            long? competitionId = null,
            long? seasonId = null,
            int? tier = null)
        {
            var whereClause = BuildWhereClause(competitionId, seasonId, tier);
            var sql = GetSql(whereClause);
            var cmd = BuildCommand(connection, sql);
            AddParameters(cmd, competitionId, seasonId, tier);

            return cmd;
        }

        public DbCommand BuildForCompetitionId(
            IDatabaseConnection connection, 
            long seasonId, 
            long teamId)
        {
            const string sql = @"
SELECT TOP(1) m.CompetitionId
FROM [dbo].[Matches] AS m
INNER JOIN (
    SELECT c.Id
    FROM [dbo].[Competitions] AS c
    WHERE c.SeasonId = @SeasonId
) AS c1
ON c1.Id = m.CompetitionId
WHERE m.HomeTeamId = @TeamId
";
            
            var cmd = BuildCommand(connection, sql);
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@SeasonId",
                    Value         = seasonId
                });
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@TeamId",
                    Value         = teamId
                });

            return cmd;
        }

        private string BuildWhereClause(
            long? competitionId, 
            long? seasonId, 
            int? tier)
        {
            var clauses = new List<string>();

            if (competitionId != null)
            {
                clauses.Add("c.Id = @CompetitionId");
            }

            if (seasonId != null)
            {
                clauses.Add("s.Id = @SeasonId");
            }

            if (tier != null)
            {
                clauses.Add("c.Tier = @Tier");
            }

            return clauses.Count > 0 ? $"WHERE {string.Join(" AND ", clauses)}" : "";
        }

        private static void AddParameters(
            DbCommand cmd,
            long? competitionId, 
            long? seasonId, 
            int? tier)
        {
            if (competitionId != null)
            {
                cmd.Parameters.Add(
                    new SqlParameter
                    {
                        ParameterName = "@CompetitionId",
                        Value         = competitionId
                    });
            }

            if (seasonId != null)
            {
                cmd.Parameters.Add(
                    new SqlParameter
                    {
                        ParameterName = "@SeasonId",
                        Value         = seasonId
                    });
            }

            if (tier != null)
            {
                cmd.Parameters.Add(
                    new SqlParameter
                    {
                        ParameterName = "@Tier",
                        Value         = tier
                    });
            }
        }

        private static DbCommand BuildCommand(IDatabaseConnection connection, string sql)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            return cmd;
        }

        private static string GetSql(string whereClause) => $@"
SELECT c.Id,
       c.Name,
       s.Id AS SeasonId,
	   s.StartYear,
	   s.EndYear,
       c.Tier,
       c.Region,
       c.Comment,
  	   cr.PointsForWin,
	   cr.TotalPlaces,
	   cr.PromotionPlaces,
	   cr.RelegationPlaces,
	   cr.PlayOffPlaces,
	   cr.RelegationPlayOffPlaces,
	   cr.ReElectionPlaces,
	   cr.FailedReElectionPosition
FROM [dbo].[Competitions] AS c
LEFT JOIN dbo.CompetitionRules AS cr ON cr.Id = c.RulesId
LEFT JOIN dbo.Seasons AS s ON s.Id = c.SeasonId
{whereClause}
";
    }
}