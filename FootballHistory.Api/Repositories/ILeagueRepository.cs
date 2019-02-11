using System.Collections.Generic;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Repositories
{
    public interface ILeagueRepository
    {
        LeagueDetailModel GetLeagueInfo(int tier, string season);
    }
}
