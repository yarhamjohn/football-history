using System.Collections.Generic;

namespace FootballHistory.Api.Repositories.LeagueDetailRepository
{
    public interface ILeagueDetailRepository
    {
        LeagueDetailModel GetLeagueInfo(int tier, string season);
        List<LeagueDetailModel> GetLeagueInfos(List<(int, string)> seasonTier);
    }
}
