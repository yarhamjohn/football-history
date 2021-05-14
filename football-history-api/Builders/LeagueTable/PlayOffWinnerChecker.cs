using System.Collections.Generic;
using System.IO;
using System.Linq;
using football.history.api.Exceptions;
using football.history.api.Repositories.Competition;
using football.history.api.Repositories.Match;

namespace football.history.api.Builders
{
    public interface IPlayOffWinnerChecker
    {
        bool IsPlayOffWinner(CompetitionModel competition, string teamName);
        bool IsRelegationPlayOffWinner(CompetitionModel competition, string teamName);
    }

    public class PlayOffWinnerChecker : IPlayOffWinnerChecker
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IPlayOffWinnerCalculator _calculator;

        public PlayOffWinnerChecker(IMatchRepository matchRepository, ICompetitionRepository competitionRepository,
            IPlayOffWinnerCalculator playOffWinnerCalculator)
        {
            _matchRepository = matchRepository;
            _competitionRepository = competitionRepository;
            _calculator = playOffWinnerCalculator;
        }

        public bool IsPlayOffWinner(CompetitionModel competition, string teamName)
        {
            var playOffMatches = competition.PlayOffPlaces > 0
                ? _matchRepository.GetPlayOffMatches(competition.Id)
                : new List<MatchModel>();
            var result = IsWinner(teamName, playOffMatches);

            var isTierTwoIn1989 = competition.StartYear == 1989 && competition.Tier == 2;
            return isTierTwoIn1989 ? FixWinnerForTierTwoIn1989(teamName) : result;
        }

        public bool IsRelegationPlayOffWinner(CompetitionModel competition, string teamName)
        {
            var playOffMatches = GetRelegationPlayOffMatches(competition);
            return IsWinner(teamName, playOffMatches);
        }

        private List<MatchModel> GetRelegationPlayOffMatches(CompetitionModel competition)
        {
            if (competition.RelegationPlayOffPlaces == 0)
            {
                return new List<MatchModel>();
            }

            var feederTier = competition.Tier + 1;
            var feederCompetition = _competitionRepository.GetCompetitionForSeasonAndTier(
                competition.SeasonId,
                feederTier);

            return _matchRepository.GetPlayOffMatches(feederCompetition.Id);
        }

        private bool IsWinner(string teamName, List<MatchModel> playOffMatches)
        {
            var playOffFinalMatches = playOffMatches.Where(m => m.RulesStage == "Final").ToList();
            return playOffFinalMatches.Count switch
            {
                0 => false,
                1 => teamName == _calculator.GetOneLeggedFinalWinner(playOffFinalMatches.Single()),
                2 => teamName == _calculator.GetTwoLeggedFinalWinner(playOffFinalMatches),
                3 => teamName == _calculator.GetReplayFinalWinner(playOffFinalMatches),
                _ => throw new DataInvalidException($"Cannot determine play off winner as too many final matches ({playOffFinalMatches.Count}) were found."),
            };
        }

        private static bool FixWinnerForTierTwoIn1989(string teamName)
            => teamName switch
            {
                // Sunderland were promoted instead of Swindon Town despite Swindon winning the play-offs due to financial irregularities.
                "Sunderland" => true,
                _ => false
            };
    }
}