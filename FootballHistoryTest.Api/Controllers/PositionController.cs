using System.Collections.Generic;
using FootballHistoryTest.Api.Builders;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistoryTest.Api.Controllers
{
    [Route("api/[controller]")]
    public class PositionController : Controller
    {
        private readonly IPositionBuilder _positionBuilder;

        public PositionController(IPositionBuilder leagueBuilder)
        {
            _positionBuilder = leagueBuilder;
        }

        [HttpGet("[action]")]
        public List<LeaguePosition> GetLeaguePositions(int seasonStartYear, int tier, string team)
        {
            return _positionBuilder.GetLeaguePositions(seasonStartYear, tier, team);
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