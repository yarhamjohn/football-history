using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace football_history.Server.Repositories
{
    public class FootballHistoryRepository : IFootballHistoryRepository
    {
        private FootballHistoryContext m_Context { get; }

        public FootballHistoryRepository(FootballHistoryContext context)
        {
            m_Context = context;
        }

        public LeagueTable GetLeague()
        {
            var sql = "SELECT COUNT(*) FROM dbo.Matches";
            var result = 0;

            using(var conn = m_Context.Database.GetDbConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                result = (Int32) cmd.ExecuteScalar();
            }


            var league = new LeagueTable 
            {
                Competition = $"{result} Premier League",
                Season = "2015 - 2016",
                LeagueTableRow = new List<LeagueTableRow> 
                {
                    new LeagueTableRow 
                    {
                        Position = 1,
                        Team = "Team 1",
                        Played = 38,
                        Won = 28,
                        Drawn = 4,
                        Lost = 6,
                        GoalsFor = 100,
                        GoalsAgainst = 50,
                        GoalDifference = 50,
                        Points = 88
                    },
                    new LeagueTableRow 
                    {
                        Position = 2,
                        Team = "Team 2",
                        Played = 38,
                        Won = 28,
                        Drawn = 3,
                        Lost = 7,
                        GoalsFor = 100,
                        GoalsAgainst = 50,
                        GoalDifference = 50,
                        Points = 87
                    }
                }
            };

            return league;
        }
    }
}