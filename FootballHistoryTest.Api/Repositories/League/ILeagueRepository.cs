using System.Collections.Generic;
using System.Data.Common;

namespace FootballHistoryTest.Api.Repositories.League
{
    public interface ILeagueRepository
    {
        LeagueModel GetLeagueModel(int seasonStartYear, int tier);
        List<LeagueModel> GetLeagueModels(List<int> seasonStartYears, List<int> tiers);
    }
}
