using System.Collections.Generic;
using FootballHistory.Api.Controllers;

namespace FootballHistory.Api.Repositories.MatchDetailRepository
{
    public interface ILeagueMatchesRepository
    {
        List<MatchDetailModel> GetLeagueMatches(SeasonTierFilter filter);
        List<MatchDetailModel> GetLeagueMatches(List<SeasonTierFilter> filters);
    }
}
