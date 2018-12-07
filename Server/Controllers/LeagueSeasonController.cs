using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using football_history.Server.Repositories;
using Microsoft.AspNetCore.Mvc;
using static football_history.Server.Repositories.LeagueSeasonRepository;

namespace football_history.Controllers
{
    [Route("api/[controller]")]
    public class LeagueSeasonController : Controller
    {
        public ILeagueSeasonRepository m_Repository { get; }
        public LeagueSeasonController(ILeagueSeasonRepository repository)
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
                
        [HttpGet("[action]")]
        public LeagueRowDrillDown GetDrillDown(string tier, string season, string team)
        {
            return m_Repository.GetDrillDown(Convert.ToInt32(tier), season, team);
        }
    }
}
