using System.Collections.Generic;
using FootballHistoryTest.Api.Controllers;

namespace FootballHistoryTest.Api.Repositories.Match
{
    public interface IMatchRepository
    {
        List<MatchModel> GetLeagueMatchModels(string team);
        List<MatchModel> GetLeagueMatchModels(int seasonStartYear, string team);
        List<MatchModel> GetLeagueMatchModels(int seasonStartYear, int tier);
        List<MatchModel> GetLeagueMatchModels(string teamOne, string teamTwo);
        List<KnockoutMatch> GetPlayOffMatchModels(int seasonStartYear, int tier);
    }
}
