using System.Collections.Generic;

namespace football_history.Server.Repositories
{
    public interface IFootballHistoryRepository
    {
        FilterOptions GetFilterOptions();
        LeagueSeason GetLeagueSeason(int tier, string season);
    }
}