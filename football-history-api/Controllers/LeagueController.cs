using System;
using football.history.api.Builders;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LeagueDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<LeagueDto> GetCompletedLeagueForTier(int seasonStartYear, int tier)
        {
            try
            {
                return Ok(_leagueBuilder.BuildForTier(seasonStartYear, tier));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LeagueDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<LeagueDto> GetCompletedLeagueForTeam(int seasonStartYear, string team)
        {
            try
            {
                return Ok(_leagueBuilder.BuildForTeam(seasonStartYear, team));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LeagueDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<LeagueDto> GetLeagueOnDateForTier(int tier, DateTime date)
        {
            try
            {
                return Ok(_leagueBuilder.BuildForTier(date, tier));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LeagueDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<LeagueDto> GetLeagueOnDateForTeam(string team, DateTime date)
        {
            try
            {
                return Ok(_leagueBuilder.BuildForTeam(date, team));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
