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
        public List<HistoricalPosition> GetHistoricalPositions(string team, string seasonStartYear, string seasonEndYear)
        {
            if (team == null)
            {
                return new List<HistoricalPosition>();
            }
            
            var historicalPositions = new List<HistoricalPosition>();
            
            var filters = _tierRepository.GetSeasonTierFilters(team, Convert.ToInt32(seasonStartYear), Convert.ToInt32(seasonEndYear));

            if (!filters.Any())
            {
                return new List<HistoricalPosition>();
            }
            
            var leagueMatchDetails = _leagueMatchesRepository.GetLeagueMatches(filters.ToArray());
            var leagueDetails = _leagueDetailRepository.GetLeagueInfos(filters.ToArray());
            var pointDeductions = _pointDeductionsRepository.GetPointDeductions(filters.ToArray());
            var playOffMatches = _playOffMatchesRepository.GetPlayOffMatches(filters.ToArray());
            
            var years = filters.OrderBy(f => f.Season).Select(f => Convert.ToInt32(f.Season.Substring(0, 4))).ToList();
            foreach (var year in years)
            {
                var season = $"{year} - {year + 1}";
                if (filters.Where(f => f.Season == season).Select(f => f.Tier).Single() == 0)
                {
                    historicalPositions.Add(new HistoricalPosition
                        {AbsolutePosition = 0, Season = season, Status = ""});
                }
                else
                {
                    var filteredLeagueMatchDetails = leagueMatchDetails.Where(m =>
                        m.Date > new DateTime(year, 7, 1) && m.Date < new DateTime(year + 1, 6, 30)).ToList();
                    var filteredPlayOffMatches = playOffMatches.Where(m =>
                        m.Date > new DateTime(year, 7, 1) && m.Date < new DateTime(year + 1, 6, 30)).ToList();
                    var filteredPointDeductions = pointDeductions.Where(pd => pd.Season == season).ToList();
                    var filteredLeagueDetail = leagueDetails.Single(ld => ld.Season == season);

                    var leagueTable = _leagueTableBuilder.BuildWithStatuses(filteredLeagueMatchDetails,
                        filteredPointDeductions, filteredLeagueDetail, filteredPlayOffMatches);
                    var tier = filters.Where(st => st.Season == season).Select(st => st.Tier).Single();
                    var position = leagueTable.Rows.Single(r => r.Team == team).Position;
                    historicalPositions.Add(new HistoricalPosition
                    {
                        AbsolutePosition = tier == 1
                            ? position
                            : position + 20 + ((tier - 2) * 24),
                        Season = season,
                        Status = leagueTable.Rows.Single(r => r.Team == team).Status
                    });
                }
            }

            return historicalPositions;
        }
    }
}
