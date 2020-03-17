using System.Collections.Generic;

namespace FootballHistoryTest.Api.Repositories.League
{
    public interface ILeagueRepository
    {
        LeagueModel GetLeagueModel(int seasonStartYear, int tier);
        List<LeagueModel> GetLeagueModels(int seasonStartYear, List<int> tiers);
    }
}
