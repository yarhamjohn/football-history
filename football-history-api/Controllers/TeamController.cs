using System.Collections.Generic;
using football.history.api.Builders;
using football.history.api.Repositories.Tier;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TeamController : Controller
    {
        private readonly ITeamBuilder _teamBuilder;
        private readonly ITierRepository _tierRepository;

        public TeamController(ITeamBuilder teamBuilder, ITierRepository tierRepository)
        {
            _teamBuilder = teamBuilder;
            _tierRepository = tierRepository;
        }

        [HttpGet]
        [MapToApiVersion("1")]
        [Route("GetAllTeams")]
        public List<Team> GetAllTeams() => _teamBuilder.GetAllTeams();

        [HttpGet]
        [MapToApiVersion("1")]
        [Route("GetTeamsInLeague")]
        public List<Team> GetTeamsInLeague(int seasonStartYear, int tier) =>
            _teamBuilder.GetTeamsInLeague(seasonStartYear, tier);

        [HttpGet]
        [MapToApiVersion("1")]
        [Route("GetTier")]
        public int GetTier(int seasonStartYear, string team) =>
            _tierRepository.GetTierForTeamInYear(seasonStartYear, team) ?? -1;
    }
}
