using System.Collections.Generic;

namespace FootballHistoryTest.Api.Repositories.Season
{
    public interface ISeasonRepository
    {
        List<SeasonDatesModel> GetSeasonDateModels();
    }
}
