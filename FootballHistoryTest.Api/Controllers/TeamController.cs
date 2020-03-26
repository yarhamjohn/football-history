using System.Collections.Generic;
using FootballHistoryTest.Api.Builders;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistoryTest.Api.Controllers
{
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        private readonly ITeamBuilder _teamBuilder;

        public TeamController(ITeamBuilder teamBuilder)
        {
            _teamBuilder = teamBuilder;
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
    }
}