using System.Collections.Generic;
using static football_history.Server.Repositories.FootballHistoryRepository;

namespace football_history.Server.Repositories
{
    public interface IFootballHistoryRepository
    {
        FilterOptions GetFilterOptions();
        LeagueSeason GetLeagueSeason(int tier, string season);

        LeagueRowDrillDown GetDrillDown(int tier, string season, string team);
    }
}