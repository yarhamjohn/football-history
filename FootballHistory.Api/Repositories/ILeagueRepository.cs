using System.Collections.Generic;
using FootballHistory.Api.Models.Controller;

namespace FootballHistory.Api.Repositories
{
    public interface ILeagueRepository
    {
        LeagueDetail GetLeagueInfo(int tier, string season);
    }
}
