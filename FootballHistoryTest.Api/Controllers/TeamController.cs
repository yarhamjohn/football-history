using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Repositories.Team;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistoryTest.Api.Controllers
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
        public List<Team> GetTeams()
        {
            return _teamRepository.GetTeamModels()
                .Select(t => new Team() {Name = t.Name, Abbreviation = t.Abbreviation})
                .ToList();
        }
    }

    public class Team
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }
}