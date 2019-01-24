using System.Collections.Generic;
using static FootballHistory.Server.Repositories.LeagueSeasonRepository;

namespace FootballHistory.Server.Repositories
{
    public interface IFilterRepository
    {
        DefaultFilter GetDefaultFilter();
    }
}
