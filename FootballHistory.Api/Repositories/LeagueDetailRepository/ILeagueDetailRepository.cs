using System.Collections.Generic;
using FootballHistory.Api.Controllers;

namespace FootballHistory.Api.Repositories.LeagueDetailRepository
{
    public interface ILeagueDetailRepository
    {
        LeagueDetailModel GetLeagueInfo(SeasonTierFilter filter);
        List<LeagueDetailModel> GetLeagueInfos(params SeasonTierFilter[] filters);
    }
}
