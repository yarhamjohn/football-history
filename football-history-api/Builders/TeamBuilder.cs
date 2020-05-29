using System.Collections.Generic;
using System.Linq;
using football.history.api.Repositories.Team;

namespace football.history.api.Builders
{
    public interface ITeamBuilder
    {
        List<Team> GetAllTeams();
        List<Team> GetTeamsInLeague(int seasonStartYear, int tier);
    }
    
    public class TeamBuilder : ITeamBuilder
    {
        private readonly ITeamRepository _teamRepository;

        public TeamBuilder(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }
        
        public List<Team> GetAllTeams()
        {
            return _teamRepository.GetTeamModels()
                .Select(t => new Team {Name = t.Name, Abbreviation = t.Abbreviation})
                .ToList();
        }
        
        public List<Team> GetTeamsInLeague(int seasonStartYear, int tier)
        {
            return _teamRepository.GetTeamModels(seasonStartYear, tier)
                .Select(t => new Team {Name = t.Name, Abbreviation = t.Abbreviation})
                .ToList();
        }
    }

    public class Team
    {
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }
}
