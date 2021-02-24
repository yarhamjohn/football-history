using System;
using System.Collections.Generic;
using football.history.api.Builders;
using football.history.api.Exceptions;
using football.history.api.Repositories.Tier;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LeaguePosition))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<LeaguePosition>> GetLeaguePositions(int seasonStartYear, string team)
        {
            try
            {
                var tier = _tierRepository.GetTierForTeamInYear(seasonStartYear, team);
                return _positionBuilder.GetLeaguePositions(seasonStartYear, tier, team);
            }
            catch (TierNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
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
