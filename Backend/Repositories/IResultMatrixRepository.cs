using System.Collections.Generic;
using Backend.Domain.Models;

namespace Backend.Repositories
{
    public interface IResultMatrixRepository
    {
        List<MatchDetailModel> GetLeagueMatches(int tier, string season);
    }
}
