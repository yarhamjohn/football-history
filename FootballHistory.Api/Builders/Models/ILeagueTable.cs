using System.Collections.Generic;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders.Models
{
    public interface ILeagueTable
    {
        LeagueTable AddPositionsAndStatuses(LeagueDetailModel leagueDetailModel, List<MatchDetailModel> playOffMatches);
    }
}