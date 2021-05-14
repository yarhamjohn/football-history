using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Exceptions;
using football.history.api.Repositories.Match;

namespace football.history.api.Builders
{
    public interface IPlayOffWinnerCalculator
    {
        string GetOneLeggedFinalWinner(MatchModel match);
        string GetTwoLeggedFinalWinner(List<MatchModel> matches);
        string GetReplayFinalWinner(List<MatchModel> matches);
    }
    
    public class PlayOffWinnerCalculator : IPlayOffWinnerCalculator
    {
        public string GetOneLeggedFinalWinner(MatchModel match)
        {
            if (HomeTeamWon(match))
            {
                return match.HomeTeamName;
            }

            if (AwayTeamWon(match))
            {
                return match.AwayTeamName;
            }

            throw new InvalidOperationException("The specified match had no winner.");
        }
        
        public string GetTwoLeggedFinalWinner(List<MatchModel> matches)
        {
            if (matches.Count != 2)
            {
                throw new InvalidOperationException($"Expected 2 matches but got {matches.Count}.");
            }

            var firstLeg = matches.OrderBy(m => m.MatchDate).First();
            var secondLeg = matches.OrderBy(m => m.MatchDate).Last();

            if (firstLeg.MatchDate == secondLeg.MatchDate)
            {
                throw new DataInvalidException($"Both matches have the same date ({firstLeg.MatchDate})");
            }

            var extraTimeInFirstLeg = firstLeg.HomeGoalsExtraTime > 0 || firstLeg.AwayGoalsExtraTime > 0;
            var penaltiesInFirstLeg = firstLeg.HomePenaltiesScored > 0 || firstLeg.HomePenaltiesScored > 0;
            if (extraTimeInFirstLeg || penaltiesInFirstLeg)
            {
                throw new DataInvalidException(
                    "The first leg of a two-legged final should not go to extra time or penalties.");
            }

            /*
             * Two legged play off finals never used the away goal rule when determining a winner.
             */
            var teamOneGoals = firstLeg.HomeGoals
                               + secondLeg.AwayGoals
                               + secondLeg.AwayGoalsExtraTime
                               + secondLeg.AwayPenaltiesScored;
            var teamTwoGoals = firstLeg.AwayGoals
                               + secondLeg.HomeGoals
                               + secondLeg.HomeGoalsExtraTime
                               + secondLeg.HomePenaltiesScored;
            
            if (teamOneGoals > teamTwoGoals)
            {
                return firstLeg.HomeTeamName;
            }

            if (teamOneGoals < teamTwoGoals)
            {
                return firstLeg.AwayTeamName;
            }

            throw new InvalidOperationException("The specified two legged matches had no winner.");
        }

        public string GetReplayFinalWinner(List<MatchModel> matches)
        {
            if (matches.Count != 3)
            {
                throw new InvalidOperationException($"Expected 3 matches but got {matches.Count}.");
            }

            var lastMatchDate = matches.Max(x => x.MatchDate);
            var replayMatch = matches.Where(x => x.MatchDate == lastMatchDate).ToList();
            if (replayMatch.Count > 1)
            {
                throw new InvalidOperationException($"Expected only one replay match but got {matches.Count}.");
            }

            return GetOneLeggedFinalWinner(replayMatch.Single());
        }
        
        private static bool HomeTeamWon(MatchModel match) =>
            match.HomeGoals > match.AwayGoals
            || match.HomeGoalsExtraTime > match.AwayGoalsExtraTime
            || match.HomePenaltiesScored > match.AwayPenaltiesScored;

        private static bool AwayTeamWon(MatchModel match) =>
            match.HomeGoals < match.AwayGoals
            || match.HomeGoalsExtraTime < match.AwayGoalsExtraTime
            || match.HomePenaltiesScored < match.AwayPenaltiesScored;
    }
}