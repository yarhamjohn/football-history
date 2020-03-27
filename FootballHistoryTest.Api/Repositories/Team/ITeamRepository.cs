using System.Collections.Generic;
using System.Data.Common;

namespace FootballHistoryTest.Api.Repositories.Team
{
    public interface ITeamRepository
    {
        List<TeamModel> GetTeamModels();
        List<TeamModel> GetTeamModels(int seasonStartYear, int tier);
    }
}
