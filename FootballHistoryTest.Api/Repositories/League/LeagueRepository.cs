using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using FootballHistoryTest.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballHistoryTest.Api.Repositories.League
{
    public class LeagueRepository : ILeagueRepository
    {
        private LeagueRepositoryContext Context { get; }

        public LeagueRepository(LeagueRepositoryContext context)
        {
            Context = context;
        }
        
        public LeagueModel GetLeagueModel(int seasonStartYear, int tier)
        {
            using var conn = Context.Database.GetDbConnection();

            var cmd = GetDbCommand(conn, seasonStartYear, tier);
            return GetLeague(cmd).Single();
        }

        private static List<LeagueModel> GetLeague(DbCommand cmd)
        {
            var result = new List<LeagueModel>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new LeagueModel
                {
                    Name = reader.GetString(0),
                    Tier = reader.GetByte(1),
                    TotalPlaces = reader.GetByte(2),
                    PromotionPlaces = reader.GetByte(3),
                    PlayOffPlaces = reader.GetByte(4),
                    RelegationPlaces = reader.GetByte(5),
                    PointsForWin = reader.GetByte(6),
                    StartYear = reader.GetInt32(7)
                });
            }

            return result;
        }

        private static DbCommand GetDbCommand(DbConnection conn, int seasonStartYear, int? tier = null)
        {
            var whereClause = tier == null
                ? "WHERE ls.[EndYear] = @SeasonStartYear"
                : "WHERE d.[Tier] = @Tier AND ls.[StartYear] = @SeasonStartYear";
            
            var sql = $@"
SELECT d.Name
      ,d.Tier
      ,[TotalPlaces]
      ,[PromotionPlaces]
      ,[PlayOffPlaces]
      ,[RelegationPlaces]
      ,[PointsForWin]
      ,[StartYear]
  FROM [dbo].[LeagueStatuses] AS ls
  LEFT JOIN [dbo].[Divisions] AS d ON ls.[DivisionId] = d.[Id]
  {whereClause}
";
            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            if (tier != null)
            {
                var tierParameter = new SqlParameter {ParameterName = "@Tier", Value = tier};
                cmd.Parameters.Add(tierParameter);
            }
            
            var seasonStartYearParameter = new SqlParameter {ParameterName = "@SeasonStartYear", Value = seasonStartYear};
            cmd.Parameters.Add(seasonStartYearParameter);

            return cmd;
        }
    }
}
