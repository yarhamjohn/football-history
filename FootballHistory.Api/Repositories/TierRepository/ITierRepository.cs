using System.Collections.Generic;
using FootballHistory.Api.Controllers;

namespace FootballHistory.Api.Repositories.TierRepository
{
    public interface ITierRepository
    {
        SeasonTierFilter[] GetSeasonTierFilters(string team, int seasonStartYear, int seasonEndYear);
    }
}
