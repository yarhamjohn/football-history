using System.Collections.Generic;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders
{
    public interface ILeagueTableCalculatorFactory
    {
        ILeagueTableCalculator Create(List<MatchDetailModel> leagueMatches, List<PointDeductionModel> pointDeductions, string team);
    }
}