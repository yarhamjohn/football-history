using System;
using System.Collections.Generic;
using football.history.api.Builders;
using football.history.api.Exceptions;
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
        public List<Team> GetAllTeams() => _teamBuilder.GetAllTeams();

        [HttpGet("[action]")]
        public List<Team> GetTeamsInLeague(int seasonStartYear, int tier) =>
            _teamBuilder.GetTeamsInLeague(seasonStartYear, tier);

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<int> GetTier(int seasonStartYear, string team)
        {
            try
            {
                return _tierRepository.GetTierForTeamInYear(seasonStartYear, team);
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
