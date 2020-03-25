using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Calculators;
using FootballHistoryTest.Api.Repositories.League;
using FootballHistoryTest.Api.Repositories.Match;
using FootballHistoryTest.Api.Repositories.PointDeductions;
using FootballHistoryTest.Api.Repositories.Tier;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FootballHistoryTest.Api.Controllers
{
    [Route("api/[controller]")]
    public class PositionController : Controller
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IPointsDeductionRepository _pointDeductionsRepository;
        private readonly ITierRepository _tierRepository;

        public PositionController(ILeagueRepository leagueRepository, IMatchRepository matchRepository,
            IPointsDeductionRepository pointDeductionsRepository, ITierRepository tierRepository)
        {
            _leagueRepository = leagueRepository;
            _matchRepository = matchRepository;
            _pointDeductionsRepository = pointDeductionsRepository;
            _tierRepository = tierRepository;
        }

        [HttpGet("[action]")]
        public List<LeaguePosition> GetLeaguePositions(int seasonStartYear, int tier, string team)
        {
            var leagueMatches =
                _matchRepository.GetLeagueMatchModels(new List<int> {seasonStartYear}, new List<int> {tier});
            var pointsDeductions = _pointDeductionsRepository.GetPointsDeductionModels(new List<int> {seasonStartYear}, new List<int> {tier});
            var leagueModel = _leagueRepository.GetLeagueModel(seasonStartYear, tier);

            return LeaguePositionCalculator.GetPositions(leagueMatches, leagueModel, pointsDeductions, team);
        }

        [HttpGet("[action]")]
        public List<HistoricalPosition> GetHistoricalPositions(List<int> seasonStartYears, string team)
        {
            var historicalPositions = new List<HistoricalPosition>();

            var tiers = _tierRepository.GetTierModels(seasonStartYears, team);
            
            var leagueMatches = _matchRepository.GetLeagueMatchModels(seasonStartYears, new List<int>());
            var playOffMatches = _matchRepository.GetPlayOffMatchModels(seasonStartYears, new List<int>());
            var pointsDeductions = _pointDeductionsRepository.GetPointsDeductionModels(seasonStartYears, new List<int>());

            var leagueModels = _leagueRepository.GetLeagueModels(seasonStartYears, new List<int>());
            
            foreach (var tierModel in tiers)
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
                    SeasonDates = new SeasonDates {StartYear = tierModel.SeasonStartYear, EndYear = tierModel.SeasonStartYear + 1}, 
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
        public SeasonDates SeasonDates { get; set; }
        public int Tier { get; set; }
        public int Position { get; set; }
        public int AbsolutePosition { get; set; }
        public LeagueStatus Status { get; set; }
    }
}