using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using FootballHistoryTest.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballHistoryTest.Api.Repositories.Tier
{
    public class TierRepository : ITierRepository
    {
        private readonly DatabaseContext _context;

        public TierRepository(DatabaseContext context)
        {
            _context = context;
        }

        public List<TierModel> GetTierModels(List<int> seasonStartYears, string team)
        {
            var conn = _context.Database.GetDbConnection();

            var cmd = GetDbCommand(conn,  team);
            var result = GetTiers(cmd).Where(t => seasonStartYears.Contains(t.SeasonStartYear)).ToList();
            conn.Close();
            return result;
        }
        
        private static List<TierModel> GetTiers(DbCommand cmd)
        {
            var tiers = new List<TierModel>();
            
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tiers.Add(new TierModel { SeasonStartYear = reader.GetInt32(0), Tier = reader.GetByte(1)});
                }
            }

            return tiers;
        }

        private static DbCommand GetDbCommand(DbConnection conn, string team)
        {
            const string sql = @"
SELECT TOP 1 WITH TIES YEAR(m.MatchDate) AS SeasonStartYear, d.Tier
  FROM [dbo].[LeagueMatches] AS m
LEFT JOIN dbo.Divisions AS d
  ON d.Id = m.DivisionId
LEFT JOIN dbo.Clubs AS hc
  ON hc.Id = m.HomeClubId
LEFT JOIN dbo.Clubs AS ac
  ON ac.Id = m.AwayClubId
WHERE (hc.Name = @Team OR ac.Name = @Team) AND MONTH(m.MatchDate) >= 7
ORDER BY ROW_NUMBER() OVER (PARTITION BY YEAR(m.MatchDate) ORDER BY YEAR(m.MatchDate))";

            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            
            var teamParameter = new SqlParameter
                {ParameterName = "@Team", Value = team};
            cmd.Parameters.Add(teamParameter);
            
            return cmd;
        }
    }
}
