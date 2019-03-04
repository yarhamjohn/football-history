using System.Collections.Generic;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using FootballHistory.Api.Repositories.MatchDetailRepository;

namespace FootballHistory.Api.LeagueSeason.LeagueTable
{
    public interface ILeagueTable
    {
        LeagueTable AddPositionsAndStatuses(LeagueDetailModel leagueDetailModel, List<MatchDetailModel> playOffMatches);
        LeagueTable AddPositions();
    }
}