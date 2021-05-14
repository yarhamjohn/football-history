using System;
using football.history.api.Repositories.Competition;

namespace football.history.api.Builders
{
    public interface IStatusCalculator
    {
        string? GetStatus(string teamName, int leaguePosition, CompetitionModel competition);
    }

    public class StatusCalculator : IStatusCalculator
    {
        private readonly IPlayOffWinnerChecker _playOffWinnerChecker;

        public StatusCalculator(IPlayOffWinnerChecker playOffWinnerChecker)
        {
            _playOffWinnerChecker = playOffWinnerChecker;
        }

        public string? GetStatus(string teamName, int leaguePosition, CompetitionModel competition)
        {
            if (competition.RelegationPlayOffPlaces > 0 && competition.RelegationPlaces == 0)
            {
                throw new InvalidOperationException(
                    "Invalid competition format found. Cannot have a relegation play off place without relegation places.");
            }
            
            if (competition.ReElectionPlaces > 0 && competition.RelegationPlaces > 0)
            {
                throw new InvalidOperationException(
                    "Invalid competition format found. Cannot have relegation places and re-election places.");
            }
            
            if (leaguePosition == 1)
            {
                return "Champions";
            }

            if (InPromotionPlaces(leaguePosition, competition))
            {
                return "Promoted";
            }

            if (InPlayOffPlaces(leaguePosition, competition))
            {
                return _playOffWinnerChecker.IsPlayOffWinner(competition, teamName)
                    ? "PlayOff Winner"
                    : "PlayOffs";
            }

            if (InRelegationPlayOffPlaces(leaguePosition, competition))
            {
                return _playOffWinnerChecker.IsRelegationPlayOffWinner(competition, teamName)
                    ? "Relegation PlayOffs"
                    : "Relegated - PlayOffs";
            }

            if (InRelegationZone(leaguePosition, competition))
            {
                return "Relegated";
            }

            if (InReElectionPlaces(leaguePosition, competition))
            {
                return FailedReElection(leaguePosition, competition) ? "Failed Re-election" : "Re-elected";
            }

            return null;
        }

        private bool InPlayOffPlaces(int position, CompetitionModel competition) =>
            position > competition.PromotionPlaces
            && position <= competition.PromotionPlaces + competition.PlayOffPlaces;

        private bool InPromotionPlaces(int position, CompetitionModel competition) =>
            position > 1 && position <= competition.PromotionPlaces;

        private bool InRelegationZone(int position, CompetitionModel competition) =>
            position > competition.TotalPlaces - competition.RelegationPlaces;

        private bool InReElectionPlaces(int position, CompetitionModel competition) =>
            position > competition.TotalPlaces - competition.ReElectionPlaces;

        private bool FailedReElection(int position, CompetitionModel competition) =>
            competition.FailedReElectionPosition == position;

        private bool
            InRelegationPlayOffPlaces(int position, CompetitionModel competition) =>
            !InRelegationZone(position, competition)
            && position
            > competition.TotalPlaces
            - (competition.RelegationPlaces + competition.RelegationPlayOffPlaces);
    }
}