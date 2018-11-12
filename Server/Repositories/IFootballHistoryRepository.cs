using System.Collections.Generic;

namespace football_history.Server.Repositories
{
    public interface IFootballHistoryRepository
    {
        LeagueTable GetLeagueTable(int tier, string season);
        LeagueFilterOptions GetLeagueFilterOptions();
        List<Results> GetMatchResultMatrix(int tier, string season);
    }
}