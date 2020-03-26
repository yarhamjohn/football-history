using System.Collections.Generic;
using System.Data.Common;

namespace FootballHistoryTest.Api.Repositories.Tier
{
    public interface ITierRepository
    {
        List<TierModel> GetTierModels(DbConnection conn, List<int> seasonStartYears, string team);
    }
}