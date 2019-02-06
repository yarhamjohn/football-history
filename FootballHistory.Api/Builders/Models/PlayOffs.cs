using System.Collections.Generic;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders.Models
{
    public class PlayOffs
    {
        public List<PlayOffsSemiFinal> SemiFinals { get; set; }
        public MatchDetailModel Final { get; set; }
    }

    public class PlayOffsSemiFinal
    {
        public MatchDetailModel FirstLeg { get; set; }
        public MatchDetailModel SecondLeg { get; set; }
    }
}
