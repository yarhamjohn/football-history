using System.Collections.Generic;
using FootballHistory.Api.Controllers;

namespace FootballHistory.Api.Repositories.MatchDetailRepository
{
    public interface IPlayOffMatchesRepository
    {
        List<MatchDetailModel> GetPlayOffMatches(SeasonTierFilter filter);
        List<MatchDetailModel> GetPlayOffMatches(List<SeasonTierFilter> filters);
    }
}
