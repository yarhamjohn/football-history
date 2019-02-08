using System.Collections.Generic;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Repositories
{
    public interface ILeagueMatchesRepository
    {
        List<MatchDetailModel> GetLeagueMatches(int tier, string season);
    }
}
