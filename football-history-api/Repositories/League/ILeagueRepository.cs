using System;
using System.Collections.Generic;

namespace football.history.api.Repositories.League
{
    public interface ILeagueRepository
    {
        LeagueModel GetLeagueModel(int seasonStartYear, int tier);
        LeagueModel GetLeagueModel(DateTime date, int tier);

        List<LeagueModel> GetLeagueModels(List<int> seasonStartYears, List<int> tiers);
    }
}
