using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders
{
    public class LeagueTableBuilder : ILeagueTableBuilder
    {
        private readonly ILeagueTableCalculatorFactory _leagueTableCalculatorFactory;

        public LeagueTableBuilder(ILeagueTableCalculatorFactory leagueTableCalculatorFactory)
        {
            _leagueTableCalculatorFactory = leagueTableCalculatorFactory;
        }
        
        public LeagueTab Build(List<MatchDetailModel> leagueMatches, List<PointDeductionModel> pointDeductions)
        {
            if (LeagueMatchesAreInvalid(leagueMatches))
            {
                throw new Exception("An invalid set of league matches were provided.");
            }

            var leagueTable = new LeagueTab();
            var teams = GetTeams(leagueMatches);
            foreach (var team in teams)
            {
                var calculator = _leagueTableCalculatorFactory.Create(leagueMatches, pointDeductions, team);

                leagueTable.Rows.Add(
                    new LeagueTableRow
                    {
                        Team = team,
                        Played = calculator.CountGamesPlayed(),
                        Won = calculator.CountWins(),
                        Lost = calculator.CountDefeats(),
                        Drawn = calculator.CountDraws(),
                        GoalsFor = calculator.CountGoalsFor(),
                        GoalsAgainst = calculator.CountGoalsAgainst(),
                        GoalDifference = calculator.CalculateGoalDifference(),
                        Points = calculator.CalculatePoints(),
                        PointsDeducted = calculator.CalculatePointsDeducted(),
                        PointsDeductionReason = calculator.GetPointDeductionReasons()
                        //TODO: Add Position and Status
                    }
                );
            }

            return leagueTable;
        }
        
        private bool LeagueMatchesAreInvalid(List<MatchDetailModel> leagueMatches)
        {
            var opponentPairs = leagueMatches.Select(g => (g.HomeTeam, g.AwayTeam)).ToList();
            var sameTeams = opponentPairs.Where(p => p.Item1 == p.Item2).ToList();

            return opponentPairs.Distinct().Count() != leagueMatches.Count
                   || sameTeams.Count > 0;
        }

        private List<string> GetTeams(List<MatchDetailModel> leagueMatches)
        {
            var homeTeams = leagueMatches.Select(m => m.HomeTeam).ToList();
            var awayTeams = leagueMatches.Select(m => m.AwayTeam).ToList();

            return homeTeams.Union(awayTeams).ToList();
        }
    }
}
