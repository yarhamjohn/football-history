using System.Collections.Generic;
using System.Data.Common;

namespace FootballHistoryTest.Api.Repositories.Team
{
    public interface ITeamRepository
    {
        List<TeamModel> GetTeamModels(DbConnection conn);
        List<TeamModel> GetTeamModels(DbConnection conn, int seasonStartYear, int tier);
    }
}