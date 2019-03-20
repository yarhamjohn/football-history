using System.Collections.Generic;
using FootballHistory.Api.Repositories.DivisionRepository;

namespace FootballHistory.Api.LeagueSeason.Filter
{
    public interface ILeagueSeasonFilterBuilder
    {
        LeagueSeasonFilter Build(List<DivisionModel> divisions);
    }
}