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
        League GetLeagueOnDate(int tier, DateTime date);
    }

    public class LeagueBuilder : ILeagueBuilder
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IPointsDeductionRepository _pointDeductionsRepository;

        public LeagueBuilder(ILeagueRepository leagueRepository, IMatchRepository matchRepository,
            IPointsDeductionRepository pointDeductionsRepository)
        {
            _leagueRepository = leagueRepository;
            _matchRepository = matchRepository;
            _pointDeductionsRepository = pointDeductionsRepository;
        }

        public League GetLeagueOnDate(int tier, DateTime date)
        {
            var seasonStartYear = GetSeasonStartYear(date);

            var pointsDeductions = _pointDeductionsRepository.GetPointsDeductionModels(seasonStartYear, tier);
            var leagueModel = _leagueRepository.GetLeagueModel(seasonStartYear, tier);
            var playOffMatches = _matchRepository.GetPlayOffMatchModels(seasonStartYear, tier);
            var leagueMatches = _matchRepository.GetLeagueMatchModels(seasonStartYear, tier);

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
                Table = GetLeagueTable(date, playOffMatches, leagueMatches, leagueModel, pointsDeductions)
            };
        }
        
        private static int GetSeasonStartYear(DateTime date)
        {
            /*
             * Originally the season start date was set to be July 1st as this was roughly half-way between seasons.
             * Due to COVID-19, there was a delay in the fixtures meaning 2019-2020 actually finished in July
             * except for the Championship play-off final which was in August (handled elsewhere).
             * This date may need revising further with the addition of more leagues over time.
             */
            return date.Month > 7 ? date.Year : date.Year - 1;
        }
        
        private static List<LeagueTableRow> GetLeagueTable(DateTime date, List<MatchModel> playOffMatches,
            List<MatchModel> leagueMatches, LeagueModel leagueModel,
            List<PointsDeductionModel> pointsDeductions)
        {
            return AllMatchesHaveBeenPlayed(date, playOffMatches, leagueMatches)
                ? LeagueTableCalculator.GetFullLeagueTable(leagueMatches, playOffMatches, leagueModel, pointsDeductions)
                : LeagueTableCalculator.GetPartialLeagueTable(leagueMatches, leagueModel, pointsDeductions, date);
        }

        private static bool AllMatchesHaveBeenPlayed(DateTime date, IEnumerable<MatchModel> playOffMatches,
            IEnumerable<MatchModel> leagueMatches)
        {
            return !playOffMatches.Any(match => match.Date >= date) && !leagueMatches.Any(match => match.Date >= date);
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
        public int PointsDeducted { get; set; }
        public string PointsDeductionReason { get; set; }
        public string Status { get; set; }
    }
}
