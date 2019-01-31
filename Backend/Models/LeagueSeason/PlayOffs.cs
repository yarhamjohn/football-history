using System.Collections.Generic;
using Backend.Domain.Models;

namespace Backend.Models.LeagueSeason
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