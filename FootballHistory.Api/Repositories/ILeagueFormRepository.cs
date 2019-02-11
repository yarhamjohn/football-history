using System.Collections.Generic;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Repositories
{
    public interface ILeagueFormRepository
    {
        List<MatchModel> GetLeagueForm(int tier, string season, string team);
    }
}
