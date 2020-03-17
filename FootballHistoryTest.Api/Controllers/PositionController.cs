using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Calculators;
using FootballHistoryTest.Api.Repositories.League;
using FootballHistoryTest.Api.Repositories.Match;
using FootballHistoryTest.Api.Repositories.PointDeductions;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistoryTest.Api.Controllers
{
    [Route("api/[controller]")]
    public class PositionController : Controller
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IPointsDeductionRepository _pointDeductionsRepository;

        public PositionController(ILeagueRepository leagueRepository, IMatchRepository matchRepository, IPointsDeductionRepository pointDeductionsRepository)
        {
            _leagueRepository = leagueRepository;
            _matchRepository = matchRepository;
            _pointDeductionsRepository = pointDeductionsRepository;
        }
        
        [HttpGet("[action]")]
        public List<LeaguePosition> GetLeaguePositions(int seasonStartYear, int tier, string team)
        {
            var leagueMatches = _matchRepository.GetLeagueMatchModels(new List<int> {seasonStartYear}, new List<int> {tier});
            var pointsDeductions = _pointDeductionsRepository.GetPointsDeductionModels(seasonStartYear, tier);
            var leagueModel = _leagueRepository.GetLeagueModel(seasonStartYear, tier);
            
            var dates = leagueMatches.Select(m => m.Date).Distinct().OrderBy(m => m.Date).ToList();
            var startDate = dates.First();
            var endDate = dates.Last().AddDays(1);
            var leaguePositions = new List<LeaguePosition>();
            
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var leagueTable = LeagueTableCalculator.GetPartialLeagueTable(leagueMatches, leagueModel, pointsDeductions, date);
                leaguePositions.Add(new LeaguePosition {Date = date, Position = leagueTable.Single(r => r.Team == team).Position});                
            }

            return leaguePositions;
        }
        
        [HttpGet("[action]")]
        public List<HistoricalPosition> GetHistoricalPositions(int startYear, int endYear, string team)
        {
            return new List<HistoricalPosition>();
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
        public int AbsolutePosition { get; set; }
        public string Status { get; set; }
    }
}
