using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Domain;
using FootballHistoryTest.Api.Repositories.Team;
using Microsoft.EntityFrameworkCore;

namespace FootballHistoryTest.Api.Builders
{
    public interface ITeamBuilder
    {
        List<Team> GetAllTeams();
        List<Team> GetTeamsInLeague(int seasonStartYear, int tier);
    }
    
    public class TeamBuilder : ITeamBuilder
    {
        private readonly DatabaseContext _context;
        private readonly ITeamRepository _teamRepository;

        public TeamBuilder(DatabaseContext context, ITeamRepository teamRepository)
        {
            _context = context;
            _teamRepository = teamRepository;
        }
        
        public List<Team> GetAllTeams()
        {
            using var conn = _context.Database.GetDbConnection();

            return _teamRepository.GetTeamModels(conn)
                .Select(t => new Team {Name = t.Name, Abbreviation = t.Abbreviation})
                .ToList();
        }
        
        public List<Team> GetTeamsInLeague(int seasonStartYear, int tier)
        {
            using var conn = _context.Database.GetDbConnection();

            return _teamRepository.GetTeamModels(conn, seasonStartYear, tier)
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