using System.Collections.Generic;
using FootballHistory.Server.Domain.Models;

namespace FootballHistory.Server.Repositories
{
    public interface IDivisionRepository
    {
        List<DivisionModel> GetDivisionModels();
    }
}
