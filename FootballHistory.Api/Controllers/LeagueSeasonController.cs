using System;
using System.Collections.Generic;
using FootballHistory.Api.LeagueSeason.LeagueSeasonFilter;
using FootballHistory.Api.LeagueSeason.LeagueTable;
using FootballHistory.Api.LeagueSeason.LeagueTableDrillDown;
using FootballHistory.Api.LeagueSeason.PlayOffs;
using FootballHistory.Api.LeagueSeason.ResultMatrix;
using FootballHistory.Api.Repositories;
using FootballHistory.Api.Repositories.DivisionRepository;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using FootballHistory.Api.Repositories.PointDeductionRepository;
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
        private readonly IPointDeductionsRepository _pointDeductionsRepository;
        private readonly ILeagueDetailRepository _leagueDetailRepository;
        private readonly ILeagueTableBuilder _leagueTableBuilder;
        private readonly ILeagueTableDrillDownBuilder _leagueTableDrillDownBuilder;
        private readonly IResultMatrixBuilder _resultMatrixBuilder;
        private readonly ILeagueSeasonFilterBuilder _leagueSeasonFilterBuilder;

        public LeagueSeasonController(
            ILeagueTableBuilder leagueTableBuilder, 
            IDivisionRepository divisionRepository, 
            ILeagueSeasonFilterBuilder leagueSeasonFilterBuilder, 
            ILeagueMatchesRepository leagueMatchesRepository, 
            IResultMatrixBuilder resultMatrixBuilder,
            IPlayOffMatchesRepository playOffMatchesRepository,
            ILeagueTableDrillDownBuilder leagueTableDrillDownBuilder,
            IPlayOffMatchesBuilder playOffMatchesBuilder,
            IPointDeductionsRepository pointDeductionsRepository,
            ILeagueDetailRepository leagueDetailRepository)
        {
            _divisionRepository = divisionRepository;
            _leagueSeasonFilterBuilder = leagueSeasonFilterBuilder;
            _leagueMatchesRepository = leagueMatchesRepository;
            _resultMatrixBuilder = resultMatrixBuilder;
            _leagueTableBuilder = leagueTableBuilder;
            _playOffMatchesRepository = playOffMatchesRepository;
            _leagueTableDrillDownBuilder = leagueTableDrillDownBuilder;
            _playOffMatchesBuilder = playOffMatchesBuilder;
            _pointDeductionsRepository = pointDeductionsRepository;
            _leagueDetailRepository = leagueDetailRepository;
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
        public LeagueTable GetLeagueTable(string tier, string season)
        {
            var divisionTier = Convert.ToInt32(tier);
            var leagueMatchDetails = _leagueMatchesRepository.GetLeagueMatches(divisionTier, season);
            var leagueDetail = _leagueDetailRepository.GetLeagueInfo(divisionTier, season);
            var pointDeductions = _pointDeductionsRepository.GetPointDeductions(divisionTier, season);
            var playOffMatches = _playOffMatchesRepository.GetPlayOffMatches(divisionTier, season);

            return _leagueTableBuilder.Build(leagueMatchDetails, pointDeductions, leagueDetail, playOffMatches);
        }
                
        [HttpGet("[action]")]
        public LeagueTableDrillDown GetDrillDown(string tier, string season, string team)
        {
            var divisionTier = Convert.ToInt32(tier);
            var matchDetails = _leagueMatchesRepository.GetLeagueMatches(divisionTier, season);
            var pointDeductions = _pointDeductionsRepository.GetPointDeductions(divisionTier, season);

            return _leagueTableDrillDownBuilder.Build(team, matchDetails, pointDeductions);
        }
    }
}
