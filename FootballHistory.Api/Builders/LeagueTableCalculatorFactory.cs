using System.Collections.Generic;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders
{
    public class LeagueTableCalculatorFactory : ILeagueTableCalculatorFactory
    {
        public ILeagueTableCalculator Create(List<MatchDetailModel> leagueMatches, List<PointDeductionModel> pointDeductions, string team)
        {
            return new LeagueTableCalculator(leagueMatches, pointDeductions, team);
        }
    }
}