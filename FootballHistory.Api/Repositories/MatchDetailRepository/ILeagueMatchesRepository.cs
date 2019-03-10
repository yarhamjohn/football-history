using System.Collections.Generic;
using FootballHistory.Api.Controllers;
using FootballHistory.Api.Repositories.TierRepository;

namespace FootballHistory.Api.Repositories.MatchDetailRepository
{
    public interface ILeagueMatchesRepository
    {
        List<MatchDetailModel> GetLeagueMatches(params SeasonTierFilter[] filter);
    }
}
