using System.Collections.Generic;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using FootballHistory.Api.Repositories.PointDeductionRepository;

namespace FootballHistory.Api.LeagueSeason.LeagueTable
{
    public interface ILeagueTableCalculatorFactory
    {
        ILeagueTableCalculator Create(List<MatchDetailModel> leagueMatches, List<PointDeductionModel> pointDeductions, string team);
    }
}