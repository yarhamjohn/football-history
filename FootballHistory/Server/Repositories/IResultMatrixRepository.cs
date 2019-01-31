using System.Collections.Generic;
using FootballHistory.Server.Domain.Models;

namespace FootballHistory.Server.Repositories
{
    public interface IResultMatrixRepository
    {
        List<MatchDetailModel> GetLeagueMatches(int tier, string season);
    }
}
