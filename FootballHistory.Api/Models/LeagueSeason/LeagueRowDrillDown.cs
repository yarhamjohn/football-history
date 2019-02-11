using System;
using System.Collections.Generic;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Models.Controller
{
    public class LeagueRowDrillDown
    {
        public List<MatchModel> Form { get; set; }
        public List<LeaguePosition> Positions {get; set; }
    }

    public class LeaguePosition
    {
        public DateTime Date { get; set; }
        public int Position { get; set; }
    }
}