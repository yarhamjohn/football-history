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
        public LeagueTable GetLeague(string tier, string season)
        {
            return m_Repository.GetLeagueTable(Convert.ToInt32(tier), season);
        }

        [HttpGet("[action]")]
        public LeagueFilterOptions GetLeagueFilterOptions()
        {
            return m_Repository.GetLeagueFilterOptions();
        }
        
        [HttpGet("[action]")]
        public List<Results> GetMatchResultMatrix(string tier, string season)
        {
            return m_Repository.GetMatchResultMatrix(Convert.ToInt32(tier), season);
        }
                
        [HttpGet("[action]")]
        public PlayOff GetPlayOffMatches(string tier, string season)
        {
            return m_Repository.GetPlayOffMatches(Convert.ToInt32(tier), season);
        }
                        
        [HttpGet("[action]")]
        public List<Match> GetLeagueTableDrillDown(string tier, string season, string team)
        {
            return m_Repository.GetLeagueTableDrillDown(Convert.ToInt32(tier), season, team);
        }
    }
}
