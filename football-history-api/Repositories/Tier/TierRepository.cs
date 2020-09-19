using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using football.history.api.Domain;
using Microsoft.EntityFrameworkCore;

namespace football.history.api.Repositories.Tier
{
    public class TierRepository : ITierRepository
    {
        private readonly DatabaseContext _context;

        public TierRepository(DatabaseContext context)
        {
            _context = context;
        }

        public int? GetTierForTeamInYear(int seasonStartYear, string team)
        {
            var conn = _context.Database.GetDbConnection();

            var cmd = GetDbCommand(conn,  team, seasonStartYear);
            var result = GetTier(cmd);
            conn.Close();
            return result;
        }

        private DbCommand GetDbCommand(DbConnection conn, string team, int seasonStartYear)
        {
            const string sql = @"
SELECT DISTINCT d.Tier
  FROM [dbo].[LeagueMatches] AS m
LEFT JOIN dbo.Divisions AS d
  ON d.Id = m.DivisionId
LEFT JOIN dbo.Clubs AS hc
  ON hc.Id = m.HomeClubId
LEFT JOIN dbo.Clubs AS ac
  ON ac.Id = m.AwayClubId
WHERE (hc.Name = @Team OR ac.Name = @Team) AND m.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonStartYear + 1, 6, 30)";
            
            const string sqlFor20192020 = @"
SELECT DISTINCT d.Tier
  FROM [dbo].[LeagueMatches] AS m
LEFT JOIN dbo.Divisions AS d
  ON d.Id = m.DivisionId
LEFT JOIN dbo.Clubs AS hc
  ON hc.Id = m.HomeClubId
LEFT JOIN dbo.Clubs AS ac
  ON ac.Id = m.AwayClubId
WHERE (hc.Name = @Team OR ac.Name = @Team) AND m.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonStartYear + 1, 8, 20)";
            
            const string sqlFor20202021 = @"
SELECT DISTINCT d.Tier
  FROM [dbo].[LeagueMatches] AS m
LEFT JOIN dbo.Divisions AS d
  ON d.Id = m.DivisionId
LEFT JOIN dbo.Clubs AS hc
  ON hc.Id = m.HomeClubId
LEFT JOIN dbo.Clubs AS ac
  ON ac.Id = m.AwayClubId
WHERE (hc.Name = @Team OR ac.Name = @Team) AND m.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 8, 21) AND DATEFROMPARTS(@SeasonStartYear + 1, 6, 30)";

            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = seasonStartYear switch
            {
                2019 => sqlFor20192020,
                2020 => sqlFor20202021,
                _ => sql
            };
            
            var teamParameter = new SqlParameter {ParameterName = "@Team", Value = team};
            cmd.Parameters.Add(teamParameter);

            var seasonStartYearParameter = new SqlParameter {ParameterName = "@SeasonStartYear", Value = seasonStartYear};
            cmd.Parameters.Add(seasonStartYearParameter);
            
            return cmd;
        }

        public List<TierModel> GetTierModels(List<int> seasonStartYears, string team)
        {
            var conn = _context.Database.GetDbConnection();

            var cmd = GetDbCommand(conn,  team);
            var result = GetTiers(cmd).Where(t => seasonStartYears.Contains(t.SeasonStartYear)).ToList();
            conn.Close();
            return result;
        }

        private static int? GetTier(DbCommand cmd)
        {
            var result = cmd.ExecuteScalar();
            if (result == null)
            {
                return null;
            }
            
            return Convert.ToInt32(cmd.ExecuteScalar());
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
