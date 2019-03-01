using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders
{
    public class LeagueTableBuilder : ILeagueTableBuilder
    {
        public LeagueTab Build(List<MatchDetailModel> leagueMatches, List<PointDeductionModel> pointDeductions)
        {
            var leagueTable = new LeagueTab();

            var teams = GetTeams(leagueMatches);
            foreach (var team in teams)
            {
                var (homeGames, awayGames) = FilterLeagueMatches(leagueMatches, team);
                if (LeagueMatchesAreInvalid(homeGames, awayGames))
                {
                    throw new Exception("An invalid set of league matches were provided.");
                }

                var (pointsDeducted, pointDeductionReasons) = CalculatePointDeductions(pointDeductions, team);
                leagueTable.Rows.Add(
                    new LeagueTableRow
                    {
                        Team = team,
                        Played = LeagueTableCalculator.CountGamesPlayed(homeGames, awayGames),
                        Won = LeagueTableCalculator.CountWins(homeGames, awayGames),
                        Lost = LeagueTableCalculator.CountDefeats(homeGames, awayGames),
                        Drawn = LeagueTableCalculator.CountDraws(homeGames, awayGames),
                        GoalsFor = LeagueTableCalculator.CountGoalsFor(homeGames, awayGames),
                        GoalsAgainst = LeagueTableCalculator.CountGoalsAgainst(homeGames, awayGames),
                        GoalDifference = LeagueTableCalculator.CalculateGoalDifference(homeGames, awayGames),
                        Points = LeagueTableCalculator.CalculatePoints(homeGames, awayGames) - pointsDeducted,
                        PointsDeducted = pointsDeducted,
                        PointsDeductionReason = pointDeductionReasons
                    }
                );
            }

            return leagueTable;
        }

        private (int PointsDeducted, string PointsDeductionReason) CalculatePointDeductions(List<PointDeductionModel> pointDeductions, string team)
        {
            var pointsDeducted = pointDeductions.Where(d => d.Team == team).Sum(d => d.PointsDeducted);
            var reasons = string.Join(", ", pointDeductions.Where(d => d.Team == team).Select(d => d.Reason));
            return (pointsDeducted, reasons);
        }

        private (List<MatchDetailModel> HomeGames, List<MatchDetailModel> AwayGames) FilterLeagueMatches(List<MatchDetailModel> leagueMatches, string team)
        {
            var homeGames = leagueMatches.Where(m => m.HomeTeam == team).ToList();
            var awayGames = leagueMatches.Where(m => m.AwayTeam == team).ToList();
            return (HomeGames: homeGames, AwayGames: awayGames);
        }
        
        private List<string> GetTeams(List<MatchDetailModel> leagueMatches)
        {
            var homeTeams = leagueMatches.Select(m => m.HomeTeam).ToList();
            var awayTeams = leagueMatches.Select(m => m.AwayTeam).ToList();

            return homeTeams.Union(awayTeams).ToList();
        }
        
        private bool LeagueMatchesAreInvalid(List<MatchDetailModel> homeGames, List<MatchDetailModel> awayGames)
        {
            var opponentPairsOne = homeGames.Select(g => (g.HomeTeam, g.AwayTeam)).ToList();
            var opponentPairsTwo = awayGames.Select(g => (g.HomeTeam, g.AwayTeam)).ToList();
                
            var sameTeamsOne = opponentPairsOne.Where(p => p.Item1 == p.Item2).ToList();
            var sameTeamsTwo = opponentPairsTwo.Where(p => p.Item1 == p.Item2).ToList();
                
            return opponentPairsOne.Distinct().Count() != homeGames.Count
                   || opponentPairsTwo.Distinct().Count() != awayGames.Count
                   || sameTeamsOne.Count > 0
                   || sameTeamsTwo.Count > 0;
        }
    }
}
