using System;
using System.Collections.Generic;
using FootballHistory.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Server.Repositories
{
    public class DivisionRepository : IDivisionRepository
    {
        private LeagueSeasonContext Context { get; }

        public DivisionRepository(LeagueSeasonContext context)
        {
            Context = context;
        }
        
        public List<DivisionModel> GetDivisionModels()
        {
            const string sql = @"
SELECT [Name]
      ,[Tier]
      ,[From]
      ,[To]
  FROM [dbo].[Divisions]
";

            var divisionModels = new List<DivisionModel>();
            using (var conn = Context.Database.GetDbConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;

                using (var reader = cmd.ExecuteReader())
                {
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
                }
            }

            return divisionModels;
        }
    }
}
