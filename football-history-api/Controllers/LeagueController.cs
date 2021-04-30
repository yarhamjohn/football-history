using System;
using football.history.api.Builders;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LeagueController : Controller
    {
        private readonly ILeagueBuilder _leagueBuilder;

        public LeagueController(ILeagueBuilder leagueBuilder)
        {
            _leagueBuilder = leagueBuilder;
        }

        [HttpGet]
        [MapToApiVersion("1")]
        [Route("GetCompletedLeague")]
        public LeagueDto GetCompletedLeague(int seasonStartYear, int tier)
        {
            return _leagueBuilder.GetCompletedLeague(tier, seasonStartYear);
        }

        [HttpGet]
        [MapToApiVersion("1")]
        [Route("GetCompletedLeagueForTeam")]
        public LeagueDto GetCompletedLeagueForTeam(int seasonStartYear, string team)
        {
            return _leagueBuilder.GetCompletedLeagueForTeam(team, seasonStartYear);
        }

        [HttpGet]
        [MapToApiVersion("1")]
        [Route("GetLeagueOnDate")]
        public LeagueDto GetLeagueOnDate(int tier, DateTime date)
        {
            return _leagueBuilder.GetLeagueOnDate(tier, date);
        }
    }
}
