using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using FootballHistory.Api.Repositories.PointDeductionRepository;

namespace FootballHistory.Api.LeagueSeason.LeagueTable
{
    public class LeagueTableBuilder : ILeagueTableBuilder
    {
        private readonly ILeagueTableCalculatorFactory _leagueTableCalculatorFactory;
        private readonly ILeagueTablePositionCalculator _leagueTablePositionCalculator;
        private readonly ILeagueTableStatusCalculator _leagueTableStatusCalculator;

        public LeagueTableBuilder(ILeagueTableCalculatorFactory leagueTableCalculatorFactory, ILeagueTablePositionCalculator leagueTablePositionCalculator, ILeagueTableStatusCalculator leagueTableStatusCalculator)
        {
            _leagueTableCalculatorFactory = leagueTableCalculatorFactory;
            _leagueTablePositionCalculator = leagueTablePositionCalculator;
            _leagueTableStatusCalculator = leagueTableStatusCalculator;
        }
        
        public LeagueTable BuildWithStatuses(List<MatchDetailModel> leagueMatches, List<PointDeductionModel> pointDeductions, LeagueDetailModel leagueDetailModel, List<MatchDetailModel> playOffMatches)
        {
            var leagueTable = BuildBasicTable(leagueMatches, pointDeductions);
            leagueTable = _leagueTablePositionCalculator.AddPositions(leagueTable, leagueDetailModel);
            leagueTable = _leagueTableStatusCalculator.AddStatuses(leagueTable, leagueDetailModel, playOffMatches);
            
            return leagueTable;
        }

        public LeagueTable BuildWithoutStatuses(List<MatchDetailModel> leagueMatches, List<PointDeductionModel> pointDeductions, LeagueDetailModel leagueDetailModel, List<string> missingTeams)
        {
            var leagueTable = BuildBasicTable(leagueMatches, pointDeductions);
            AddMissingTeams(missingTeams, leagueTable);
            leagueTable = _leagueTablePositionCalculator.AddPositions(leagueTable, leagueDetailModel);

            return leagueTable;
        }

        private static void AddMissingTeams(List<string> missingTeams, LeagueTable leagueTable)
        {
            foreach (var team in missingTeams)
            {
                leagueTable.Rows.Add(new LeagueTableRow {Team = team});
            }
        }
        
        private LeagueTable BuildBasicTable(List<MatchDetailModel> leagueMatches, List<PointDeductionModel> pointDeductions)
        {
            if (LeagueMatchesAreInvalid(leagueMatches))
            {
                throw new Exception("An invalid set of league matches were provided.");
            }

            var leagueTable = new LeagueTable();
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
                    }
                );
            }

            return leagueTable;
        }
        
        private static bool LeagueMatchesAreInvalid(List<MatchDetailModel> leagueMatches)
        {
            var opponentPairs = leagueMatches.Select(g => (g.HomeTeam, g.AwayTeam)).ToList();
            var sameTeams = opponentPairs.Where(p => p.Item1 == p.Item2).ToList();

            return opponentPairs.Distinct().Count() != leagueMatches.Count
                   || sameTeams.Count > 0;
        }

        private static List<string> GetTeams(List<MatchDetailModel> leagueMatches)
        {
            var homeTeams = leagueMatches.Select(m => m.HomeTeam).ToList();
            var awayTeams = leagueMatches.Select(m => m.AwayTeam).ToList();

            return homeTeams.Union(awayTeams).ToList();
        }
    }
}
