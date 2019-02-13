using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders
{
    public class PlayOffMatchesBuilder : IPlayOffMatchesBuilder
    {
        public PlayOffs Build(List<MatchDetailModel> matchDetails)
        {
            var playOffs = new PlayOffs();

            var playOffMatches = matchDetails.OrderBy(m => m.Date).ToList();
            foreach (var match in playOffMatches)
            {
                if (match.Round == "Final")
                {
                    playOffs.Final = match;
                }
                else
                {
                    AddSemiFinalMatch(playOffs, match);
                }
            }

            return playOffs;
        }

        private static void AddSemiFinalMatch(PlayOffs playOffs, MatchDetailModel match)
        {
            if (IsSecondLeg(playOffs, match))
            {
                AddSecondLeg(playOffs, match);
            }
            else
            {
                AddFirstLeg(playOffs, match);
            }
        }

        private static bool IsSecondLeg(PlayOffs playOffs, MatchDetailModel match)
        {
            return playOffs.SemiFinals.Where(sf => sf.FirstLeg.HomeTeam == match.AwayTeam).ToList().Count == 1;
        }
        
        private static void AddSecondLeg(PlayOffs playOffs, MatchDetailModel match)
        {
            playOffs.SemiFinals = playOffs.SemiFinals
                .Select(sf => sf.FirstLeg.HomeTeam == match.AwayTeam 
                    ? new PlayOffsSemiFinal {FirstLeg = sf.FirstLeg, SecondLeg = match} 
                    : sf)
                .ToList();
        }

        private static void AddFirstLeg(PlayOffs playOffs, MatchDetailModel match)
        {
            playOffs.SemiFinals.Add(new PlayOffsSemiFinal {FirstLeg = match});
        }
    }
}
