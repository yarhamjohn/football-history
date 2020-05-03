using System.Collections.Generic;
using FootballHistoryTest.Api.Builders;
using FootballHistoryTest.Api.Repositories.Tier;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistoryTest.Api.Controllers
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
        public List<Team> GetAllTeams()
        {
            return _teamBuilder.GetAllTeams();
        }
        
        [HttpGet("[action]")]
        public List<Team> GetTeamsInLeague(int seasonStartYear, int tier)
        {
            return _teamBuilder.GetTeamsInLeague(seasonStartYear, tier);
        }

        [HttpGet("[action]")]
        public int GetTier(int seasonStartYear, string team)
        {
            return _tierRepository.GetTierForTeamInYear(seasonStartYear, team) ?? -1;
        }
    }
}
