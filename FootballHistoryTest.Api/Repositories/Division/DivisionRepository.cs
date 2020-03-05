using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using FootballHistoryTest.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballHistoryTest.Api.Repositories.Division
{
    public class DivisionRepository : IDivisionRepository
    {
        private DivisionRepositoryContext Context { get; }

        public DivisionRepository(DivisionRepositoryContext context)
        {
            Context = context;
        }
        
        public List<DivisionModel> GetDivisionModels(int? tier = null)
        {
            using var conn = Context.Database.GetDbConnection();
            var cmd = GetDbCommand(conn, tier);
            return GetDivisions(cmd);
        }
        
        private static List<DivisionModel> GetDivisions(DbCommand cmd)
        {
            var divisionModels = new List<DivisionModel>();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                divisionModels.Add(
                    new DivisionModel
                    {
                        Name = reader.GetString(0),
                        Tier = reader.GetByte(1),
                        From = reader.GetInt16(2),
                        To = reader.IsDBNull(3) ? DateTime.UtcNow.Year : reader.GetInt16(3)
                    }
                );
            }

            return divisionModels;
        }

        private static DbCommand GetDbCommand(DbConnection conn, int? tier = null)
        {
            var whereClause = tier == null ? "" : "WHERE [Tier] = @Tier";
            var sql = $@"
SELECT [Name]
      ,[Tier]
      ,[From]
      ,[To]
  FROM [dbo].[Divisions]
  {whereClause}
";
            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            if (tier != null)
            {
                var tierParameter = new SqlParameter {ParameterName = "@Tier", Value = tier};
                cmd.Parameters.Add(tierParameter);
            }

            return cmd;
        }
    }
}
