using FootballHistory.Api.Models.ControllerModels;
using FootballHistory.Api.Models.DatabaseModels;
using System.Collections.Generic;

namespace FootballHistory.Api.Builders.ResultMatrixBuilder
{
    public interface IResultMatrixBuilder
    {
        ResultMatrix Build(List<MatchDetailModel> matchDetails);
    }
}
