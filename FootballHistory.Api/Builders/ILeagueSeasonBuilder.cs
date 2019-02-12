using System.Collections.Generic;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders
{
    public interface ILeagueSeasonBuilder
    {
        List<LeagueTableRow> Build(List<MatchDetailModel> leagueMatchDetails, LeagueDetailModel leagueDetail,
            List<PointDeductionModel> pointDeductions, List<MatchDetailModel> playOffMatches);
    }
}
