using System.Collections.Generic;

namespace FootballHistory.Server.Repositories
{
    public interface IResultMatrixRepository
    {
        List<MatchDetail> GetResultMatrix(int tier, string season);
    }
}
