using System.Collections.Generic;
using FootballHistory.Api.Models.Controller;

namespace FootballHistory.Api.Repositories
{
    public interface ILeagueFormRepository
    {
        List<MatchResultOld> GetLeagueForm(int tier, string season, string team);
    }
}
