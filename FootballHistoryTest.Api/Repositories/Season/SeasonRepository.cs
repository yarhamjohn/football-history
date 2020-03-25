using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using FootballHistoryTest.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballHistoryTest.Api.Repositories.Season
{
    public class SeasonRepository : ISeasonRepository
    {
        private SeasonRepositoryContext Context { get; }

        public SeasonRepository(SeasonRepositoryContext context)
        {
            Context = context;
        }
        
        public List<SeasonModel> GetSeasonModels()
        {
            using var conn = Context.Database.GetDbConnection();
            
            var cmd = GetDbCommand(conn);
            return GetTierBySeason(cmd);
        }
        
        private static List<SeasonModel> GetTierBySeason(DbCommand cmd)
        {
            var result = new List<SeasonModel>();
            
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new SeasonModel
                        {
                            SeasonStartYear = reader.GetInt32(0), 
                            SeasonEndYear = reader.GetInt32(1), 
                            Tier = reader.GetByte(2), 
                            Name = reader.GetString(3)
                        });
                }
            }

            return result;
        }

        private static DbCommand GetDbCommand(DbConnection conn)
        {
            const string sql = @"
SELECT s.StartYear, s.EndYear, d.Tier, d.Name FROM [dbo].[LeagueStatuses] AS s
INNER JOIN dbo.Divisions AS d ON s.DivisionId = d.Id
";
            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            return cmd;
        }
    }
}
