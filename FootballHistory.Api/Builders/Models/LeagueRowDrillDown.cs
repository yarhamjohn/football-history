using System.Collections.Generic;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders.Models
{
    public class LeagueRowDrillDown
    {
        public List<Match> Form { get; set; }
        public List<LeaguePosition> Positions {get; set; }
    }
}
