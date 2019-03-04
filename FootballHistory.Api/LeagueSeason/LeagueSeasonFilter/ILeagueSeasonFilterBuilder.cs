using System.Collections.Generic;
using FootballHistory.Api.Repositories.DivisionRepository;

namespace FootballHistory.Api.LeagueSeason.LeagueSeasonFilter
{
    public interface ILeagueSeasonFilterBuilder
    {
        LeagueSeasonFilter Build(List<DivisionModel> divisions);
    }
}