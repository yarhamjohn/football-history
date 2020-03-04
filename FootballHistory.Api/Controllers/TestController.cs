using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Domain;
using FootballHistory.Api.LeagueSeason.Table;
using FootballHistory.Api.Repositories.DivisionRepository;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistory.Api.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private IDivisionRepository _divisionRepository;

        public TestController(IDivisionRepository divisionRepository)
        {
            _divisionRepository = divisionRepository;
        }
        
        public class TestMatch
        {
            public DateTime Date { get; set; }
            public string HomeTeam { get; set; }
            public string HomeTeamAbbreviation { get; set; }
            public string AwayTeam { get; set; }
            public string AwayTeamAbbreviation { get; set; }
            public int HomeGoals { get; set; }
            public int AwayGoals { get; set; }
        }

        public class TestPlayOffMatch : TestMatch
        {
            public string Round { get; set; }
            public bool ExtraTime { get; set; }
            public int? HomeGoalsExtraTime { get; set; }
            public int? AwayGoalsExtraTime { get; set; }
            public bool PenaltyShootout { get; set; }
            public int? HomeGoalsPenaltyShootout { get; set; }
            public int? AwayGoalsPenaltyShootout { get; set; }
        }
        
        public class TestLeague
        {
            public string Name { get; set; }
            public List<string> Teams { get; set; }
            public int TotalPlaces { get; set; }
            public int PromotionPlaces { get; set; }
            public int PlayOffPlaces { get; set; }
            public int RelegationPlaces { get; set; }
            public int PointsForWin { get; set; }
        }

        public class TestLeaguePosition
        {
            public DateTime Date { get; set; }
            public int Position { get; set; }
        }

        public class TestHistoricalPosition
        {
            public TestSeason Season { get; set; }
            public int AbsolutePosition { get; set; }
            public string Status { get; set; }
        }

        public class TestDivision
        {
            public string Name { get; set; }
            public int YearActiveFrom { get; set; }
            public int YearActiveTo { get; set; }
        }
        
        public class TestSeason
        {
            public int StartYear { get; set; }
            public int EndYear { get; set; }
        }

        public class TestTeam
        {
            public string Name { get; set; }
            public string Abbreviation { get; set; }
        }

        [HttpGet("[action]")]
        public List<TestSeason> GetSeasons()
        {
            return new List<TestSeason>();
        }
        
        [HttpGet("[action]")]
        public List<TestTeam> GetTeams()
        {
            return new List<TestTeam>();
        }
        
        [HttpGet("[action]")]
        public TestLeague GetLeague(int tier, int startYear)
        {
            return new TestLeague();
        }
        
        [HttpGet("[action]")]
        public Dictionary<Tier, TestLeague> GetLeagues(int startYear)
        {
            return new Dictionary<Tier, TestLeague>();
        }
        
        [HttpGet("[action]")]
        public List<TestDivision> GetDivisions(int tier)
        {
            var divisionModels = _divisionRepository.GetDivisions(tier);
            return divisionModels
                .Select(d => new TestDivision { Name = d.Name, YearActiveFrom = d.From, YearActiveTo = d.To} )
                .ToList();
        }
        
        [HttpGet("[action]")]
        public LeagueTable GetLeagueTable(int tier, int startYear)
        {
            return new LeagueTable();
        }
        
        [HttpGet("[action]")]
        public List<TestMatch> GetLeagueMatches(int tier, int startYear)
        {
            return new List<TestMatch>();
        }
        
        [HttpGet("[action]")]
        public List<TestMatch> GetTeamLeagueMatches(int tier, int startYear, string team)
        {
            return new List<TestMatch>();
        }
        
        [HttpGet("[action]")]
        public List<TestLeaguePosition> GetLeaguePositions(int tier, int startYear, string team)
        {
            return new List<TestLeaguePosition>();
        }
        
        [HttpGet("[action]")]
        public List<TestPlayOffMatch> GetPlayOffMatches(int tier, int startYear)
        {
            return new List<TestPlayOffMatch>();
        }
        
        [HttpGet("[action]")]
        public List<TestHistoricalPosition> GetHistoricalPositions(int startYear, int endYear, string team)
        {
            return new List<TestHistoricalPosition>();
        }
    }
}