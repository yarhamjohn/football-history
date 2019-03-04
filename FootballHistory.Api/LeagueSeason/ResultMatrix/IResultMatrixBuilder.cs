using System.Collections.Generic;
using FootballHistory.Api.Repositories.MatchDetailRepository;

namespace FootballHistory.Api.LeagueSeason.ResultMatrix
{
    public interface IResultMatrixBuilder
    {
        ResultMatrix Build(List<MatchDetailModel> matchDetails);
    }
}
