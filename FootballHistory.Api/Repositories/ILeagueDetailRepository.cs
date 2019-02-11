using FootballHistory.Api.Models.Controller;

namespace FootballHistory.Api.Repositories
{
    public interface ILeagueDetailRepository
    {
        LeagueDetailModel GetLeagueInfo(int tier, string season);
    }
}
