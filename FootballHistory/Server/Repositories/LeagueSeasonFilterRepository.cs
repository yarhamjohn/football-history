using System.Collections.Generic;
using System.Linq;
using FootballHistory.Server.Models;

namespace FootballHistory.Server.Repositories
{
    public class LeagueSeasonFilterRepository : ILeagueSeasonFilterRepository
    {
        private IDivisionRepository DivisionRepository { get; }
        private ILeagueSeasonFilterBuilder LeagueSeasonFilterBuilder { get; }

        public LeagueSeasonFilterRepository(IDivisionRepository divisionRepository, ILeagueSeasonFilterBuilder leagueSeasonFilterBuilder)
        {
            DivisionRepository = divisionRepository;
            LeagueSeasonFilterBuilder = leagueSeasonFilterBuilder;
        }

        public LeagueSeasonFilter GetLeagueSeasonFilters()
        {
            var divisionModels = DivisionRepository.GetDivisionModels();
            return LeagueSeasonFilterBuilder.Build(divisionModels);
        }
    }
}
