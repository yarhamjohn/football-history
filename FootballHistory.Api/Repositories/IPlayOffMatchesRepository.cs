using System.Collections.Generic;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Repositories
{
    public interface IPlayOffMatchesRepository
    {
        List<MatchDetailModel> GetPlayOffMatches(int tier, string season);
    }
}
