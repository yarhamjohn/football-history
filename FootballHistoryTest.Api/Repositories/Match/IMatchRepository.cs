using System.Collections.Generic;
using FootballHistoryTest.Api.Controllers;

namespace FootballHistoryTest.Api.Repositories.Match
{
    public interface IMatchRepository
    {
        List<MatchModel> GetLeagueMatchModels(List<int> seasonStartYears, List<int> tiers);
        List<MatchModel> GetLeagueMatchModels(List<int> seasonStartYears, List<int> tiers, List<string> teams);
        List<MatchModel> GetPlayOffMatchModels(List<int> seasonStartYears, List<int> tiers);
        List<MatchModel> GetLeagueHeadToHeadMatchModels(List<int> seasonStartYears, List<int> tiers, string teamOne, string teamTwo);
    }
}
