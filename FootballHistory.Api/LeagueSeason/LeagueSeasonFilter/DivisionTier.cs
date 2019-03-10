using System.Collections.Generic;
using FootballHistory.Api.Controllers;
using FootballHistory.Api.Domain;

namespace FootballHistory.Api.LeagueSeason.LeagueSeasonFilter
{
    public class DivisionTier
    {
        public List<Division> Divisions { get; set; }
        public Tier Tier { get; set; }
    }
}