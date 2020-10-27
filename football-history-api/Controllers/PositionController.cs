using System.Collections.Generic;
using football.history.api.Builders;
using football.history.api.Repositories.Tier;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
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
            return tier == null
                ? new List<LeaguePosition>()
                : _positionBuilder.GetLeaguePositions(seasonStartYear, (int) tier, team);
        }

        [HttpGet("[action]")]
        public List<HistoricalPosition> GetHistoricalPositions(
            int startYear,
            int endYear,
            string team) =>
            _positionBuilder.GetHistoricalPositions(startYear, endYear, team);

        [HttpGet("[action]")]
        public List<HistoricalPosition> GetHistoricalPositionsForSeasons(
            List<int> seasonStartYears,
            string team) =>
            _positionBuilder.GetHistoricalPositionsForSeasons(seasonStartYears, team);
    }
}
