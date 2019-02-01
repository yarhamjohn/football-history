using System.Collections.Generic;
using FootballHistory.Api.Domain.Models;

namespace FootballHistory.Api.Repositories
{
    public interface IDivisionRepository
    {
        List<DivisionModel> GetDivisionModels();
    }
}
