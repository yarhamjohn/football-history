using System.Collections.Generic;
using FootballHistory.Server.Models;

namespace FootballHistory.Server.Repositories
{
    public interface IDivisionRepository
    {
        List<DivisionModel> GetDivisionModels();
    }
}
