using System.Collections.Generic;
using FootballHistory.Api.Controllers;
using FootballHistory.Api.Repositories.TierRepository;

namespace FootballHistory.Api.Repositories.MatchDetailRepository
{
    public interface IPlayOffMatchesRepository
    {
        List<MatchDetailModel> GetPlayOffMatches(params SeasonTierFilter[] filter);
    }
}
