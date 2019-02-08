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
            return CreatePlayOffs(matchDetails);
        }

        private PlayOffs CreatePlayOffs(List<MatchDetailModel> matchDetails)
        {
            var playOffMatches = matchDetails
                .Select(m => new MatchDetailModel 
                    {
                        Competition = m.Competition,
                        Round = m.Round,
                        Date = m.Date,
                        HomeTeam = m.HomeTeam,
                        HomeTeamAbbreviation = m.HomeTeamAbbreviation,
                        AwayTeam = m.AwayTeam,
                        AwayTeamAbbreviation = m.AwayTeamAbbreviation,
                        HomeGoals = m.HomeGoals,
                        AwayGoals = m.AwayGoals,
                        ExtraTime = m.ExtraTime,
                        HomeGoalsET = m.ExtraTime ? m.HomeGoalsET : (int?) null,
                        AwayGoalsET = m.ExtraTime ? m.AwayGoalsET : (int?) null,
                        PenaltyShootout = m.PenaltyShootout,
                        HomePenaltiesTaken = m.PenaltyShootout ? m.HomePenaltiesTaken : (int?) null,
                        HomePenaltiesScored = m.PenaltyShootout ? m.HomePenaltiesScored : (int?) null,
                        AwayPenaltiesTaken = m.PenaltyShootout ? m.AwayPenaltiesTaken : (int?) null,
                        AwayPenaltiesScored = m.PenaltyShootout ? m.AwayPenaltiesScored : (int?) null
                    })
                .OrderBy(m => m.Date)
                .ToList();
            
            var playOffs = new PlayOffs();

            foreach(var match in playOffMatches)
            {
                if (match.Round == "Final")
                {
                    playOffs.Final = match;
                }
                else
                {
                    AddSemiFinal(playOffs, match);
                }
            }

            return playOffs;
        }

        private void AddSemiFinal(PlayOffs playOffs, MatchDetailModel match)
        {
            if (playOffs.SemiFinals.Count == 0)
            {
                playOffs.SemiFinals.Add(new PlayOffsSemiFinal { FirstLeg = match });
            } 
            else if (playOffs.SemiFinals.Count == 1)
            {
                if (match.HomeTeam == playOffs.SemiFinals[0].FirstLeg.AwayTeam)
                {
                    playOffs.SemiFinals[0].SecondLeg = match;
                }
                else
                {
                    playOffs.SemiFinals.Add(new PlayOffsSemiFinal { FirstLeg = match });
                }
            }
            else
            {
                if (match.HomeTeam == playOffs.SemiFinals[0].FirstLeg.AwayTeam)
                {
                    playOffs.SemiFinals[0].SecondLeg = match;
                }
                else
                {
                    playOffs.SemiFinals[1].SecondLeg = match;
                }
            }
        }
    }
}
