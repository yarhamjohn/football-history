using FootballHistory.Api.Controllers;
using FootballHistory.Api.Domain;

namespace FootballHistory.Api.Repositories.TierRepository
{
    public class SeasonTierFilter
    {
        public Tier Tier { get; set; }
        public int SeasonStartYear { get; set; }
        public int SeasonEndYear => SeasonStartYear + 1;
        public string Season => $"{SeasonStartYear} - {SeasonEndYear}";
    }
}