using System.Collections.Generic;
using System.Data.Common;

namespace FootballHistoryTest.Api.Repositories.Tier
{
    public interface ITierRepository
    {
        List<TierModel> GetTierModels(List<int> seasonStartYears, string team);
        int GetTierForTeamInYear(int seasonStartYear, string team);
    }
}
