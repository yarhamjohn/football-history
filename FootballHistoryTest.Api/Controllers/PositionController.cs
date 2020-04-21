using System.Collections.Generic;
using FootballHistoryTest.Api.Builders;
using FootballHistoryTest.Api.Repositories.Tier;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistoryTest.Api.Controllers
{
    [Route("api/[controller]")]
    public class PositionController : Controller
    {
        private readonly IPositionBuilder _positionBuilder;
        private readonly ITierRepository _tierRepository;

        public PositionController(IPositionBuilder leagueBuilder, ITierRepository tierRepository)
        {
            _positionBuilder = leagueBuilder;
            _tierRepository = tierRepository;
        }

        [HttpGet("[action]")]
        public List<LeaguePosition> GetLeaguePositions(int seasonStartYear, string team)
        {
            var tier = _tierRepository.GetTierForTeamInYear(seasonStartYear, team);
            return tier == null ? new List<LeaguePosition>() : _positionBuilder.GetLeaguePositions(seasonStartYear, (int) tier, team);
        }

        [HttpGet("[action]")]
        public List<HistoricalPosition> GetHistoricalPositions(int startYear, int endYear, string team)
        {
            return _positionBuilder.GetHistoricalPositions(startYear, endYear, team);
        }

        [HttpGet("[action]")]
        public List<HistoricalPosition> GetHistoricalPositionsForSeasons(List<int> seasonStartYears, string team)
        {
            return _positionBuilder.GetHistoricalPositionsForSeasons(seasonStartYears, team);
        }
    }
}