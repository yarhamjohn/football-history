using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using football.history.api.Calculators;
using football.history.api.Domain;
using football.history.api.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace football.history.api.Repositories.League
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly DatabaseContext _context;
        private readonly IDateCalculator _dateCalculator;

        public LeagueRepository(DatabaseContext context, IDateCalculator dateCalculator)
        {
            _context = context;
            _dateCalculator = dateCalculator;
        }

        public LeagueModel GetLeagueModel(int seasonStartYear, int tier)
        {
            var leagueModels = GetLeagueModels(
                new List<int> { seasonStartYear },
                new List<int> { tier });

            if (!leagueModels.Any())
            {
                throw new LeagueModelNotFoundException($"No league model was found for seasonStartYear: {seasonStartYear} and tier {tier}.");
            }

            if (leagueModels.Count > 1)
            {
                throw new MultipleLeagueModelsFoundException($"Multiple league models were found for seasonStartYear: {seasonStartYear} and tier {tier}.");
            }

            return leagueModels.Single();
        }

        public LeagueModel GetLeagueModel(DateTime date, int tier)
        {
            var seasonStartYear = _dateCalculator.GetSeasonStartYear(date);
            return GetLeagueModel(seasonStartYear, tier);
        }

        public List<LeagueModel> GetLeagueModels(List<int> seasonStartYears, List<int> tiers)
        {
            var conn = _context.Database.GetDbConnection();
            var cmd = GetDbCommand(conn, seasonStartYears, tiers);
            var result = GetLeague(cmd);
            conn.Close();
            return result;
        }

        private static List<LeagueModel> GetLeague(DbCommand cmd)
        {
            var result = new List<LeagueModel>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(
                    new LeagueModel
                    {
                        Name = reader.GetString(0),
                        Tier = reader.GetByte(1),
                        TotalPlaces = reader.GetByte(2),
                        PromotionPlaces = reader.GetByte(3),
                        PlayOffPlaces = reader.GetByte(4),
                        RelegationPlaces = reader.GetByte(5),
                        RelegationPlayOffPlaces = reader.GetByte(6),
                        ReElectionPlaces = reader.GetByte(7),
                        FailedReElectionPosition = reader.IsDBNull(8) ? (byte?) null : reader.GetByte(8),
                        PointsForWin = reader.GetByte(9),
                        StartYear = reader.GetInt32(10)
                    });
            }

            return result;
        }

        private static DbCommand GetDbCommand(
            DbConnection conn,
            List<int> seasonStartYears,
            List<int> tiers)
        {
            var whereClause = BuildWhereClause(tiers, seasonStartYears);

            var sql = $@"
SELECT d.Name
      ,d.Tier
      ,[TotalPlaces]
      ,[PromotionPlaces]
      ,[PlayOffPlaces]
      ,[RelegationPlaces]
      ,[RelegationPlayOffPlaces]
      ,[ReElectionPlaces]
      ,[FailedReElectionPosition]
      ,[PointsForWin]
      ,[StartYear]
  FROM [dbo].[LeagueStatuses] AS ls
  LEFT JOIN [dbo].[Divisions] AS d ON ls.[DivisionId] = d.[Id]
  {whereClause}
";
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            for (var i = 0; i < tiers.Count; i++)
            {
                var tierParameter = new SqlParameter
                {
                    ParameterName = $"@Tier{i}",
                    Value = tiers[i]
                };
                cmd.Parameters.Add(tierParameter);
            }

            for (var i = 0; i < seasonStartYears.Count; i++)
            {
                var seasonStartYearParameter = new SqlParameter
                {
                    ParameterName = $"@SeasonStartYear{i}",
                    Value = seasonStartYears[i]
                };
                cmd.Parameters.Add(seasonStartYearParameter);
            }

            return cmd;
        }

        private static string BuildWhereClause(List<int> tiers, List<int> seasonStartYears)
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

            var seasonClauses = new List<string>();
            for (var i = 0; i < seasonStartYears.Count; i++)
            {
                seasonClauses.Add($"ls.[StartYear] = @SeasonStartYear{i}");
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
