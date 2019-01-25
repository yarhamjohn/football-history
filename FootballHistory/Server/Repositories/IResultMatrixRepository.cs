using System.Collections.Generic;

namespace FootballHistory.Server.Repositories
{
    public interface IResultMatrixRepository
    {
        List<MatchDetail> GetLeagueMatches(int tier, string season);
    }
}
