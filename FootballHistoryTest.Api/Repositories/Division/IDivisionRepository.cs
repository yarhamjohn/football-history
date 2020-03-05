using System.Collections.Generic;

namespace FootballHistoryTest.Api.Repositories.Division
{
    public interface IDivisionRepository
    {
        List<DivisionModel> GetDivisionModels(int? tier = null);
    }
}
