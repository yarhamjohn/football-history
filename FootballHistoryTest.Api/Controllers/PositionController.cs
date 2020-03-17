using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Calculators;
using FootballHistoryTest.Api.Repositories.League;
using FootballHistoryTest.Api.Repositories.Match;
using FootballHistoryTest.Api.Repositories.PointDeductions;
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

        public PositionController(ILeagueRepository leagueRepository, IMatchRepository matchRepository,
            IPointsDeductionRepository pointDeductionsRepository)
        {
            _leagueRepository = leagueRepository;
            _matchRepository = matchRepository;
            _pointDeductionsRepository = pointDeductionsRepository;
        }

        [HttpGet("[action]")]
        public List<LeaguePosition> GetLeaguePositions(int seasonStartYear, int tier, string team)
        {
            var leagueMatches =
                _matchRepository.GetLeagueMatchModels(new List<int> {seasonStartYear}, new List<int> {tier});
            var pointsDeductions = _pointDeductionsRepository.GetPointsDeductionModels(seasonStartYear, tier);
            var leagueModel = _leagueRepository.GetLeagueModel(seasonStartYear, tier);

            return LeaguePositionCalculator.GetPositions(leagueMatches, leagueModel, pointsDeductions, team);
        }

        [HttpGet("[action]")]
        public List<HistoricalPosition> GetHistoricalPositions(List<int> seasonStartYears, string team)
        {
            var historicalPositions = new List<HistoricalPosition>();
            foreach (var year in seasonStartYears)
            {
                var leagueMatches = _matchRepository.GetLeagueMatchModels(new List<int> {year}, new List<int>());
                var tier = leagueMatches.Select(m => m.Tier).First();
                var playOffMatches = _matchRepository.GetPlayOffMatchModels(new List<int> {year}, new List<int> {tier});
                var pointsDeductions = _pointDeductionsRepository.GetPointsDeductionModels(year, tier);
                var leagueModels = _leagueRepository.GetLeagueModels(year, Enumerable.Range(1, tier).ToList());
                var leagueTable =
                    LeagueTableCalculator.GetFullLeagueTable(leagueMatches, playOffMatches, leagueModels.Single(m => m.Tier == tier),
                        pointsDeductions);

                var teamRow = leagueTable.Single(r => r.Team == team);
                historicalPositions.Add(new HistoricalPosition
                {
                    SeasonDates = new SeasonDates {StartYear = year, EndYear = year + 1}, 
                    Tier = tier,
                    Position = teamRow.Position,
                    AbsolutePosition = leagueModels.Where(m => m.Tier != tier).Select(m => m.TotalPlaces).Sum() + teamRow.Position,
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