namespace FootballHistory.Api.Repositories.TierRepository
{
    public interface ITierRepository
    {
        int GetTier(string season, string team);
    }
}
