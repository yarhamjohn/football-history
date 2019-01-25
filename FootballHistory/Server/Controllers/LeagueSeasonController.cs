using System;
using System.Collections.Generic;
using FootballHistory.Server.Builders;
using FootballHistory.Server.Builders.Models;
using FootballHistory.Server.Models.LeagueSeason;
using FootballHistory.Server.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistory.Server.Controllers
{
    [Route("api/[controller]")]
    public class LeagueSeasonController : Controller
    {
        private readonly IDivisionRepository _divisionRepository;
        private readonly ILeagueSeasonFilterBuilder _leagueSeasonFilterBuilder;
        private readonly IResultMatrixRepository _resultMatrixRepository;
        private readonly IResultMatrixBuilder _resultMatrixBuilder;
        private readonly ILeagueSeasonRepository _leagueSeasonRepository;

        public LeagueSeasonController(
            ILeagueSeasonRepository leagueSeasonRepository, 
            IDivisionRepository divisionRepository, 
            ILeagueSeasonFilterBuilder leagueSeasonFilterBuilder, 
            IResultMatrixRepository resultMatrixRepository, 
            IResultMatrixBuilder resultMatrixBuilder)
        {
            _divisionRepository = divisionRepository;
            _leagueSeasonFilterBuilder = leagueSeasonFilterBuilder;
            _resultMatrixRepository = resultMatrixRepository;
            _resultMatrixBuilder = resultMatrixBuilder;
            _leagueSeasonRepository = leagueSeasonRepository;
        }

        [HttpGet("[action]")]
        public LeagueSeasonFilter GetLeagueSeasonFilters()
        {
            var divisionModels = _divisionRepository.GetDivisionModels();
            return _leagueSeasonFilterBuilder.Build(divisionModels);
        }
        
        [HttpGet("[action]")]
        public ResultMatrix GetResultMatrix(string tier, string season)
        {
            var matchDetails = _resultMatrixRepository.GetResultMatrix(Convert.ToInt32(tier), season);
            return _resultMatrixBuilder.Build(matchDetails);
        }
                                
        [HttpGet("[action]")]
        public PlayOffs GetPlayOffMatches(string tier, string season)
        {
            return _leagueSeasonRepository.GetPlayOffMatches(Convert.ToInt32(tier), season);
        }
                
        [HttpGet("[action]")]
        public List<LeagueTableRow> GetLeagueTable(string tier, string season)
        {
            return _leagueSeasonRepository.GetLeagueTable(Convert.ToInt32(tier), season);
        }
                
        [HttpGet("[action]")]
        public LeagueRowDrillDown GetDrillDown(string tier, string season, string team)
        {
            return _leagueSeasonRepository.GetDrillDown(Convert.ToInt32(tier), season, team);
        }
    }
}
