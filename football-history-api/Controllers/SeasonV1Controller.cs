using System.Collections.Generic;
using System.Linq;
using football.history.api.Builders;
using Microsoft.AspNetCore.Mvc;

namespace football.history.api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/Season")]
    public class SeasonV1Controller : Controller
    {
        private readonly ISeasonBuilder _seasonBuilder;

        public SeasonV1Controller(ISeasonBuilder seasonBuilder)
        {
            _seasonBuilder = seasonBuilder;
        }

        [HttpGet]
        [MapToApiVersion("1")]
        [Route("GetSeasons")]
        public List<Season> GetSeasons()
        {
            // TODO: This is a hack to limit the returned data - should be removed/updated as more data is added
            return _seasonBuilder.GetSeasons().Where(s => s.StartYear >= 1958).ToList();
        }
    }
}
