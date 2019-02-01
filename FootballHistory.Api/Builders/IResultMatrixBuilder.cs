using System.Collections.Generic;
using FootballHistory.Api.Domain.Models;
using FootballHistory.Api.Models.LeagueSeason;

namespace FootballHistory.Api.Builders
{
    public interface IResultMatrixBuilder
    {
        ResultMatrix Build(List<MatchDetailModel> matchDetails);
    }
}
