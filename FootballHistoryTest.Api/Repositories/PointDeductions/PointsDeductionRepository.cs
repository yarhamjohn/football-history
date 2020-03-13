using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using FootballHistoryTest.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballHistoryTest.Api.Repositories.PointDeductions
{
    public class PointsDeductionRepository : IPointsDeductionRepository
    {
        private PointsDeductionRepositoryContext Context { get; }

        public PointsDeductionRepository(PointsDeductionRepositoryContext context)
        {
            Context = context;
        }
        
        public List<PointsDeductionModel> GetPointsDeductionModels(int seasonStartYear, int tier)
        {
            using var conn = Context.Database.GetDbConnection();
            var cmd = GetDbCommand(conn, seasonStartYear, tier);
            return GetPointDeductions(cmd);
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
                        PointsDeducted = reader.GetByte(2),
                        Reason = reader.GetString(3)
                    }
                );
            }

            return pointsDeductionModels;
        }

        private static DbCommand GetDbCommand(DbConnection conn, int seasonStartYear, int tier)
        {
            var whereClause = "WHERE d.[Tier] = @Tier AND pd.[StartYear] = @SeasonStartYear";

            var sql = $@"
SELECT c.Name
      ,pd.StartYear
      ,pd.PointsDeducted
      ,pd.Reason
  FROM dbo.PointDeductions AS pd
INNER JOIN dbo.Clubs AS c ON c.Id = pd.ClubId
INNER JOIN dbo.Divisions AS d ON d.Id = pd.DivisionId
  {whereClause}
";
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            var tierParameter = new SqlParameter {ParameterName = "@Tier", Value = tier};
            cmd.Parameters.Add(tierParameter);

            var seasonStartYearParameter = new SqlParameter
                {ParameterName = "@SeasonStartYear", Value = seasonStartYear};
            cmd.Parameters.Add(seasonStartYearParameter);

            return cmd;
        }
    }
}
