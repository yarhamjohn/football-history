using System.Collections.Generic;

namespace FootballHistory.Api.Repositories.TierRepository
{
    public interface ITierRepository
    {
        List<(int, string)> GetTier(string team);
    }
}
