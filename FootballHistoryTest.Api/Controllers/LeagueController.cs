using System;
using FootballHistoryTest.Api.Builders;
using FootballHistoryTest.Api.Repositories.Tier;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistoryTest.Api.Controllers
{
    [Route("api/[controller]")]
    public class LeagueController : Controller
    {
        private readonly ILeagueBuilder _leagueBuilder;
        private readonly ITierRepository _tierRepository;

        public LeagueController(ILeagueBuilder leagueBuilder, ITierRepository tierRepository)
        {
            _leagueBuilder = leagueBuilder;
            _tierRepository = tierRepository;
        }

        [HttpGet("[action]")]
        public League GetCompletedLeague(int seasonStartYear, int tier)
        {
            var seasonEndDate = new DateTime(seasonStartYear + 1, 06, 30);
            return _leagueBuilder.GetLeagueOnDate(tier, seasonEndDate);
        }

        [HttpGet("[action]")]
        public League GetCompletedLeagueForTeam(int seasonStartYear, string team)
        {
            var tier = _tierRepository.GetTierForTeamInYear(seasonStartYear, team);
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
