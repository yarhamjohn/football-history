using System.Collections.Generic;
using System.Data.Common;

namespace FootballHistoryTest.Api.Repositories.Season
{
    public interface ISeasonRepository
    {
        List<SeasonModel> GetSeasonModels();
    }
}
