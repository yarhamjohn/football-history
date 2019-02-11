using System.Collections.Generic;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Repositories
{
    public interface ILeagueFormRepository
    {
        List<MatchDetailModel> GetLeagueMatches(int tier, string season, string team);
    }
}
