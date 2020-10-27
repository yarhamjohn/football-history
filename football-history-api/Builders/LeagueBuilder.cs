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
        League GetLeagueOnDate(int tier, int seasonStartYear, DateTime date);
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

        public League GetLeagueOnDate(int tier, int seasonStartYear, DateTime date)
        {
            var leagueModel = _leagueRepository.GetLeagueModel(seasonStartYear, tier);
            var leagueTable = GetLeagueTable(tier, seasonStartYear, date, leagueModel);

            return new League
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

        private List<LeagueTableRow> GetLeagueTable(
            int tier,
            int seasonStartYear,
            DateTime date,
            LeagueModel leagueModel)
        {
            var pointsDeductions =
                _pointDeductionsRepository.GetPointsDeductionModels(seasonStartYear, tier);
            var playOffMatches = _matchRepository.GetPlayOffMatchModels(seasonStartYear, tier);
            var leagueMatches = _matchRepository.GetLeagueMatchModels(seasonStartYear, tier);

            return AllMatchesHaveBeenPlayed(date, playOffMatches, leagueMatches)
                ? LeagueTableCalculator.GetFullLeagueTable(
                    leagueMatches,
                    playOffMatches,
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

    public class League
    {
        public string Name { get; set; }
        public int Tier { get; set; }
        public int TotalPlaces { get; set; }
        public int PromotionPlaces { get; set; }
        public int PlayOffPlaces { get; set; }
        public int RelegationPlaces { get; set; }
        public int PointsForWin { get; set; }
        public int StartYear { get; set; }
        public List<LeagueTableRow> Table { get; set; }
    }

    public class LeagueTableRow
    {
        public int Position { get; set; }
        public string Team { get; set; }
        public int Played { get; set; }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference { get; set; }
        public int Points { get; set; }
        public double PointsPerGame { get; set; }
        public int PointsDeducted { get; set; }
        public string? PointsDeductionReason { get; set; }
        public string? Status { get; set; }
    }
}
