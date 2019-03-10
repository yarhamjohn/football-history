using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.LeagueSeason.LeagueTable;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using FootballHistory.Api.Repositories.PointDeductionRepository;
using FootballHistory.Api.Repositories.TeamRepository;
using FootballHistory.Api.Repositories.TierRepository;
using FootballHistory.Api.Team.HistoricalPosition;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistory.Api.Controllers
{
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ILeagueTableBuilder _leagueTableBuilder;
        private readonly ILeagueMatchesRepository _leagueMatchesRepository;
        private readonly IPlayOffMatchesRepository _playOffMatchesRepository;
        private readonly IPointDeductionsRepository _pointDeductionsRepository;
        private readonly ILeagueDetailRepository _leagueDetailRepository;
        private readonly ITierRepository _tierRepository;
        private readonly IHistoricalPositionBuilder _historicalPositionBuilder;

        public TeamController(ITeamRepository teamRepository,
            ILeagueTableBuilder leagueTableBuilder, 
            ILeagueMatchesRepository leagueMatchesRepository, 
            IPlayOffMatchesRepository playOffMatchesRepository,
            IPointDeductionsRepository pointDeductionsRepository,
            ILeagueDetailRepository leagueDetailRepository,
            ITierRepository tierRepository,
            IHistoricalPositionBuilder historicalPositionBuilder)
        {
            _teamRepository = teamRepository;
            _leagueTableBuilder = leagueTableBuilder;
            _leagueMatchesRepository = leagueMatchesRepository;
            _playOffMatchesRepository = playOffMatchesRepository;
            _pointDeductionsRepository = pointDeductionsRepository;
            _leagueDetailRepository = leagueDetailRepository;
            _tierRepository = tierRepository;
            _historicalPositionBuilder = historicalPositionBuilder;
        }
        
        [HttpGet("[action]")]
        public List<string> GetTeamFilters()
        {
            return _teamRepository.GetAllTeams();
        }
                
        [HttpGet("[action]")]
        public List<HistoricalPosition> GetHistoricalPositions(string team, int firstSeasonStartYear, int lastSeasonStartYear)
        {
            if (team == null)
            {
                return new List<HistoricalPosition>();
            }
            
            var filters = _tierRepository.GetSeasonTierFilters(team, firstSeasonStartYear, lastSeasonStartYear).ToArray();
            if (!filters.Any())
            {
                return new List<HistoricalPosition>();
            }
            
            var leagueMatchDetails = _leagueMatchesRepository.GetLeagueMatches(filters);
            var leagueDetails = _leagueDetailRepository.GetLeagueInfos(filters);
            var pointDeductions = _pointDeductionsRepository.GetPointDeductions(filters);
            var playOffMatches = _playOffMatchesRepository.GetPlayOffMatches(filters);
            
            return _historicalPositionBuilder.Build(team, filters, leagueMatchDetails, playOffMatches, pointDeductions, leagueDetails);
        }
    }
}
