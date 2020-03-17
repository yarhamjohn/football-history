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
            return GetLeagueModels(seasonStartYear, new List<int> { tier }).Single();
        }
        
        public List<LeagueModel> GetLeagueModels(int seasonStartYear, List<int> tiers)
        {
            using var conn = Context.Database.GetDbConnection();
            var cmd = GetDbCommand(conn, seasonStartYear, tiers);

            return GetLeague(cmd);
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

        private static DbCommand GetDbCommand(DbConnection conn, int seasonStartYear, List<int> tiers)
        {
            var whereClause = BuildWhereClause(tiers);
            
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

            for (var i = 0; i < tiers.Count; i++)
            {
                var tierParameter = new SqlParameter {ParameterName = $"@Tier{i}", Value = tiers[i]};
                cmd.Parameters.Add(tierParameter);
            }
            
            var seasonStartYearParameter = new SqlParameter {ParameterName = "@SeasonStartYear", Value = seasonStartYear};
            cmd.Parameters.Add(seasonStartYearParameter);

            return cmd;
        }
        
        private static string BuildWhereClause(List<int> tiers)
        {
            var clauses = new List<string> { "WHERE ls.[StartYear] = @SeasonStartYear" };
            var tierClauses = new List<string>();
            for (var i = 0; i < tiers.Count; i++)
            {
                tierClauses.Add($"d.Tier = @Tier{i}");
            }

            if (tierClauses.Count > 1)
            {
                clauses.Add("(" + string.Join(" OR ", tierClauses) + ")");
            }

            if (tierClauses.Count == 1)
            {
                clauses.Add(tierClauses.Single());
            }

            return string.Join(" AND ", clauses);
        }
    }
}
