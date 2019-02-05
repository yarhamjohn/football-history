using FootballHistory.Api.Models.DatabaseModels;
using System.Collections.Generic;

namespace FootballHistory.Api.Repositories.ResultMatrixRepository
{
    public interface IResultMatrixRepository
    {
        List<MatchDetailModel> GetLeagueMatches(int tier, string season);
    }
}
