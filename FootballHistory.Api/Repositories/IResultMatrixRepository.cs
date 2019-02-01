using System.Collections.Generic;
using FootballHistory.Api.Domain.Models;

namespace FootballHistory.Api.Repositories
{
    public interface IResultMatrixRepository
    {
        List<MatchDetailModel> GetLeagueMatches(int tier, string season);
    }
}
