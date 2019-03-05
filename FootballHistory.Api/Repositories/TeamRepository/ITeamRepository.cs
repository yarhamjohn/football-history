using System.Collections.Generic;

namespace FootballHistory.Api.Repositories.TeamRepository
{
    public interface ITeamRepository
    {
        List<string> GetAllTeams();
    }
}