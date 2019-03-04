using System.Collections.Generic;
using FootballHistory.Api.Repositories.MatchDetailRepository;

namespace FootballHistory.Api.LeagueSeason.PlayOffs
{
    public class PlayOffs
    {
        public List<PlayOffsSemiFinal> SemiFinals { get; set; }
        public MatchDetailModel Final { get; set; }

        public PlayOffs()
        {
            SemiFinals = new List<PlayOffsSemiFinal>();
        }
    }

    public class PlayOffsSemiFinal
    {
        public MatchDetailModel FirstLeg { get; set; }
        public MatchDetailModel SecondLeg { get; set; }
    }
}
