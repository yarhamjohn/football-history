using System.Collections.Generic;
using System.Data.Common;

namespace FootballHistoryTest.Api.Repositories.League
{
    public interface ILeagueRepository
    {
        LeagueModel GetLeagueModel(DbConnection conn, int seasonStartYear, int tier);
        List<LeagueModel> GetLeagueModels(DbConnection conn, List<int> seasonStartYears, List<int> tiers);
    }
}
