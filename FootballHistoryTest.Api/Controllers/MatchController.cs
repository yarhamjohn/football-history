using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Builders;
using FootballHistoryTest.Api.Repositories.Match;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistoryTest.Api.Controllers
{
    [Route("api/[controller]")]
    public class MatchController : Controller
    {
        private readonly IMatchBuilder _matchBuilder;

        public MatchController(IMatchBuilder matchBuilder)
        {
            _matchBuilder = matchBuilder;
        }
    
        [HttpGet("[action]")]
        public List<Match> GetLeagueMatches(List<int> seasonStartYears, List<int> tiers, List<string> teams)
        {
            return _matchBuilder.GetLeagueMatches(seasonStartYears, tiers, teams);
        }
    
        [HttpGet("[action]")]
        public List<Match> GetHeadToHeadLeagueMatches(List<int> seasonStartYears, List<int> tiers, string teamOne, string teamTwo)
        {
            return _matchBuilder.GetHeadToHeadLeagueMatches(seasonStartYears, tiers, teamOne, teamTwo);
        }
    
        [HttpGet("[action]")]
        public List<KnockoutMatch> GetPlayOffMatches(List<int> seasonStartYears, List<int> tiers)
        {
            return _matchBuilder.GetPlayOffMatches(seasonStartYears, tiers);
        }
    }
}