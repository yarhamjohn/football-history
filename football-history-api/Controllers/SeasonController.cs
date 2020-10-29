using System.Collections.Generic;
using System.Linq;
using football.history.api.Builders;
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
        public List<Season> GetSeasons()
        {
            // TODO: This is a hack to limit the returned data - should be removed/updated as more data is added
            return _seasonBuilder.GetSeasons().Where(s => s.StartYear >= 1989).ToList();
        }
    }
}
