using System;
using FootballHistoryTest.Api.Builders;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistoryTest.Api.Controllers
{
    [Route("api/[controller]")]
    public class LeagueController : Controller
    {
        private readonly ILeagueBuilder _leagueBuilder;

        public LeagueController(ILeagueBuilder leagueBuilder)
        {
            _leagueBuilder = leagueBuilder;
        }

        [HttpGet("[action]")]
        public League GetCompletedLeague(int seasonStartYear, int tier)
        {
            var seasonEndDate = new DateTime(seasonStartYear + 1, 06, 30);
            return _leagueBuilder.GetLeagueOnDate(tier, seasonEndDate);
        }

        [HttpGet("[action]")]
        public League GetLeagueOnDate(int tier, DateTime date)
        {
            return _leagueBuilder.GetLeagueOnDate(tier, date);
        }
    }
}
