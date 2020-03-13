using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Controllers;
using FootballHistoryTest.Api.Repositories.League;
using FootballHistoryTest.Api.Repositories.Match;
using FootballHistoryTest.Api.Repositories.PointDeductions;

namespace FootballHistoryTest.Api.Calculators
{
    public static class LeagueTableCalculator
    {
        public static List<LeagueTableRow> GetLeagueTable(List<MatchModel> leagueMatches, List<MatchModel> playOffMatches, LeagueModel leagueModel, List<PointsDeductionModel> pointsDeductions)
        {
            var leagueTable = GetTable(leagueMatches, leagueModel, pointsDeductions);
            var sortedLeagueTable = SortTable(leagueTable, leagueModel);
            return AddStatuses(sortedLeagueTable, playOffMatches, leagueModel);
        }

        private static List<LeagueTableRow> GetTable(List<MatchModel> leagueMatches, LeagueModel leagueModel, List<PointsDeductionModel> pointDeductions)
        {
            var leagueTable = new List<LeagueTableRow>();

            var teams = leagueMatches.Select(m => m.HomeTeam).Distinct();
            foreach (var team in teams)
            {
                var teamMatches = leagueMatches.Where(m => MatchCalculator.MatchInvolvesTeam(m, team)).ToList();
                var numWins = GetNumWins(teamMatches, team);
                var numDefeats = GetNumDefeats(teamMatches, team);
                var numDraws = GetNumDraws(teamMatches, team);

                var homeTeamMatches = leagueMatches.Where(m => m.HomeTeam == team).ToList();
                var awayTeamMatches = leagueMatches.Where(m => m.AwayTeam == team).ToList();
                var goalsFor = homeTeamMatches.Sum(m => m.HomeGoals) + awayTeamMatches.Sum(m => m.AwayGoals);
                var goalsAgainst = homeTeamMatches.Sum(m => m.AwayGoals) + awayTeamMatches.Sum(m => m.HomeGoals);
                var pointsDeductionModel = pointDeductions.SingleOrDefault(p => p.Team == team);
                var pointsDeducted = pointsDeductionModel?.PointsDeducted ?? 0;

                leagueTable.Add(new LeagueTableRow
                {
                    Team = team,
                    Played = teamMatches.Count,
                    Won = numWins,
                    Lost = numDefeats,
                    Drawn = numDraws,
                    GoalsFor = goalsFor,
                    GoalsAgainst = goalsAgainst,
                    GoalDifference = goalsFor - goalsAgainst,
                    Points = numWins * leagueModel.PointsForWin + numDraws - pointsDeducted,
                    PointsDeducted = pointsDeducted,
                    PointsDeductionReason = pointsDeductionModel?.Reason
                });
            }

            return leagueTable;
        }

        private static List<LeagueTableRow> AddStatuses(List<LeagueTableRow> leagueTable, List<MatchModel> playOffMatches, LeagueModel leagueModel)
        {
            var playOffWinner = GetPlayOffWinner(playOffMatches);

            leagueTable.ForEach(r =>
            {
                if (r.Position == 1)
                {
                    r.Status = "Champions";
                }

                if (r.Position > leagueTable.Count - leagueModel.RelegationPlaces)
                {
                    r.Status = "Relegated";
                }

                if (r.Position > 1 && r.Position <= leagueModel.PromotionPlaces)
                {
                    r.Status = "Promoted";
                }

                if (r.Position > leagueModel.PromotionPlaces &&
                    r.Position <= leagueModel.PromotionPlaces + leagueModel.PlayOffPlaces)
                {
                    r.Status = "Play-Offs";
                }

                if (r.Team == playOffWinner)
                {
                    r.Status = "Play-Off Winner";
                }
            });

            return leagueTable;
        }

        private static List<LeagueTableRow> SortTable(List<LeagueTableRow> leagueTable, LeagueModel leagueModel)
        {
            List<LeagueTableRow> sortedLeagueTable;
            if (PremierLeague_Or_FootballLeagueFrom1999(leagueModel))
            {
                sortedLeagueTable = leagueTable
                    .OrderByDescending(t => t.Points)
                    .ThenByDescending(t => t.GoalDifference) // Goal ratio was used prior to 1976-77
                    .ThenByDescending(t => t.GoalsFor)
                    // head to head
                    .ThenBy(t =>
                        t.Team).ToList(); // unless it affects a promotion/relegation spot at the end of the season in which case a play-off occurs (this has never happened)
            }
            else
            {
                sortedLeagueTable = leagueTable
                    .OrderByDescending(t => t.Points)
                    .ThenByDescending(t => t.GoalsFor)
                    .ThenByDescending(t => t.GoalDifference) // Goal ratio was used prior to 1976-77
                    // head to head
                    .ThenBy(t =>
                        t.Team)
                    .ToList(); // unless it affects a promotion/relegation spot at the end of the season in which case a play-off occurs (this has never happened)
            }

            for (var i = 0; i < sortedLeagueTable.Count; i++)
            {
                sortedLeagueTable[i].Position = i + 1;
            }

            return sortedLeagueTable;
        }

        private static bool PremierLeague_Or_FootballLeagueFrom1999(LeagueModel leagueModel)
        {
            return leagueModel.StartYear >= 1999 || leagueModel.Name == "Premier League";
        }

        private static int GetNumDraws(List<MatchModel> teamMatches, string team)
        {
            return teamMatches.Count(m => MatchCalculator.TeamDrewMatch(m, team));
        }

        private static int GetNumDefeats(List<MatchModel> matches, string team)
        {
            return matches.Count(m => MatchCalculator.TeamLostMatch(m, team));
        }

        private static int GetNumWins(List<MatchModel> matches, string team)
        {
            return matches.Count(m => MatchCalculator.TeamWonMatch(m, team));
        }

        private static string? GetPlayOffWinner(List<MatchModel> playOffMatches)
        {
            var playOffFinal = playOffMatches.SingleOrDefault(m => m.Round == "Final");
            return playOffFinal == null ? null : MatchCalculator.GetMatchWinner(playOffFinal);
        }
    }
}
