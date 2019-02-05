using System.Collections.Generic;
using FootballHistory.Api.Models.ControllerModels;
using FootballHistory.Api.Models.DatabaseModels;

namespace FootballHistory.Api.Builders.LeagueSeasonFilterBuilder
{
    public interface ILeagueSeasonFilterBuilder
    {
        LeagueSeasonFilter Build(List<DivisionModel> divisions);
    }
}