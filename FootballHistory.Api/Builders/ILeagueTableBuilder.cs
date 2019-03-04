using System.Collections.Generic;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders
{
    public interface ILeagueTableBuilder
    {
        LeagueTable Build(List<MatchDetailModel> leagueMatchDetails, List<PointDeductionModel> pointDeductions, LeagueDetailModel leagueDetailModel, List<MatchDetailModel> playOffMatches);
    }
}
