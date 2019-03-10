using System.Collections.Generic;
using FootballHistory.Api.Controllers;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using FootballHistory.Api.Repositories.PointDeductionRepository;

namespace FootballHistory.Api.Team.HistoricalPosition
{
    public interface IHistoricalPositionBuilder
    {
        List<HistoricalPosition> Build(string team, SeasonTierFilter[] filters, List<MatchDetailModel> leagueMatchDetails, List<MatchDetailModel> playOffMatches, List<PointDeductionModel> pointDeductions, List<LeagueDetailModel> leagueDetails);
    }
}