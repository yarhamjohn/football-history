using System.Collections.Generic;
using FootballHistory.Api.Domain;

namespace FootballHistory.Api.LeagueSeason.Filter
{
    public class DivisionTier
    {
        public List<Division> Divisions { get; set; }
        public Tier Tier { get; set; }
    }
}