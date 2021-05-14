using System.Collections.Generic;
using football.history.api.Builders;
using football.history.api.Repositories.Tier;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/Position")]
    public class PositionV1Controller : Controller
    {
        private readonly IPositionBuilder _positionBuilder;
        private readonly ITierRepository _tierRepository;

        public PositionV1Controller(IPositionBuilder leagueBuilder, ITierRepository tierRepository)
        {
            _positionBuilder = leagueBuilder;
            _tierRepository = tierRepository;
        }

        [HttpGet]
        [MapToApiVersion("1")]
        [Route("GetLeaguePositions")]
        public List<LeaguePosition> GetLeaguePositions(int seasonStartYear, string team)
        {
            var tier = _tierRepository.GetTierForTeamInYear(seasonStartYear, team);
            return tier == null
                ? new List<LeaguePosition>()
                : _positionBuilder.GetLeaguePositions(seasonStartYear, (int) tier, team);
        }

        [HttpGet]
        [MapToApiVersion("1")]
        [Route("GetHistoricalPositions")]
        public List<HistoricalPosition> GetHistoricalPositions(
            int startYear,
            int endYear,
            string team) =>
            _positionBuilder.GetHistoricalPositions(startYear, endYear, team);

        [HttpGet]
        [MapToApiVersion("1")]
        [Route("GetHistoricalPositionsForSeasons")]
        public List<HistoricalPosition> GetHistoricalPositionsForSeasons(
            List<int> seasonStartYears,
            string team) =>
            _positionBuilder.GetHistoricalPositionsForSeasons(seasonStartYears, team);
    }
}
