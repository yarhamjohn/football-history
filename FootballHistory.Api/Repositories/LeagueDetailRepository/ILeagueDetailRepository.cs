namespace FootballHistory.Api.Repositories.LeagueDetailRepository
{
    public interface ILeagueDetailRepository
    {
        LeagueDetailModel GetLeagueInfo(int tier, string season);
    }
}
