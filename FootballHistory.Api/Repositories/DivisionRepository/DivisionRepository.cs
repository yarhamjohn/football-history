using System;
using System.Collections.Generic;
using System.Data.Common;
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
            using (var conn = Context.Database.GetDbConnection())
            {
                var cmd = GetDbCommand(conn);
                return GetDivisions(cmd);
            }
        }

        private static List<DivisionModel> GetDivisions(DbCommand cmd)
        {
            using (var reader = cmd.ExecuteReader())
            {
                var divisionModels = new List<DivisionModel>();
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
    }
}
