using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using football.history.api.Domain;
using Microsoft.EntityFrameworkCore;

namespace football.history.api.Repositories.League
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly DatabaseContext _context;

        public LeagueRepository(DatabaseContext context)
        {
            _context = context;
        }

        public LeagueModel GetLeagueModel(int seasonStartYear, int tier) =>
            GetLeagueModels(new List<int> { seasonStartYear }, new List<int> { tier }).Single();

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
                        PointsForWin = reader.GetByte(6),
                        StartYear = reader.GetInt32(7)
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
