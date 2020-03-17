using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Controllers;
using FootballHistoryTest.Api.Repositories.League;
using FootballHistoryTest.Api.Repositories.Match;
using FootballHistoryTest.Api.Repositories.PointDeductions;

namespace FootballHistoryTest.Api.Calculators
{
    public static class LeaguePositionCalculator
    {
        public static List<LeaguePosition> GetPositions(List<MatchModel> leagueMatches, LeagueModel leagueModel, List<PointsDeductionModel> pointsDeductions, string team)
        {
            if (!leagueMatches.Any(m => m.HomeTeam == team || m.AwayTeam == team))
            {
                return new List<LeaguePosition>();
            }
            
            var dates = leagueMatches.Select(m => m.Date).Distinct().OrderBy(m => m.Date).ToList();
            var startDate = dates.First();
            var endDate = dates.Last().AddDays(1);
            var leaguePositions = new List<LeaguePosition>();
            
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var leagueTable = LeagueTableCalculator.GetPartialLeagueTable(leagueMatches, leagueModel, pointsDeductions, date);
                leaguePositions.Add(new LeaguePosition {Date = date, Position = leagueTable.Single(r => r.Team == team).Position});                
            }

            return leaguePositions;
        }
    }
}