using System.Collections.Generic;
using Backend.Builders.Models;
using Backend.Domain.Models;

namespace Backend.Builders
{
    public interface ILeagueSeasonFilterBuilder
    {
        LeagueSeasonFilter Build(List<DivisionModel> divisionModels);
    }
}