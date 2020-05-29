using System.Collections.Generic;

namespace football.history.api.Repositories.Tier
{
    public interface ITierRepository
    {
        List<TierModel> GetTierModels(List<int> seasonStartYears, string team);
        int? GetTierForTeamInYear(int seasonStartYear, string team);
    }
}
