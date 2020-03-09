using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Repositories.Match;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistoryTest.Api.Controllers
{
    [Route("api/[controller]")]
    public class MatchController : Controller
    {
        private readonly IMatchRepository _matchRepository;

        public MatchController(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }
    
        [HttpGet("[action]")]
        public List<Match> GetLeagueMatchesForTeam(int seasonStartYear, string team)
        {
            var matchModels = _matchRepository.GetLeagueMatchModels(seasonStartYear, team);
            return matchModels
                .Select(m => new Match
                {
                    Tier = m.Tier,
                    Division = m.Division,
                    Date = m.Date,
                    HomeTeam = m.HomeTeam,
                    HomeTeamAbbreviation = m.HomeTeamAbbreviation,
                    AwayTeam = m.AwayTeam,
                    AwayTeamAbbreviation = m.AwayTeamAbbreviation,
                    HomeGoals = m.HomeGoals,
                    AwayGoals = m.AwayGoals
                    
                })
                .ToList();
        }
    
        [HttpGet("[action]")]
        public List<Match> GetMatchesForTeam(string team)
        {
            var matchModels = _matchRepository.GetLeagueMatchModels(team);
            return matchModels
                .Select(m => new Match
                {
                    Tier = m.Tier,
                    Division = m.Division,
                    Date = m.Date,
                    HomeTeam = m.HomeTeam,
                    HomeTeamAbbreviation = m.HomeTeamAbbreviation,
                    AwayTeam = m.AwayTeam,
                    AwayTeamAbbreviation = m.AwayTeamAbbreviation,
                    HomeGoals = m.HomeGoals,
                    AwayGoals = m.AwayGoals
                    
                })
                .ToList();
        }
    
        [HttpGet("[action]")]
        public List<Match> GetMatchesBetweenTeam(string teamOne, string teamTwo)
        {
            var matchModels = _matchRepository.GetLeagueMatchModels(teamOne, teamTwo);
            return matchModels
                .Select(m => new Match
                {
                    Tier = m.Tier,
                    Division = m.Division,
                    Date = m.Date,
                    HomeTeam = m.HomeTeam,
                    HomeTeamAbbreviation = m.HomeTeamAbbreviation,
                    AwayTeam = m.AwayTeam,
                    AwayTeamAbbreviation = m.AwayTeamAbbreviation,
                    HomeGoals = m.HomeGoals,
                    AwayGoals = m.AwayGoals
                    
                })
                .ToList();
        }
    
        [HttpGet("[action]")]
        public List<Match> GetLeagueMatches(int seasonStartYear, int tier)
        {
            var matchModels = _matchRepository.GetLeagueMatchModels(seasonStartYear, tier);
            return matchModels
                .Select(m => new Match
                {
                    Tier = m.Tier,
                    Division = m.Division,
                    Date = m.Date,
                    HomeTeam = m.HomeTeam,
                    HomeTeamAbbreviation = m.HomeTeamAbbreviation,
                    AwayTeam = m.AwayTeam,
                    AwayTeamAbbreviation = m.AwayTeamAbbreviation,
                    HomeGoals = m.HomeGoals,
                    AwayGoals = m.AwayGoals
                    
                })
                .ToList();
        }
    
        [HttpGet("[action]")]
        public List<KnockoutMatch> GetPlayOffMatches(int seasonStartYear, int tier)
        {
            var matchModels = _matchRepository.GetPlayOffMatchModels(seasonStartYear, tier);
            return matchModels
                .Select(m => new KnockoutMatch
                {
                    Tier = m.Tier,
                    Division = m.Division,
                    Date = m.Date,
                    HomeTeam = m.HomeTeam,
                    HomeTeamAbbreviation = m.HomeTeamAbbreviation,
                    AwayTeam = m.AwayTeam,
                    AwayTeamAbbreviation = m.AwayTeamAbbreviation,
                    HomeGoals = m.HomeGoals,
                    AwayGoals = m.AwayGoals,
                    Round = m.Round,
                    Leg = m.Leg,
                    ExtraTime = m.ExtraTime,
                    HomeGoalsExtraTime = m.HomeGoalsExtraTime,
                    AwayGoalsExtraTime = m.AwayGoalsExtraTime,
                    PenaltyShootout = m.PenaltyShootout,
                    HomeGoalsPenaltyShootout = m.HomeGoalsPenaltyShootout,
                    AwayGoalsPenaltyShootout = m.AwayGoalsPenaltyShootout

                })
                .ToList();
        }
    }

    public class Match
    {
        public int Tier { get; set; }
        public string Division { get; set; }
        public DateTime Date { get; set; }
        public string HomeTeam { get; set; }
        public string HomeTeamAbbreviation { get; set; }
        public string AwayTeam { get; set; }
        public string AwayTeamAbbreviation { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
    }

    public class KnockoutMatch : Match
    {
        public string Round { get; set; }
        public int? Leg { get; set; }
        public bool ExtraTime { get; set; }
        public int? HomeGoalsExtraTime { get; set; }
        public int? AwayGoalsExtraTime { get; set; }
        public bool PenaltyShootout { get; set; }
        public int? HomeGoalsPenaltyShootout { get; set; }
        public int? AwayGoalsPenaltyShootout { get; set; }
    }
}