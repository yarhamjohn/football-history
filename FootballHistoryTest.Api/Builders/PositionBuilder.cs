using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Calculators;
using FootballHistoryTest.Api.Domain;
using FootballHistoryTest.Api.Repositories.League;
using FootballHistoryTest.Api.Repositories.Match;
using FootballHistoryTest.Api.Repositories.PointDeductions;
using FootballHistoryTest.Api.Repositories.Tier;
using Microsoft.EntityFrameworkCore;

namespace FootballHistoryTest.Api.Builders
{
    public interface IPositionBuilder
    {
        List<LeaguePosition> GetLeaguePositions(int seasonStartYear, int tier, string team);
        List<HistoricalPosition> GetHistoricalPositions(int startYear, int endYear, string team);
        List<HistoricalPosition> GetHistoricalPositionsForSeasons(List<int> seasonStartYears, string team);

    }
    
    public class PositionBuilder : IPositionBuilder
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IPointsDeductionRepository _pointDeductionsRepository;
        private readonly ITierRepository _tierRepository;

        public PositionBuilder(ILeagueRepository leagueRepository, IMatchRepository matchRepository,
            IPointsDeductionRepository pointDeductionsRepository, ITierRepository tierRepository)
        {
            _leagueRepository = leagueRepository;
            _matchRepository = matchRepository;
            _pointDeductionsRepository = pointDeductionsRepository;
            _tierRepository = tierRepository;
        }

        public List<LeaguePosition> GetLeaguePositions(int seasonStartYear, int tier, string team)
        {
            var leagueMatches = _matchRepository.GetLeagueMatchModels(seasonStartYear, tier);
            var pointsDeductions = _pointDeductionsRepository.GetPointsDeductionModels(seasonStartYear, tier);
            var leagueModel = _leagueRepository.GetLeagueModel(seasonStartYear, tier);

            return LeaguePositionCalculator.GetPositions(leagueMatches, leagueModel, pointsDeductions, team);
        }

        public List<HistoricalPosition> GetHistoricalPositions(int startYear, int endYear, string team)
        {
            var start = Math.Min(startYear, endYear);
            var numYears = Math.Max(startYear, endYear) - start;
            var seasonStartYears = Enumerable.Range(start, numYears).ToList();
            return GetHistoricalPositionsForSeasons(seasonStartYears, team);
        }

        public List<HistoricalPosition> GetHistoricalPositionsForSeasons(List<int> seasonStartYears, string team)
        {
            var historicalPositions = new List<HistoricalPosition>();

            var tierModels = _tierRepository.GetTierModels(seasonStartYears, team);
            var tiers = tierModels.Select(t => t.Tier).ToList();

            var leagueMatches = _matchRepository.GetLeagueMatchModels(seasonStartYears, tiers);
            var playOffMatches = _matchRepository.GetPlayOffMatchModels(seasonStartYears, tiers);
            var pointsDeductions = _pointDeductionsRepository.GetPointsDeductionModels(seasonStartYears, tiers);

            var leagueModels = _leagueRepository.GetLeagueModels(seasonStartYears, tiers);
            
            foreach (var tierModel in tierModels)
            {
                var leagueMatchesInSeason = leagueMatches.Where(m =>
                    m.Date >= new DateTime(tierModel.SeasonStartYear, 7, 1) && m.Date <= new DateTime(tierModel.SeasonStartYear + 1, 6, 30) && m.Tier == tierModel.Tier).ToList();
                var playOffMatchesInSeason = playOffMatches.Where(m =>
                    m.Date >= new DateTime(tierModel.SeasonStartYear, 7, 1) && m.Date <= new DateTime(tierModel.SeasonStartYear + 1, 6, 30) && m.Tier == tierModel.Tier).ToList();
                var pointsDeductionsInSeason = pointsDeductions.Where(pd =>
                    pd.SeasonStartYear == tierModel.SeasonStartYear && pd.Tier == tierModel.Tier).ToList();
                var leagueModel = leagueModels.Single(l => l.StartYear == tierModel.SeasonStartYear && l.Tier == tierModel.Tier);
                
                var leagueTable = LeagueTableCalculator.GetFullLeagueTable(leagueMatchesInSeason, playOffMatchesInSeason, leagueModel, pointsDeductionsInSeason);
                var teamRow = leagueTable.Single(r => r.Team == team);
                historicalPositions.Add(new HistoricalPosition
                {
                    SeasonStartYear = tierModel.SeasonStartYear,
                    Tier = tierModel.Tier,
                    Position = teamRow.Position,
                    AbsolutePosition = leagueModels.Where(m => m.Tier != tierModel.Tier).Select(m => m.TotalPlaces).Sum() + teamRow.Position,
                    Status = teamRow.Status
                });
            }

            return historicalPositions;
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
