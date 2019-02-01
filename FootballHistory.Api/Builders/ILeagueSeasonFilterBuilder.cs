using System.Collections.Generic;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Domain.Models;

namespace FootballHistory.Api.Builders
{
    public interface ILeagueSeasonFilterBuilder
    {
        LeagueSeasonFilter Build(List<DivisionModel> divisionModels);
    }
}