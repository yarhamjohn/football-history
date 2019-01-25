using System.Collections.Generic;
using FootballHistory.Server.Domain.Models;
using FootballHistory.Server.Models.LeagueSeason;

namespace FootballHistory.Server.Builders
{
    public interface IResultMatrixBuilder
    {
        ResultMatrix Build(List<MatchDetailModel> matchDetails);
    }
}
