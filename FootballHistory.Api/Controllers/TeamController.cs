using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.LeagueSeason.LeagueSeasonFilter;
using FootballHistory.Api.LeagueSeason.LeagueTable;
using FootballHistory.Api.LeagueSeason.LeagueTableDrillDown;
using FootballHistory.Api.LeagueSeason.PlayOffs;
using FootballHistory.Api.LeagueSeason.ResultMatrix;
using FootballHistory.Api.Repositories.DivisionRepository;
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

        public TeamController(ITeamRepository teamRepository,
            ILeagueTableBuilder leagueTableBuilder, 
            ILeagueMatchesRepository leagueMatchesRepository, 
            IPlayOffMatchesRepository playOffMatchesRepository,
            IPointDeductionsRepository pointDeductionsRepository,
            ILeagueDetailRepository leagueDetailRepository,
            ITierRepository tierRepository)
        {
            _teamRepository = teamRepository;
            _leagueTableBuilder = leagueTableBuilder;
            _leagueMatchesRepository = leagueMatchesRepository;
            _playOffMatchesRepository = playOffMatchesRepository;
            _pointDeductionsRepository = pointDeductionsRepository;
            _leagueDetailRepository = leagueDetailRepository;
            _tierRepository = tierRepository;
        }
        
        [HttpGet("[action]")]
        public List<string> GetTeamFilters()
        {
            return _teamRepository.GetAllTeams();
        }
                
        [HttpGet("[action]")]
        public List<HistoricalPosition> GetHistoricalPositions(string team)
        {
            if (team == null)
            {
                return new List<HistoricalPosition>();
            }
            
            var historicalPositions = new List<HistoricalPosition>();
            
            // Get tier for each season
            // get leaguematchdetails, leaguedetail, pointdeductions and playoffmatches for all tier/season combinations
            
            // loop over each season, building a league table with relvant data
            for (var year = 1992; year <= 1992; year++)
            {
                var season = $"{year} - {year + 1}";
                var tier = _tierRepository.GetTier(season, team);
                
                var leagueMatchDetails = _leagueMatchesRepository.GetLeagueMatches(tier, season);
                var leagueDetail = _leagueDetailRepository.GetLeagueInfo(tier, season);
                var pointDeductions = _pointDeductionsRepository.GetPointDeductions(tier, season);
                var playOffMatches = _playOffMatchesRepository.GetPlayOffMatches(tier, season);

                var leagueTable = _leagueTableBuilder.BuildWithStatuses(leagueMatchDetails, pointDeductions, leagueDetail, playOffMatches);
                historicalPositions.Add(new HistoricalPosition
                {
                    AbsolutePosition = tier == 1
                        ? leagueTable.Rows.Single(r => r.Team == team).Position
                        : leagueTable.Rows.Single(r => r.Team == team).Position + ((tier - 1) * 24),
                    Season = season,
                    Status = leagueTable.Rows.Single(r => r.Team == team).Status
                });
            }

            return historicalPositions;
        }
    }
}
