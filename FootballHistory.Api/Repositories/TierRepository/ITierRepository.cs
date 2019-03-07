using System.Collections.Generic;
using FootballHistory.Api.Controllers;

namespace FootballHistory.Api.Repositories.TierRepository
{
    public interface ITierRepository
    {
        List<SeasonTierFilter> GetTier(string team);
    }
}
