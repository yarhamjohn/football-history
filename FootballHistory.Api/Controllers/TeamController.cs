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
                
        [HttpGet("[action]")]
        public List<HistoricalPosition> GetHistoricalPositions(string team)
        {
            return new List<HistoricalPosition>
            {
                new HistoricalPosition { Season = "2015 - 2016", AbsolutePosition = 5, Status = "" },
                new HistoricalPosition { Season = "2016 - 2017", AbsolutePosition = 15, Status = "P" },
                new HistoricalPosition { Season = "2017 - 2018", AbsolutePosition = 25, Status = "R" },
                new HistoricalPosition { Season = "2018 - 2019", AbsolutePosition = 20, Status = "P" },
                new HistoricalPosition { Season = "2019 - 2020", AbsolutePosition = 30, Status = "" },
                new HistoricalPosition { Season = "2020 - 2021", AbsolutePosition = 40, Status = "R" }
            };
        }
    }

    public class HistoricalPosition
    {
        public string Season { get; set; }
        public int AbsolutePosition { get; set; }
        public string Status { get; set; }
    }
}
