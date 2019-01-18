using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballHistory.Server.Repositories;
using Microsoft.AspNetCore.Mvc;
using static FootballHistory.Server.Repositories.LeagueSeasonRepository;

namespace FootballHistory.Controllers
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
        public DefaultFilter GetDefaultFilter()
        {
            return m_Repository.GetDefaultFilter();
        }
                
        [HttpGet("[action]")]
        public List<ResultMatrixRow> GetResultMatrix(string tier, string season)
        {
            return m_Repository.GetResultMatrix(Convert.ToInt32(tier), season);
        }
                                
        [HttpGet("[action]")]
        public PlayOffs GetPlayOffMatches(string tier, string season)
        {
            return m_Repository.GetPlayOffMatches(Convert.ToInt32(tier), season);
        }
                
        [HttpGet("[action]")]
        public List<LeagueTableRow> GetLeagueTable(string tier, string season)
        {
            return m_Repository.GetLeagueTable(Convert.ToInt32(tier), season);
        }
                
        [HttpGet("[action]")]
        public LeagueRowDrillDown GetDrillDown(string tier, string season, string team)
        {
            return m_Repository.GetDrillDown(Convert.ToInt32(tier), season, team);
        }
    }
}
