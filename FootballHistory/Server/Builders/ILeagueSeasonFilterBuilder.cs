using System.Collections.Generic;
using FootballHistory.Server.Builders.Models;
using FootballHistory.Server.Domain.Models;

namespace FootballHistory.Server.Builders
{
    public interface ILeagueSeasonFilterBuilder
    {
        LeagueSeasonFilter Build(List<DivisionModel> divisionModels);
    }
}