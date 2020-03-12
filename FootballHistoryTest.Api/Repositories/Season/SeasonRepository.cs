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
        
        public List<SeasonDatesModel> GetSeasonDateModels()
        {
            using var conn = Context.Database.GetDbConnection();
            
            var cmd = GetDbCommand(conn);
            return GetTierBySeason(cmd);
        }
        
        private static List<SeasonDatesModel> GetTierBySeason(DbCommand cmd)
        {
            var result = new List<SeasonDatesModel>();
            
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new SeasonDatesModel
                        {SeasonStartYear = reader.GetInt32(0), SeasonEndYear = reader.GetInt32(1)});
                }
            }

            return result;
        }

        private static DbCommand GetDbCommand(DbConnection conn)
        {
            const string sql = @"SELECT StartYear, EndYear FROM [dbo].[LeagueStatuses]";
            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            return cmd;
        }
    }
}
