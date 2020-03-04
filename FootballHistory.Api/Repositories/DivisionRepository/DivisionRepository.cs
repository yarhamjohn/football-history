using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using FootballHistory.Api.Controllers;
using FootballHistory.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Repositories.DivisionRepository
{
    public class DivisionRepository : IDivisionRepository
    {
        private DivisionRepositoryContext Context { get; }

        public DivisionRepository(DivisionRepositoryContext context)
        {
            Context = context;
        }
        
        public List<DivisionModel> GetDivisions()
        {
            using var conn = Context.Database.GetDbConnection();
            var cmd = GetDbCommand(conn);
            return GetDivisions(cmd);
        }
        
        public List<DivisionModel> GetDivisions(int tier)
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
                        Tier = (Tier) reader.GetByte(1),
                        From = reader.GetInt16(2),
                        To = reader.IsDBNull(3) ? DateTime.UtcNow.Year : reader.GetInt16(3)
                    }
                );
            }

            return divisionModels;
        }

        private static DbCommand GetDbCommand(DbConnection conn)
        {
            const string sql = @"
SELECT [Name]
      ,[Tier]
      ,[From]
      ,[To]
  FROM [dbo].[Divisions]
";
            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            
            return cmd;
        }

        private static DbCommand GetDbCommand(DbConnection conn, int tier)
        {
            const string sql = @"
SELECT [Name]
      ,[Tier]
      ,[From]
      ,[To]
  FROM [dbo].[Divisions]
";
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            
            var tierParameter = new SqlParameter {ParameterName = "@Tier", Value = tier};
            cmd.Parameters.Add(tierParameter);
            
            return cmd;
        }
    }
}
