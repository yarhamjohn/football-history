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
            
            var filters = _tierRepository.GetTier(team);

            var leagueMatchDetails = _leagueMatchesRepository.GetLeagueMatches(filters.ToArray());
            var leagueDetails = _leagueDetailRepository.GetLeagueInfos(filters.ToArray());
            var pointDeductions = _pointDeductionsRepository.GetPointDeductions(filters.ToArray());
            var playOffMatches = _playOffMatchesRepository.GetPlayOffMatches(filters.ToArray());
            
            //TODO - think about the singles...
            for (var year = 1992; year <= 2017; year++)
            {
                var season = $"{year} - {year + 1}";
                var filteredLeagueMatchDetails = leagueMatchDetails.Where(m => m.Date > new DateTime(year, 7, 1) && m.Date < new DateTime(year + 1, 6, 30)).ToList();
                var filteredPlayOffMatches = playOffMatches.Where(m => m.Date > new DateTime(year, 7, 1) && m.Date < new DateTime(year + 1, 6, 30)).ToList();
                var filteredPointDeductions = pointDeductions.Where(pd => pd.Season == season).ToList();
                var filteredLeagueDetail = leagueDetails.Where(ld => ld.Season == season).ToList();
                
                var leagueTable = _leagueTableBuilder.BuildWithStatuses(filteredLeagueMatchDetails, filteredPointDeductions, filteredLeagueDetail.Single(d => d.Season == season), filteredPlayOffMatches);
                historicalPositions.Add(new HistoricalPosition
                {
                    AbsolutePosition = filters.Where(st => st.Season == season).Select(st => st.Tier).Single() == 1
                        ? leagueTable.Rows.Single(r => r.Team == team).Position
                        : leagueTable.Rows.Single(r => r.Team == team).Position + ((filters.Where(st => st.Season == season).Select(st => st.Tier).Single() - 1) * 24),
                    Season = season,
                    Status = leagueTable.Rows.Single(r => r.Team == team).Status
                });
            }

            return historicalPositions;
        }
    }
}
