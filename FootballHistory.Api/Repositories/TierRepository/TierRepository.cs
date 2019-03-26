using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using FootballHistory.Api.Controllers;
using FootballHistory.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Repositories.TierRepository
{
    public class TierRepository : ITierRepository
    {
        private TierRepositoryContext Context { get; }

        public TierRepository(TierRepositoryContext context)
        {
            Context = context;
        }
        
        public SeasonTierFilter[] GetSeasonTierFilters(string team, int seasonStartYear, int seasonEndYear)
        {
            using (var conn = Context.Database.GetDbConnection())
            {
                var cmd = GetDbCommand(conn, team, seasonStartYear, seasonEndYear);
                return GetTierBySeason(cmd, seasonStartYear, seasonEndYear);
            }
        }
        
        private static SeasonTierFilter[] GetTierBySeason(DbCommand cmd, int seasonStartYear, int seasonEndYear)
        {
            var result = new List<SeasonTierFilter>();
            
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(
                        new SeasonTierFilter
                        {
                            Tier = (Tier) reader.GetByte(0),
                            SeasonStartYear = reader.GetInt32(1)
                        }
                    );
                }
            }
            
            for (var year = seasonStartYear; year <= seasonEndYear; year++)
            {
                if (result.All(r => r.SeasonStartYear != year))
                {
                    result.Add(
                        new SeasonTierFilter
                        {
                            Tier = Tier.UnknownTier,
                            SeasonStartYear = year
                        }
                    );
                }
            }

            return result.ToArray();
        }

        private static DbCommand GetDbCommand(DbConnection conn, string team, int seasonStartYear, int seasonEndYear)
        {
            const string sql = @"
SELECT Tier, SeasonStartYear
FROM (
    SELECT Tier, SeasonStartYear
    FROM (
        SELECT d.Tier
          ,CASE WHEN MONTH(lm.MatchDate) >= 7 
                THEN YEAR(lm.MatchDate)
                ELSE YEAR(lm.MatchDate) - 1 
                END AS SeasonStartYear
        FROM [dbo].[LeagueMatches] lm
        INNER JOIN dbo.Divisions d ON d.Id = lm.DivisionId
        INNER JOIN dbo.Clubs c ON c.Id = lm.HomeClubId
        WHERE c.Name = @TeamName
    ) AS a
    GROUP BY a.Tier, a.SeasonStartYear
) AS b
WHERE b.SeasonStartYear BETWEEN @SeasonStartYear AND @SeasonEndYear
";
            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@TeamName", team));
            cmd.Parameters.Add(new SqlParameter("@SeasonStartYear", seasonStartYear));
            cmd.Parameters.Add(new SqlParameter("@SeasonEndYear", seasonEndYear));

            return cmd;
        }
    }
}
