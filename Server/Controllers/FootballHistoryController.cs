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
        public LeagueTable GetLeague()
        {
            return m_Repository.GetLeagueTable(CompetitionNames.PremierLeague, 2005);
        }
    }
}
