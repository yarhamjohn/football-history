using FootballHistory.Api.Models.DatabaseModels;
using System.Collections.Generic;

namespace FootballHistory.Api.Repositories.DivisionRepository
{
    public interface IDivisionRepository
    {
        List<DivisionModel> GetDivisions();
    }
}
