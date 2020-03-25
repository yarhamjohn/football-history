using System.Collections.Generic;

namespace FootballHistoryTest.Api.Repositories.Tier
{
    public interface ITierRepository
    {
        List<TierModel> GetTierModels(List<int> seasonStartYears, string team);
    }
}