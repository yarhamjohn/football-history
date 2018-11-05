using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace football_history.Controllers
{
    [Route("api/[controller]")]
    public class FootballHistoryController : Controller
    {
        [HttpGet("[action]")]
        public LeagueTable GetLeague()
        {
            var league = new LeagueTable 
            {
                Competition = "Premier League",
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

        public class LeagueTable
        {
            public string Competition { get; set; }
            public string Season { get; set; }
            public List<LeagueTableRow> LeagueTableRow { get; set; }
        }

        public class LeagueTableRow
        {
            public int Position { get; set; }
            public string Team { get; set; }
            public int Played { get; set; }
            public int Won { get; set; }
            public int Drawn { get; set; }
            public int Lost { get; set; }
            public int GoalsFor { get; set; }
            public int GoalsAgainst { get; set; }
            public int GoalDifference { get; set; }
            public int Points { get; set; }
        }
    }
}
