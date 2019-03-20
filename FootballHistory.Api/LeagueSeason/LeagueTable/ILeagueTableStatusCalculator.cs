using System.Collections.Generic;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using FootballHistory.Api.Repositories.MatchDetailRepository;

namespace FootballHistory.Api.LeagueSeason.LeagueTable
{
    public interface ILeagueTableStatusCalculator
    {
        LeagueTable AddStatuses(LeagueTable leagueTable, LeagueDetailModel leagueDetailModel, List<MatchDetailModel> playOffMatches);
    }
}