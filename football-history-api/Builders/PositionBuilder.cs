using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Calculators;
using football.history.api.Repositories.League;
using football.history.api.Repositories.Match;
using football.history.api.Repositories.PointDeductions;
using football.history.api.Repositories.Tier;

namespace football.history.api.Builders
{
    public interface IPositionBuilder
    {
        List<LeaguePosition> GetLeaguePositions(int seasonStartYear, int tier, string team);

        List<HistoricalPosition> GetHistoricalPositions(int startYear, int endYear, string team);

        List<HistoricalPosition> GetHistoricalPositionsForSeasons(
            List<int> seasonStartYears,
            string team);
    }

    public class PositionBuilder : IPositionBuilder
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IPointsDeductionRepository _pointDeductionsRepository;
        private readonly ITierRepository _tierRepository;

        public PositionBuilder(
            ILeagueRepository leagueRepository,
            IMatchRepository matchRepository,
            IPointsDeductionRepository pointDeductionsRepository,
            ITierRepository tierRepository)
        {
            _leagueRepository = leagueRepository;
            _matchRepository = matchRepository;
            _pointDeductionsRepository = pointDeductionsRepository;
            _tierRepository = tierRepository;
        }

        public List<LeaguePosition> GetLeaguePositions(int seasonStartYear, int tier, string team)
        {
            var leagueMatches = _matchRepository.GetLeagueMatchModels(seasonStartYear, tier);
            var pointsDeductions =
                _pointDeductionsRepository.GetPointsDeductionModels(seasonStartYear, tier);
            var leagueModel = _leagueRepository.GetLeagueModel(seasonStartYear, tier);

            return GetPositions(leagueMatches, leagueModel, pointsDeductions, team);
        }

        public List<HistoricalPosition> GetHistoricalPositions(
            int startYear,
            int endYear,
            string team)
        {
            var start = Math.Min(startYear, endYear);
            var numYears = Math.Max(startYear, endYear) - start;
            var seasonStartYears = Enumerable.Range(start, numYears).ToList();
            return GetHistoricalPositionsForSeasons(seasonStartYears, team);
        }

        public List<HistoricalPosition> GetHistoricalPositionsForSeasons(
            List<int> seasonStartYears,
            string team)
        {
            var tierModels = _tierRepository.GetTierModels(seasonStartYears, team);
            var tiers = tierModels.Select(t => t.Tier).ToList();

            var leagueMatches = _matchRepository.GetLeagueMatchModels(seasonStartYears, tiers);
            var playOffMatches = _matchRepository.GetPlayOffMatchModels(seasonStartYears, tiers);
            var pointsDeductions =
                _pointDeductionsRepository.GetPointsDeductionModels(seasonStartYears, tiers);

            // Get all tiers
            var leagueModels = _leagueRepository.GetLeagueModels(seasonStartYears, new List<int>());

            return GetPositions(
                team,
                tierModels,
                leagueMatches,
                playOffMatches,
                pointsDeductions,
                leagueModels);
        }

        private static List<HistoricalPosition> GetPositions(
            string team,
            List<TierModel> tierModels,
            List<MatchModel> leagueMatches,
            List<MatchModel> playOffMatches,
            List<PointsDeductionModel> pointsDeductions,
            List<LeagueModel> leagueModels)
        {
            var historicalPositions = new List<HistoricalPosition>();
            foreach (var tierModel in tierModels)
            {
                var leagueMatchesInSeason = GetMatchesInSeason(leagueMatches, tierModel);
                var playOffMatchesInSeason = GetMatchesInSeason(playOffMatches, tierModel);

                var pointsDeductionsInSeason = pointsDeductions.Where(
                        pd => pd.SeasonStartYear == tierModel.SeasonStartYear
                            && pd.Tier == tierModel.Tier)
                    .ToList();
                var leagueModel = leagueModels.Single(
                    l => l.StartYear == tierModel.SeasonStartYear && l.Tier == tierModel.Tier);

                var leagueTable = LeagueTableCalculator.GetFullLeagueTable(
                    leagueMatchesInSeason,
                    playOffMatchesInSeason,
                    leagueModel,
                    pointsDeductionsInSeason);
                var teamRow = leagueTable.Single(r => r.Team == team);

                historicalPositions.Add(
                    new HistoricalPosition
                    {
                        SeasonStartYear = tierModel.SeasonStartYear,
                        Tier = tierModel.Tier,
                        Position = teamRow.Position,
                        AbsolutePosition = GetAbsolutePosition(leagueModels, tierModel, teamRow),
                        Status = teamRow.Status
                    });
            }

            return historicalPositions;
        }

        private static List<MatchModel> GetMatchesInSeason(
            List<MatchModel> matches,
            TierModel tierModel)
        {
            if (tierModel.SeasonStartYear == 2019)
            {
                return matches.Where(
                        m => m.Date >= new DateTime(tierModel.SeasonStartYear, 7, 1)
                            && m.Date <= new DateTime(tierModel.SeasonStartYear + 1, 8, 20)
                            && m.Tier == tierModel.Tier)
                    .ToList();
            }

            if (tierModel.SeasonStartYear == 2020)
            {
                return matches.Where(
                        m => m.Date >= new DateTime(tierModel.SeasonStartYear, 8, 21)
                            && m.Date <= new DateTime(tierModel.SeasonStartYear + 1, 6, 30)
                            && m.Tier == tierModel.Tier)
                    .ToList();
            }

            return matches.Where(
                    m => m.Date >= new DateTime(tierModel.SeasonStartYear, 7, 1)
                        && m.Date <= new DateTime(tierModel.SeasonStartYear + 1, 6, 30)
                        && m.Tier == tierModel.Tier)
                .ToList();
        }

        private static int GetAbsolutePosition(
            List<LeagueModel> leagueModels,
            TierModel tierModel,
            LeagueTableRow teamRow)
        {
            return leagueModels
                    .Where(m => m.StartYear == tierModel.SeasonStartYear && m.Tier < tierModel.Tier)
                    .Select(m => m.TotalPlaces)
                    .Sum()
                + teamRow.Position;
        }

        private static List<LeaguePosition> GetPositions(
            List<MatchModel> leagueMatches,
            LeagueModel leagueModel,
            List<PointsDeductionModel> pointsDeductions,
            string team)
        {
            if (!leagueMatches.Any(m => m.HomeTeam == team || m.AwayTeam == team))
            {
                return new List<LeaguePosition>();
            }

            var dates = leagueMatches.Select(m => m.Date).Distinct().OrderBy(m => m.Date).ToList();
            var startDate = dates.First();
            var endDate = dates.Last().AddDays(1);
            var leaguePositions = new List<LeaguePosition>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var leagueTable = LeagueTableCalculator.GetPartialLeagueTable(
                    leagueMatches,
                    leagueModel,
                    pointsDeductions,
                    date);
                leaguePositions.Add(
                    new LeaguePosition
                    {
                        Date = date,
                        Position = leagueTable.Single(r => r.Team == team).Position
                    });
            }

            return leaguePositions;
        }
    }

    public class LeaguePosition
    {
        public DateTime Date { get; set; }
        public int Position { get; set; }
    }

    public class HistoricalPosition
    {
        public int SeasonStartYear { get; set; }
        public int Tier { get; set; }
        public int Position { get; set; }
        public int AbsolutePosition { get; set; }
        public string Status { get; set; }
    }
}
