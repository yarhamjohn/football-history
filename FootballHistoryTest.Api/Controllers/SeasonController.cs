using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Builders;
using FootballHistoryTest.Api.Repositories.Season;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistoryTest.Api.Controllers
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
            return _seasonBuilder.GetSeasons();
        }
    }
}