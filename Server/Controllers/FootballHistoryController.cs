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
        public LeagueSeason GetLeagueSeason(string tier, string season)
        {
            var selectedTier = tier == "null" ? (int?) null : Convert.ToInt32(tier);
            return m_Repository.GetLeagueSeason(selectedTier, season == "null" ? null : season);
        }
    }
}
