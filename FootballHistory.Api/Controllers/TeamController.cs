using System.Collections.Generic;
using FootballHistory.Api.Repositories.TeamRepository;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistory.Api.Controllers
{
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        private readonly ITeamRepository _teamRepository;

        public TeamController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }
        
        [HttpGet("[action]")]
        public List<string> GetTeamFilters()
        {
            return _teamRepository.GetAllTeams();
        }
    }
}
