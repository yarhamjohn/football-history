using System.Collections.Generic;
using Backend.Domain.Models;
using Backend.Models.LeagueSeason;

namespace Backend.Builders
{
    public interface IResultMatrixBuilder
    {
        ResultMatrix Build(List<MatchDetailModel> matchDetails);
    }
}
