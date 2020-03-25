using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Repositories.Season;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistoryTest.Api.Controllers
{
    [Route("api/[controller]")]
    public class SeasonController : Controller
    {
        private readonly ISeasonRepository _seasonRepository;

        public SeasonController(ISeasonRepository seasonRepository)
        {
            _seasonRepository = seasonRepository;
        }

        [HttpGet("[action]")]
        public List<Season> GetSeasons()
        {
            var seasonModels = _seasonRepository.GetSeasonModels();
            return seasonModels.GroupBy(s => s.SeasonStartYear,
                (startYear, models) => new Season
                {
                    StartYear = startYear, EndYear = startYear + 1,
                    Divisions = models.Select(m => new Division {Name = m.Name, Tier = m.Tier}).ToList()
                }).ToList();
        }
    }

    public class Season
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public List<Division> Divisions { get; set; }
    }

    public class Division
    {
        public string Name { get; set; }
        public int Tier { get; set; }
    }
}