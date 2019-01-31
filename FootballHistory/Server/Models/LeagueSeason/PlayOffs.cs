using System.Collections.Generic;
using FootballHistory.Server.Domain.Models;

namespace FootballHistory.Server.Models.LeagueSeason
{
    public class PlayOffs
    {
        public List<SemiFinal> SemiFinals { get; set; }
        public MatchDetailModel Final { get; set; }
    }

    public class SemiFinal
    {
        public MatchDetailModel FirstLeg { get; set; }
        public MatchDetailModel SecondLeg { get; set; }
    }
}