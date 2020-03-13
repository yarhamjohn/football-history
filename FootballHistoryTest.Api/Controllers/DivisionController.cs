using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Repositories.Division;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistoryTest.Api.Controllers
{
    [Route("api/[controller]")]
    public class DivisionController : Controller
    {
        private readonly IDivisionRepository _divisionRepository;

        public DivisionController(IDivisionRepository divisionRepository)
        {
            _divisionRepository = divisionRepository;
        }
        
        [HttpGet("[action]")]
        public List<Division> GetTierDivisionHistory(int? tier)
        {
            return _divisionRepository.GetDivisionModels(tier)
                .Select(d => new Division { Name = d.Name, Tier = d.Tier, YearActiveFrom = d.From, YearActiveTo = d.To} )
                .ToList();
        }
    }

    public class Division
    {
        public string Name { get; set; }
        public int Tier { get; set; }
        public int YearActiveFrom { get; set; }
        public int YearActiveTo { get; set; }
    }

}
