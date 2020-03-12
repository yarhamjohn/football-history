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
        public List<SeasonDates> GetSeasonDates()
        {
            var seasonDatesModels = _seasonRepository.GetSeasonDateModels();
            return seasonDatesModels
                .Select(s => new SeasonDates {StartYear = s.SeasonStartYear, EndYear = s.SeasonEndYear})
                .ToList();
        }
    }
    
    public class SeasonDates
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }
    }
}