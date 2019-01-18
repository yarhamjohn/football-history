using System;
using System.Collections.Generic;
using FootballHistory.Server.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistory.Server.Controllers
{
    [Route("api/[controller]")]
    public class LeagueSeasonController : Controller
    {
        private ILeagueSeasonRepository Repository { get; }
        public LeagueSeasonController(ILeagueSeasonRepository repository)
        {
            Repository = repository;
        }

        [HttpGet("[action]")]
        public FilterOptions GetFilterOptions()
        {
            return Repository.GetFilterOptions();
        }
        
        [HttpGet("[action]")]
        public DefaultFilter GetDefaultFilter()
        {
            return Repository.GetDefaultFilter();
        }
                
        [HttpGet("[action]")]
        public List<ResultMatrixRow> GetResultMatrix(string tier, string season)
        {
            return Repository.GetResultMatrix(Convert.ToInt32(tier), season);
        }
                                
        [HttpGet("[action]")]
        public PlayOffs GetPlayOffMatches(string tier, string season)
        {
            return Repository.GetPlayOffMatches(Convert.ToInt32(tier), season);
        }
                
        [HttpGet("[action]")]
        public List<LeagueTableRow> GetLeagueTable(string tier, string season)
        {
            return Repository.GetLeagueTable(Convert.ToInt32(tier), season);
        }
                
        [HttpGet("[action]")]
        public LeagueRowDrillDown GetDrillDown(string tier, string season, string team)
        {
            return Repository.GetDrillDown(Convert.ToInt32(tier), season, team);
        }
    }
}
