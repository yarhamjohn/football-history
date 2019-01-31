using System.Collections.Generic;
using Backend.Domain.Models;

namespace Backend.Repositories
{
    public interface IDivisionRepository
    {
        List<DivisionModel> GetDivisionModels();
    }
}
