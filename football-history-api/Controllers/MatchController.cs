using System.Collections.Generic;
using football.history.api.Builders;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MatchController : Controller
    {
        private readonly IMatchBuilder _matchBuilder;

        public MatchController(IMatchBuilder matchBuilder)
        {
            _matchBuilder = matchBuilder;
        }

        [HttpGet]
        [MapToApiVersion("1")]
        [Route("GetLeagueMatches")]
        public List<Match> GetLeagueMatches(
            List<int> seasonStartYears,
            List<int> tiers,
            List<string> teams) =>
            _matchBuilder.GetLeagueMatches(seasonStartYears, tiers, teams);

        [HttpGet]
        [MapToApiVersion("1")]
        [Route("GetHeadToHeadLeagueMatches")]
        public List<Match> GetHeadToHeadLeagueMatches(
            List<int> seasonStartYears,
            List<int> tiers,
            string teamOne,
            string teamTwo) =>
            _matchBuilder.GetHeadToHeadLeagueMatches(seasonStartYears, tiers, teamOne, teamTwo);

        [HttpGet]
        [MapToApiVersion("1")]
        [Route("GetPlayOffMatches")]
        public List<KnockoutMatch> GetPlayOffMatches(List<int> seasonStartYears, List<int> tiers) =>
            _matchBuilder.GetPlayOffMatches(seasonStartYears, tiers);
    }
}
