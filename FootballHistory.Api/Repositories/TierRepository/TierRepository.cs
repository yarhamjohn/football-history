using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using FootballHistory.Api.Domain;
using FootballHistory.Api.Repositories.DivisionRepository;
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
        
        public int GetTier(string season, string team)
        {
            using (var conn = Context.Database.GetDbConnection())
            {
                var cmd = GetDbCommand(conn, season, team);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private static DbCommand GetDbCommand(DbConnection conn, string season, string team)
        {
            const string sql = @"
SELECT TOP(1) d.Tier
FROM [dbo].[LeagueMatches] AS lm
INNER JOIN [dbo].[Divisions] AS d
  ON d.Id = lm.DivisionId
INNER JOIN [dbo].[Clubs] AS c
  ON c.Id = lm.HomeClubId
WHERE lm.MatchDate BETWEEN @StartDate AND @EndDate
  AND c.Name = @TeamName
";
            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@StartDate", new DateTime(Convert.ToInt32(season.Substring(0, 4)), 7, 1)));
            cmd.Parameters.Add(new SqlParameter("@EndDate", new DateTime(Convert.ToInt32(season.Substring(7, 4)), 6, 30)));
            cmd.Parameters.Add(new SqlParameter("@TeamName", team));

            return cmd;
        }
    }
}
