using System.Collections.Generic;
using System.Data.Common;

namespace FootballHistoryTest.Api.Repositories.Match
{
    public interface IMatchRepository
    {
        List<MatchModel> GetLeagueMatchModels(DbConnection conn, int seasonStartYear, int tier);
        List<MatchModel> GetLeagueMatchModels(DbConnection conn, List<int> seasonStartYears, List<int> tiers);
        List<MatchModel> GetLeagueMatchModels(DbConnection conn, List<int> seasonStartYears, List<int> tiers, List<string> teams);
        List<MatchModel> GetPlayOffMatchModels(DbConnection conn, int seasonStartYear, int tier);
        List<MatchModel> GetPlayOffMatchModels(DbConnection conn, List<int> seasonStartYears, List<int> tiers);
        List<MatchModel> GetLeagueHeadToHeadMatchModels(DbConnection conn, List<int> seasonStartYears, List<int> tiers, string teamOne, string teamTwo);
    }
}
