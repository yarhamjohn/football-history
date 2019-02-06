using System.Collections.Generic;

namespace FootballHistory.Api.Builders.Models
{
    public class LeagueSeasonFilter
    {
        public List<string> AllSeasons;
        public List<Tier> AllTiers;
    }

    public class Tier
    {
        public List<Division> Divisions { get; set; }
        public int Level { get; set; }
    }

    public class Division
    {
        public string Name { get; set; }
        public int ActiveFrom { get; set; }
        public int ActiveTo { get; set; }
    }
}
