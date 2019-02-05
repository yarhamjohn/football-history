using FootballHistory.Api.Models.DatabaseModels;
using System.Collections.Generic;

namespace FootballHistory.Api.Models.Controller
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