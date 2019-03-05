using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistory.Api.Controllers
{
    [Route("api/[controller]")]
    public class TeamController : Controller
    {
        [HttpGet("[action]")]
        public List<string> GetTeamFilters()
        {
            return new List<string> {"Brighton", "Arsenal", "Norwich"};
        }
    }
}
