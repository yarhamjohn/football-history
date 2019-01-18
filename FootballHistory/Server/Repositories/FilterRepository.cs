using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

        public FilterOptions GetFilterOptions()
        {
            var filterOptions = new FilterOptions
            {
                AllSeasons = new List<string>(),
                AllTiers = new List<Tier>()
            };

            var getSeasonsSql = @"
SELECT DISTINCT Season
FROM dbo.LeagueStatuses
WHERE CAST(SUBSTRING(Season, 1, 4) AS INT) >= @OldestStartYear
GROUP BY Season;
";

            var getTiersSql = @"
SELECT Name, Tier, [From], [To]
FROM dbo.Divisions
WHERE [From] >= @OldestStartYear;
";

            var sql = $"{getSeasonsSql} {getTiersSql}";

            using(var conn = Context.Database.GetDbConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add(new SqlParameter("@OldestStartYear", OldestStartYear));

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        filterOptions.AllSeasons.Add(
                            reader.GetString(0)
                        );
                    }

                    reader.NextResult();

                    while (reader.Read())
                    {
                        var tier = reader.GetByte(1);
                        var division = new Division
                            {
                                Name = reader.GetString(0),
                                ActiveFrom = reader.GetInt16(2),
                                ActiveTo = reader.IsDBNull(3) ? DateTime.UtcNow.Year : reader.GetInt16(3)
                            };

                        AddDivision(tier, division, filterOptions);
                    }
                } 
                else 
                {
                    Console.WriteLine("No rows found");
                }
                reader.Close();
            }

            return filterOptions;
        }

        private void AddDivision(int tier, Division division, FilterOptions leagueFilterOptions)
        {
            var tierExists = leagueFilterOptions.AllTiers.Where(t => t.Level == tier).ToList().Count == 1;
            if (tierExists)
            {
                leagueFilterOptions.AllTiers = leagueFilterOptions.AllTiers
                    .Select(t => {
                        if (t.Level == tier) {
                            t.Divisions.Add(division);
                        } 
                        return t;
                    }).ToList();
            }
            else 
            {
                leagueFilterOptions.AllTiers.Add(
                    new Tier
                    {
                        Level = tier,
                        Divisions = new List<Division> { division }
                    }
                );
            }
        }
    }
}
