using System;
using System.Collections.Generic;
using FootballHistoryTest.Api.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistoryTest.Api.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
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
        
        [HttpGet("[action]")]
        public TestLeagueTable GetLeagueTable(int tier, int startYear)
        {
            return new TestLeagueTable();
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