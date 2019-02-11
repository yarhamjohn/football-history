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
        private readonly ILeagueMatchesRepository _leagueMatchesRepository;
        private readonly IPlayOffMatchesRepository _playOffMatchesRepository;
        private readonly IPlayOffMatchesBuilder _playOffMatchesBuilder;
        private readonly ILeagueSeasonBuilder _leagueSeasonBuilder;
        private readonly ILeagueTableDrillDownBuilder _leagueTableDrillDownBuilder;
        private readonly IResultMatrixBuilder _resultMatrixBuilder;
        private readonly ILeagueSeasonFilterBuilder _leagueSeasonFilterBuilder;

        public LeagueSeasonController(
            ILeagueSeasonBuilder leagueSeasonBuilder, 
            IDivisionRepository divisionRepository, 
            ILeagueSeasonFilterBuilder leagueSeasonFilterBuilder, 
            ILeagueMatchesRepository leagueMatchesRepository, 
            IResultMatrixBuilder resultMatrixBuilder,
            IPlayOffMatchesRepository playOffMatchesRepository,
            ILeagueTableDrillDownBuilder leagueTableDrillDownBuilder,
            IPlayOffMatchesBuilder playOffMatchesBuilder)
        {
            _divisionRepository = divisionRepository;
            _leagueSeasonFilterBuilder = leagueSeasonFilterBuilder;
            _leagueMatchesRepository = leagueMatchesRepository;
            _resultMatrixBuilder = resultMatrixBuilder;
            _leagueSeasonBuilder = leagueSeasonBuilder;
            _playOffMatchesRepository = playOffMatchesRepository;
            _leagueTableDrillDownBuilder = leagueTableDrillDownBuilder;
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
            var matchDetails = _leagueMatchesRepository.GetLeagueMatches(Convert.ToInt32(tier), season);
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
            return _leagueSeasonBuilder.GetLeagueTable(Convert.ToInt32(tier), season);
        }
                
        [HttpGet("[action]")]
        public LeagueRowDrillDown GetDrillDown(string tier, string season, string team)
        {
            return _leagueTableDrillDownBuilder.GetDrillDown(Convert.ToInt32(tier), season, team);
        }
    }
}
