using System.Collections.Generic;

namespace FootballHistoryTest.Api.Repositories.Team
{
    public interface ITeamRepository
    {
        List<TeamModel> GetTeamModels();
    }
}