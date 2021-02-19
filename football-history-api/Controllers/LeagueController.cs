using System;
using football.history.api.Builders;
using football.history.api.Calculators;
using football.history.api.Repositories.Tier;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
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
        public LeagueDto GetCompletedLeagueForTier(int seasonStartYear, int tier)
        {
            return _leagueBuilder.BuildForTier(seasonStartYear, tier);
        }

        [HttpGet("[action]")]
        public LeagueDto GetCompletedLeagueForTeam(int seasonStartYear, string team)
        {
            return _leagueBuilder.BuildForTeam(seasonStartYear, team);
        }

        [HttpGet("[action]")]
        public LeagueDto GetLeagueOnDateForTier(int tier, DateTime date)
        {
            return _leagueBuilder.BuildForTier(date, tier);
        }

        [HttpGet("[action]")]
        public LeagueDto GetLeagueOnDateForTeam(string team, DateTime date)
        {
            return _leagueBuilder.BuildForTeam(date, team);
        }
    }
}
