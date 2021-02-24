using System;
using System.Collections.Generic;
using football.history.api.Builders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
{
    [Route("api/[controller]")]
    public class MatchController : Controller
    {
        private readonly IMatchBuilder _matchBuilder;

        public MatchController(IMatchBuilder matchBuilder)
        {
            _matchBuilder = matchBuilder;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Match>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Match>> GetLeagueMatches(
            List<int> seasonStartYears,
            List<int> tiers,
            List<string> teams)
        {
            try
            {
                return Ok(_matchBuilder.GetLeagueMatches(seasonStartYears, tiers, teams));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Match>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Match>> GetHeadToHeadLeagueMatches(
            List<int> seasonStartYears,
            List<int> tiers,
            string teamOne,
            string teamTwo)
        {
            try
            {
                return Ok(
                    _matchBuilder.GetHeadToHeadLeagueMatches(
                        seasonStartYears,
                        tiers,
                        teamOne,
                        teamTwo));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<KnockoutMatch>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<KnockoutMatch>> GetPlayOffMatches(
            List<int> seasonStartYears,
            List<int> tiers)
        {
            try
            {
                return Ok(_matchBuilder.GetPlayOffMatches(seasonStartYears, tiers));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
