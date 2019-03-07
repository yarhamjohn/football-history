using System.Collections.Generic;
using FootballHistory.Api.Controllers;

namespace FootballHistory.Api.Repositories.MatchDetailRepository
{
    public interface IPlayOffMatchesRepository
    {
        List<MatchDetailModel> GetPlayOffMatches(params SeasonTierFilter[] filter);
    }
}
