using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using FootballHistoryTest.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballHistoryTest.Api.Repositories.Match
{
    public class MatchRepository : IMatchRepository
    {
        private MatchRepositoryContext Context { get; }

        public MatchRepository(MatchRepositoryContext context)
        {
            Context = context;
        }
        
        public List<MatchModel> GetLeagueMatchModels(int seasonStartYear, int tier)
        {
            using var conn = Context.Database.GetDbConnection();
            
            var cmd = GetDbCommand(conn, seasonStartYear, tier);
            return GetMatches(cmd);
        }

        private List<MatchModel> GetMatches(DbCommand cmd)
        {
            var result = new List<MatchModel>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new MatchModel
                {
                    Tier = reader.GetByte(0),
                    Division = reader.GetString(1),
                });
            }

            return result;
        }

        private static DbCommand GetDbCommand(DbConnection conn, int seasonStartYear, int tier)
        {
            var whereClause = $"WHERE d.Tier = @Tier AND lm.MatchDate BETWEEN @SeasonStart AND @SeasonEnd";
            var sql = $@"
SELECT d.Tier
      ,d.Name
      ,lm.MatchDate
      ,hc.Name
      ,hc.Abbreviation
      ,ac.Name
      ,ac.Abbreviation
      ,lm.HomeGoals
      ,lm.AwayGoals
FROM [dbo].[LeagueMatches] AS lm
INNER JOIN [dbo].[Divisions] AS d
  ON lm.DivisionId = d.Id
INNER JOIN [dbo].[Clubs] AS hc
  ON lm.HomeClubId = hc.Id
INNER JOIN [dbo].[Clubs] AS ac
  ON lm.AwayClubId = ac.Id
{whereClause}
";
            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            
            var tierParameter = new SqlParameter {ParameterName = "@Tier", Value = tier};
            cmd.Parameters.Add(tierParameter);
            
            var seasonStartParameter = new SqlParameter {ParameterName = "@SeasonStart", Value = $"{seasonStartYear}-07-01"};
            cmd.Parameters.Add(seasonStartParameter);
            var seasonEndParameter = new SqlParameter {ParameterName = "@SeasonEnd", Value = $"{seasonStartYear + 1}-06-30"};
            cmd.Parameters.Add(seasonEndParameter);
            
            return cmd;
        }
    }
}
