using System;
using System.Collections.Generic;
using football.history.api.Builders;
using football.history.api.Repositories.Tier;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
{
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        private readonly ITeamBuilder _teamBuilder;
        private readonly ITierRepository _tierRepository;

        public TeamController(ITeamBuilder teamBuilder, ITierRepository tierRepository)
        {
            _teamBuilder = teamBuilder;
            _tierRepository = tierRepository;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Team>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Team>> GetAllTeams()
        {
            try
            {
                return Ok(_teamBuilder.GetAllTeams());
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Team>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Team>> GetTeamsInLeague(int seasonStartYear, int tier)
        {
            try
            {
                return Ok(_teamBuilder.GetTeamsInLeague(seasonStartYear, tier));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<int> GetTier(int seasonStartYear, string team)
        {
            try
            {
                return Ok(_tierRepository.GetTierForTeamInYear(seasonStartYear, team));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
