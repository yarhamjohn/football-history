using System;
using football.history.api.Builders;
using football.history.api.Calculators;
using football.history.api.Exceptions;
using football.history.api.Repositories.Tier;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LeagueDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<LeagueDto> GetCompletedLeagueForTeam(int seasonStartYear, string team)
        {
            try
            {
                return _leagueBuilder.BuildForTeam(seasonStartYear, team);
            }
            catch (TierNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public LeagueDto GetLeagueOnDateForTier(int tier, DateTime date)
        {
            return _leagueBuilder.BuildForTier(date, tier);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LeagueDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<LeagueDto> GetLeagueOnDateForTeam(string team, DateTime date)
        {
            try
            {
                return _leagueBuilder.BuildForTeam(date, team);
            }
            catch (TierNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
