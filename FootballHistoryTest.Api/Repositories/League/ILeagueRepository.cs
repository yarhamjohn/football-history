namespace FootballHistoryTest.Api.Repositories.League
{
    public interface ILeagueRepository
    {
        LeagueModel GetLeagueModel(int seasonStartYear, int tier);
    }
}
