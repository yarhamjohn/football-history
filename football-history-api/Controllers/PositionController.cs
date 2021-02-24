using System;
using System.Collections.Generic;
using football.history.api.Builders;
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<LeaguePosition>> GetLeaguePositions(
            int seasonStartYear,
            string team)
        {
            try
            {
                var tier = _tierRepository.GetTierForTeamInYear(seasonStartYear, team);
                return Ok(_positionBuilder.GetLeaguePositions(seasonStartYear, tier, team));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HistoricalPosition))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<HistoricalPosition>> GetHistoricalPositions(
            int startYear,
            int endYear,
            string team)
        {
            try
            {
                return Ok(_positionBuilder.GetHistoricalPositions(startYear, endYear, team));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(HistoricalPosition))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<HistoricalPosition>> GetHistoricalPositionsForSeasons(
            List<int> seasonStartYears,
            string team)
        {
            try
            {
                return Ok(_positionBuilder.GetHistoricalPositionsForSeasons(seasonStartYears, team));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
