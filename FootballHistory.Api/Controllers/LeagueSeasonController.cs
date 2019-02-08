using System;
using System.Collections.Generic;
using FootballHistory.Api.Builders;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistory.Api.Controllers
{
    [Route("api/[controller]")]
    public class LeagueSeasonController : Controller
    {
        private readonly IDivisionRepository _divisionRepository;
        private readonly IResultMatrixRepository _resultMatrixRepository;
        private readonly IPlayOffMatchesRepository _playOffMatchesRepository;
        private readonly IPlayOffMatchesBuilder _playOffMatchesBuilder;
        private readonly ILeagueSeasonRepository _leagueSeasonRepository;
        private readonly ILeagueTableDrillDownRepository _leagueTableDrillDownRepository;
        private readonly IResultMatrixBuilder _resultMatrixBuilder;
        private readonly ILeagueSeasonFilterBuilder _leagueSeasonFilterBuilder;

        public LeagueSeasonController(
            ILeagueSeasonRepository leagueSeasonRepository, 
            IDivisionRepository divisionRepository, 
            ILeagueSeasonFilterBuilder leagueSeasonFilterBuilder, 
            IResultMatrixRepository resultMatrixRepository, 
            IResultMatrixBuilder resultMatrixBuilder,
            IPlayOffMatchesRepository playOffMatchesRepository,
            ILeagueTableDrillDownRepository leagueTableDrillDownRepository,
            IPlayOffMatchesBuilder playOffMatchesBuilder)
        {
            _divisionRepository = divisionRepository;
            _leagueSeasonFilterBuilder = leagueSeasonFilterBuilder;
            _resultMatrixRepository = resultMatrixRepository;
            _resultMatrixBuilder = resultMatrixBuilder;
            _leagueSeasonRepository = leagueSeasonRepository;
            _playOffMatchesRepository = playOffMatchesRepository;
            _leagueTableDrillDownRepository = leagueTableDrillDownRepository;
            _playOffMatchesBuilder = playOffMatchesBuilder;
        }

        [HttpGet("[action]")]
        public LeagueSeasonFilter GetLeagueSeasonFilters()
        {
            var divisions = _divisionRepository.GetDivisions();
            return _leagueSeasonFilterBuilder.Build(divisions);
        }
        
        [HttpGet("[action]")]
        public ResultMatrix GetResultMatrix(string tier, string season)
        {
            var matchDetails = _resultMatrixRepository.GetLeagueMatches(Convert.ToInt32(tier), season);
            return _resultMatrixBuilder.Build(matchDetails);
        }
                                
        [HttpGet("[action]")]
        public PlayOffs GetPlayOffMatches(string tier, string season)
        {
            var matchDetails = _playOffMatchesRepository.GetPlayOffMatches(Convert.ToInt32(tier), season);
            return _playOffMatchesBuilder.Build(matchDetails);
        }
                
        [HttpGet("[action]")]
        public List<LeagueTableRow> GetLeagueTable(string tier, string season)
        {
            return _leagueSeasonRepository.GetLeagueTable(Convert.ToInt32(tier), season);
        }
                
        [HttpGet("[action]")]
        public LeagueRowDrillDown GetDrillDown(string tier, string season, string team)
        {
            return _leagueTableDrillDownRepository.GetDrillDown(Convert.ToInt32(tier), season, team);
        }
    }
}
