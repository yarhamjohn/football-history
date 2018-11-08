using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using football_history.Server.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace football_history.Controllers
{
    [Route("api/[controller]")]
    public class FootballHistoryController : Controller
    {
        public IFootballHistoryRepository m_Repository { get; }
        public FootballHistoryController(IFootballHistoryRepository repository)
        {
            m_Repository = repository;
        }

        [HttpGet("[action]")]
        public LeagueTable GetLeague(string competition, string season)
        {
            return m_Repository.GetLeagueTable(competition, season);
        }

        [HttpGet("[action]")]
        public LeagueFilterOptions GetLeagueFilterOptions()
        {
            return m_Repository.GetLeagueFilterOptions();
        }
    }
}
