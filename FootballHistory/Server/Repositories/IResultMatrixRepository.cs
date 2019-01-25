using FootballHistory.Server.Models.LeagueSeason;

namespace FootballHistory.Server.Repositories
{
    public interface IResultMatrixRepository
    {
        ResultMatrix GetResultMatrix(int tier, string season);
    }
}
