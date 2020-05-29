using System.Collections.Generic;

namespace football.history.api.Repositories.Season
{
    public interface ISeasonRepository
    {
        List<SeasonModel> GetSeasonModels();
    }
}
