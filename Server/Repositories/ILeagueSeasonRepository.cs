using System.Collections.Generic;
using static football_history.Server.Repositories.LeagueSeasonRepository;

namespace football_history.Server.Repositories
{
    public interface ILeagueSeasonRepository
    {
        FilterOptions GetFilterOptions();
        LeagueSeason GetLeagueSeason(int tier, string season);

        LeagueRowDrillDown GetDrillDown(int tier, string season, string team);
    }
}