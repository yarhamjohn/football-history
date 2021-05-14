using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace football.history.api.Repositories.Match
{
    public interface IMatchCommandBuilder
    {
        public DbCommand Build(IDatabaseConnection connection, long matchId);

        public DbCommand Build(
            IDatabaseConnection connection,
            long? competitionId = null,
            long? seasonId = null,
            long? teamId = null,
            string? type = null,
            DateTime? matchDate = null);
    }

    public class MatchCommandBuilder : IMatchCommandBuilder
    {
        public DbCommand Build(IDatabaseConnection connection, long matchId)
        {
            var sql = GetSql("WHERE m.Id = @Id");
            var cmd = BuildCommand(connection, sql);
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@Id",
                    Value         = matchId
                });

            return cmd;
        }

        public DbCommand Build(
            IDatabaseConnection connection, 
            long? competitionId,
            long? seasonId,
            long? teamId,
            string? type,
            DateTime? matchDate)
        {
            var whereClause = BuildWhereClause(competitionId, seasonId, teamId, type, matchDate);
            var sql = GetSql(whereClause);
            var cmd = BuildCommand(connection, sql);

            AddParameters(cmd, competitionId, seasonId, teamId, type, matchDate);

            return cmd;
        }

        private string BuildWhereClause(
            long? competitionId, 
            long? seasonId, 
            long? teamId, 
            string? type, 
            DateTime? matchDate)
        {
            var clauses = new List<string>();

            if (competitionId is not null)
            {
                clauses.Add("c.Id = @CompetitionId");
            }

            if (seasonId is not null)
            {
                clauses.Add("s.Id = @SeasonId");
            }

            if (teamId is not null)
            {
                clauses.Add("(ht.Id = @HomeTeamId OR at.Id = @AwayTeamId)");
            }

            if (type is not null)
            {
                clauses.Add("r.Type = @Type");
            }

            if (matchDate is not null)
            {
                clauses.Add("m.MatchDate < @MatchDate");
            }

            return clauses.Count > 0 ? $"WHERE {string.Join(" AND ", clauses)}" : "";
        }

        private static void AddParameters(
            DbCommand cmd,
            long? competitionId, 
            long? seasonId, 
            long? teamId, 
            string? type,
            DateTime? matchDate)
        {
            if (competitionId is not null)
            {
                cmd.Parameters.Add(
                    new SqlParameter
                    {
                        ParameterName = "@CompetitionId",
                        Value         = competitionId
                    });
            }

            if (seasonId is not null)
            {
                cmd.Parameters.Add(
                    new SqlParameter
                    {
                        ParameterName = "@SeasonId",
                        Value         = seasonId
                    });
            }

            if (teamId is not null)
            {
                cmd.Parameters.Add(
                    new SqlParameter
                    {
                        ParameterName = "@HomeTeamId",
                        Value         = teamId
                    });

                cmd.Parameters.Add(
                    new SqlParameter
                    {
                        ParameterName = "@AwayTeamId",
                        Value         = teamId
                    });
            }

            if (type is not null)
            {
                cmd.Parameters.Add(
                    new SqlParameter
                    {
                        ParameterName = "@Type",
                        Value         = type
                    });
            }

            if (matchDate is not null)
            {
                cmd.Parameters.Add(
                    new SqlParameter
                    {
                        ParameterName = "@MatchDate",
                        Value         = matchDate
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
SELECT m.Id
      ,m.MatchDate
      ,c.Id AS CompetitionId
      ,c.Name AS CompetitionName
      ,s.StartYear
      ,s.EndYear
      ,c.Tier
      ,c.Region
      ,r.Type
      ,r.Stage
      ,r.ExtraTime
      ,r.Penalties
      ,r.NumLegs
      ,r.AwayGoals
      ,r.Replays
      ,ht.Id AS HomeTeamId
      ,ht.Name As HomeTeamName
      ,ht.Abbreviation AS HomeTeamAbbreviation
      ,at.Id AS AwayTeamId
      ,at.Name AS AwayTeamName
      ,at.Abbreviation AS AwayTeamAbbreviation
      ,m.HomeGoals
      ,m.AwayGoals
      ,m.HomeGoalsET
      ,m.AwayGoalsET
      ,m.HomePenaltiesTaken
      ,m.HomePenaltiesScored
      ,m.AwayPenaltiesTaken
      ,m.AwayPenaltiesScored
FROM [dbo].[Matches] AS m
LEFT JOIN [dbo].[Competitions] AS c ON c.Id = m.CompetitionId
LEFT JOIN [dbo].[MatchRules] AS r ON r.Id = m.RulesId
LEFT JOIN [dbo].[Teams] AS ht ON ht.Id = m.HomeTeamId
LEFT JOIN [dbo].[Teams] AS at ON at.Id = m.AwayTeamId
LEFT JOIN [dbo].[Seasons] AS s ON s.Id = c.SeasonId
{whereClause}
";
    }
}