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
        private readonly ITierRepository _tierRepository;

        public LeagueController(ILeagueBuilder leagueBuilder, ITierRepository tierRepository)
        {
            _leagueBuilder = leagueBuilder;
            _tierRepository = tierRepository;
        }

        [HttpGet("[action]")]
        public LeagueDto GetCompletedLeague(int seasonStartYear, int tier)
        {
            var seasonEndDate = DateCalculator.GetSeasonEndDate(seasonStartYear);
            return _leagueBuilder.GetLeagueOnDate(tier, seasonStartYear, seasonEndDate);
        }

        [HttpGet("[action]")]
        public LeagueDto GetCompletedLeagueForTeam(int seasonStartYear, string team)
        {
            var tier = _tierRepository.GetTierForTeamInYear(seasonStartYear, team);
            var seasonEndDate = DateCalculator.GetSeasonEndDate(seasonStartYear);
            return tier == null
                ? new LeagueDto()
                : _leagueBuilder.GetLeagueOnDate((int) tier, seasonStartYear, seasonEndDate);
        }

        [HttpGet("[action]")]
        public LeagueDto GetLeagueOnDate(int tier, DateTime date)
        {
            var seasonStartYear = DateCalculator.GetSeasonStartYear(date);
            return _leagueBuilder.GetLeagueOnDate(tier, seasonStartYear, date);
        }
    }
}
