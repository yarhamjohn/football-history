using System;
using football.history.api.Builders;
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
        public League GetCompletedLeague(int seasonStartYear, int tier)
        {
            var seasonEndDate = GetSeasonEndDate(seasonStartYear); 
            return _leagueBuilder.GetLeagueOnDate(tier, seasonEndDate);
        }

        [HttpGet("[action]")]
        public League GetCompletedLeagueForTeam(int seasonStartYear, string team)
        {
            var tier = _tierRepository.GetTierForTeamInYear(seasonStartYear, team);
            var seasonEndDate = GetSeasonEndDate(seasonStartYear); 
            return tier == null ? new League() : _leagueBuilder.GetLeagueOnDate((int)tier, seasonEndDate);
        }

        [HttpGet("[action]")]
        public League GetLeagueOnDate(int tier, DateTime date)
        {
            return _leagueBuilder.GetLeagueOnDate(tier, date);
        }

        private static DateTime GetSeasonEndDate(int seasonStartYear)
        {
            /*
             * Originally the season end date was set to be June 30th as this was roughly half-way between seasons.
             * Due to COVID-19, there was a delay in the fixtures meaning 2019-2020 actually finished on August 4th
             * with the Championship play-off final. Although the 2020-2021 league season did not commence until
             * September, some cup games were held from 29th August so the middle of August is set here.
             */
            if (seasonStartYear == 2019)
            {
                return new DateTime(seasonStartYear + 1, 08, 20);
            }
            
            return new DateTime(seasonStartYear + 1, 06, 30);
        }
    }
}
