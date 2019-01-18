using System;
using System.Collections.Generic;
using FootballHistory.Server.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistory.Server.Controllers
{
    [Route("api/[controller]")]
    public class LeagueSeasonController : Controller
    {
        private ILeagueSeasonRepository LeagueSeasonRepository { get; }
        private IFilterRepository FilterRepository { get; }

        public LeagueSeasonController(ILeagueSeasonRepository leagueSeasonRepository, IFilterRepository filterRepository)
        {
            LeagueSeasonRepository = leagueSeasonRepository;
            FilterRepository = filterRepository;
        }

        [HttpGet("[action]")]
        public FilterOptions GetFilterOptions()
        {
            return FilterRepository.GetFilterOptions();
        }
        
        [HttpGet("[action]")]
        public DefaultFilter GetDefaultFilter()
        {
            return FilterRepository.GetDefaultFilter();
        }
                
        [HttpGet("[action]")]
        public List<ResultMatrixRow> GetResultMatrix(string tier, string season)
        {
            return LeagueSeasonRepository.GetResultMatrix(Convert.ToInt32(tier), season);
        }
                                
        [HttpGet("[action]")]
        public PlayOffs GetPlayOffMatches(string tier, string season)
        {
            return LeagueSeasonRepository.GetPlayOffMatches(Convert.ToInt32(tier), season);
        }
                
        [HttpGet("[action]")]
        public List<LeagueTableRow> GetLeagueTable(string tier, string season)
        {
            return LeagueSeasonRepository.GetLeagueTable(Convert.ToInt32(tier), season);
        }
                
        [HttpGet("[action]")]
        public LeagueRowDrillDown GetDrillDown(string tier, string season, string team)
        {
            return LeagueSeasonRepository.GetDrillDown(Convert.ToInt32(tier), season, team);
        }
    }
}
