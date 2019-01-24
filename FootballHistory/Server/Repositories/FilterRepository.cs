using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Server.Repositories
{
    public class FilterRepository : IFilterRepository
    {
        private const int TopTier = 1;
        private const int OldestStartYear = 1992;
        private LeagueSeasonContext Context { get; }

        public FilterRepository(LeagueSeasonContext context)
        {
            Context = context;
        }

        public DefaultFilter GetDefaultFilter()
        {
            var defaultFilter = new DefaultFilter
            {
                Tier = new Tier
                {
                    Divisions = new List<Division>(),
                    Level = TopTier
                }
            };

            var sql = @"
SELECT d.Name
    ,d.[From]
    ,d.[To]
    ,ls.Season
FROM dbo.divisions d 
CROSS JOIN (
    SELECT TOP(1) Season
    FROM dbo.LeagueStatuses 
    ORDER BY Season DESC
) ls 
WHERE d.[From] >= @OldestStartYear AND d.Tier = @TopTier
";
            using(var conn = Context.Database.GetDbConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add(new SqlParameter("@TopTier", TopTier));
                cmd.Parameters.Add(new SqlParameter("@OldestStartYear", OldestStartYear));

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        defaultFilter.Season = reader.GetString(3);
                        defaultFilter.Tier.Divisions.Add(
                            new Division
                            {
                                Name = reader.GetString(0),
                                ActiveFrom = reader.GetInt16(1),
                                ActiveTo = reader.IsDBNull(2) ? DateTime.UtcNow.Year : reader.GetInt16(2)
                            }
                        );
                    }
                } 
                else 
                {
                    Console.WriteLine("No rows found");
                }
                reader.Close();
            }

            return defaultFilter;
        }
    }
}
