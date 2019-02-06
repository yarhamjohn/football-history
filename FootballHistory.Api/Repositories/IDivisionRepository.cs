using System.Collections.Generic;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Repositories
{
    public interface IDivisionRepository
    {
        List<DivisionModel> GetDivisions();
    }
}
