using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using football.history.api.Domain;
using Microsoft.EntityFrameworkCore;

namespace football.history.api.Repositories.PointDeductions
{
    public class PointsDeductionRepository : IPointsDeductionRepository
    {
        private readonly DatabaseContext _context;

        public PointsDeductionRepository(DatabaseContext context)
        {
            _context = context;
        }

        public List<PointsDeductionModel> GetPointsDeductionModels(int seasonStartYear, int tier) =>
            GetPointsDeductionModels(new List<int> { seasonStartYear }, new List<int> { tier });

        public List<PointsDeductionModel> GetPointsDeductionModels(
            List<int> seasonStartYears,
            List<int> tiers)
        {
            var conn = _context.Database.GetDbConnection();
            var cmd = GetDbCommand(conn, seasonStartYears, tiers);
            var result = GetPointDeductions(cmd);
            conn.Close();
            return result;
        }

        private static List<PointsDeductionModel> GetPointDeductions(DbCommand cmd)
        {
            var pointsDeductionModels = new List<PointsDeductionModel>();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pointsDeductionModels.Add(
                    new PointsDeductionModel
                    {
                        Team = reader.GetString(0),
                        SeasonStartYear = reader.GetInt32(1),
                        PointsDeducted = reader.GetInt16(2),
                        Reason = reader.GetString(3),
                        Tier = reader.GetByte(4)
                    });
            }

            return pointsDeductionModels;
        }

        private static DbCommand GetDbCommand(
            DbConnection conn,
            List<int> seasonStartYears,
            List<int> tiers)
        {
            var whereClause = BuildWhereClause(seasonStartYears, tiers);

            var sql = $@"
SELECT c.Name
      ,pd.StartYear
      ,pd.PointsDeducted
      ,pd.Reason
      ,d.Tier
  FROM dbo.PointDeductions AS pd
INNER JOIN dbo.Clubs AS c ON c.Id = pd.ClubId
INNER JOIN dbo.Divisions AS d ON d.Id = pd.DivisionId
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

        private static string BuildWhereClause(List<int> seasonStartYears, List<int> tiers)
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
                seasonClauses.Add($"pd.[StartYear] = @SeasonStartYear{i}");
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
