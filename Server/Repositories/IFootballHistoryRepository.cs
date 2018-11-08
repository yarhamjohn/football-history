using System.Collections.Generic;

namespace football_history.Server.Repositories
{
    public interface IFootballHistoryRepository
    {
        LeagueTable GetLeagueTable(string competitionName, string season);
        LeagueFilterOptions GetLeagueFilterOptions();
    }
}