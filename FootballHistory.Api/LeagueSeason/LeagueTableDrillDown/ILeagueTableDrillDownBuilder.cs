using System.Collections.Generic;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using FootballHistory.Api.Repositories.PointDeductionRepository;

namespace FootballHistory.Api.LeagueSeason.LeagueTableDrillDown
{
    public interface ILeagueTableDrillDownBuilder
    {
        LeagueRowDrillDown Build(string team, List<MatchDetailModel> matchDetails, List<PointDeductionModel> pointDeductions);
    }
}
