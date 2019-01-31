using System;
using System.Collections.Generic;

namespace Backend.Models.LeagueSeason
{
    public class LeagueRowDrillDown
    {
        public List<MatchResultOld> Form { get; set; }
        public List<LeaguePosition> Positions {get; set; }
    }

    public class MatchResultOld
    {
        public DateTime MatchDate { get; set; }
        public string Result { get; set; } 
    }

    public class LeaguePosition
    {
        public DateTime Date { get; set; }
        public int Position { get; set; }
    }
}