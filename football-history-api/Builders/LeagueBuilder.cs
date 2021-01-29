using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Calculators;
using football.history.api.Repositories.League;
using football.history.api.Repositories.Match;
using football.history.api.Repositories.PointDeductions;

namespace football.history.api.Builders
{
    public interface ILeagueBuilder
    {
        LeagueDto GetLeagueOnDate(int tier, int seasonStartYear, DateTime date);
    }

    public class LeagueBuilder : ILeagueBuilder
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IPointsDeductionRepository _pointDeductionsRepository;

        public LeagueBuilder(
            ILeagueRepository leagueRepository,
            IMatchRepository matchRepository,
            IPointsDeductionRepository pointDeductionsRepository)
        {
            _leagueRepository = leagueRepository;
            _matchRepository = matchRepository;
            _pointDeductionsRepository = pointDeductionsRepository;
        }

        public LeagueDto GetLeagueOnDate(int tier, int seasonStartYear, DateTime date)
        {
            var leagueModel = _leagueRepository.GetLeagueModel(seasonStartYear, tier);
            var leagueTable = GetLeagueTable(tier, seasonStartYear, date, leagueModel);

            return new LeagueDto
            {
                Name = leagueModel.Name,
                Tier = leagueModel.Tier,
                TotalPlaces = leagueModel.TotalPlaces,
                PromotionPlaces = leagueModel.PromotionPlaces,
                RelegationPlaces = leagueModel.RelegationPlaces,
                PlayOffPlaces = leagueModel.PlayOffPlaces,
                PointsForWin = leagueModel.PointsForWin,
                StartYear = leagueModel.StartYear,
                Table = leagueTable
            };
        }

        private List<LeagueTableRowDto> GetLeagueTable(
            int tier,
            int seasonStartYear,
            DateTime date,
            LeagueModel leagueModel)
        {
            var pointsDeductions =
                _pointDeductionsRepository.GetPointsDeductionModels(seasonStartYear, tier);
            var playOffMatches = _matchRepository.GetPlayOffMatchModels(seasonStartYear, tier);
            var relegationPlayOffMatches = _matchRepository.GetPlayOffMatchModels(seasonStartYear, tier + 1);
            var leagueMatches = _matchRepository.GetLeagueMatchModels(seasonStartYear, tier);

            return AllMatchesHaveBeenPlayed(date, playOffMatches, leagueMatches)
                ? LeagueTableCalculator.GetFullLeagueTable(
                    leagueMatches,
                    playOffMatches,
                    relegationPlayOffMatches,
                    leagueModel,
                    pointsDeductions)
                : LeagueTableCalculator.GetPartialLeagueTable(
                    leagueMatches,
                    leagueModel,
                    pointsDeductions,
                    date);
        }

        private static bool AllMatchesHaveBeenPlayed(
            DateTime date,
            IEnumerable<MatchModel> playOffMatches,
            IEnumerable<MatchModel> leagueMatches)
        {
            var playOffMatchesAfterDate = playOffMatches.Any(match => match.Date >= date);
            var leagueMatchesAfterDate = leagueMatches.Any(match => match.Date >= date);
            return !playOffMatchesAfterDate && !leagueMatchesAfterDate;
        }
    }
}
