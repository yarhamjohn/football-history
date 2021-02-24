using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Builders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
{
    [Route("api/[controller]")]
    public class SeasonController : Controller
    {
        private readonly ISeasonBuilder _seasonBuilder;

        public SeasonController(ISeasonBuilder seasonBuilder)
        {
            _seasonBuilder = seasonBuilder;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Season>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Season>> GetSeasons()
        {
            try
            {
                // TODO: This is a hack to limit the returned data - should be removed/updated as more data is added
                return Ok(_seasonBuilder.GetSeasons().Where(s => s.StartYear >= 1958).ToList());
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
