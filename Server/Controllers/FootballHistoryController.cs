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
        public FilterOptions GetFilterOptions()
        {
            return m_Repository.GetFilterOptions();
        }
        
        [HttpGet("[action]")]
        public LeagueSeason GetLeagueSeason(string tier, string season)
        {
            return m_Repository.GetLeagueSeason(Convert.ToInt32(tier), season);
        }
    }
}
