using System.Collections.Generic;

namespace football.history.api.Repositories.Team
{
    public interface ITeamRepository
    {
        List<TeamModel> GetTeamModels();

        List<TeamModel> GetTeamModels(int seasonStartYear, int tier);
    }
}
