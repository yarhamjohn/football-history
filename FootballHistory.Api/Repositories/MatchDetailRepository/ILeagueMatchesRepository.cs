using System.Collections.Generic;

namespace FootballHistory.Api.Repositories.MatchDetailRepository
{
    public interface ILeagueMatchesRepository
    {
        List<MatchDetailModel> GetLeagueMatches(int tier, string season);
        List<MatchDetailModel> GetLeagueMatches(int tier, string season, string team);
    }
}
