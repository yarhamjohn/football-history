using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Builders;
using football.history.api.Repositories.League;
using football.history.api.Repositories.Match;
using football.history.api.Repositories.PointDeductions;

namespace football.history.api.Calculators
{
    public static class LeagueTableCalculator
    {
        public static List<LeagueTableRow> GetFullLeagueTable(
            List<MatchModel> leagueMatches,
            List<MatchModel> playOffMatches,
            LeagueModel leagueModel,
            List<PointsDeductionModel> pointsDeductions)
        {
            var leagueTable = GetTable(leagueMatches, leagueModel, pointsDeductions);
            var sortedLeagueTable = LeagueTableSorter.SortTable(leagueTable, leagueModel);
            return AddStatuses(sortedLeagueTable, playOffMatches, leagueModel);
        }

        public static List<LeagueTableRow> GetPartialLeagueTable(
            List<MatchModel> leagueMatches,
            LeagueModel leagueModel,
            List<PointsDeductionModel> pointsDeductions,
            DateTime date)
        {
            var matchesToDate = leagueMatches.Where(m => m.Date < date).ToList();
            var leagueTable = GetTable(matchesToDate, leagueModel, pointsDeductions);

            var allTeamsInLeague = GetTeamsInvolvedInMatches(leagueMatches);
            var expandedLeagueTable = AddMissingTeams(leagueTable, allTeamsInLeague);

            return LeagueTableSorter.SortTable(expandedLeagueTable, leagueModel);
        }

        private static List<string> GetTeamsInvolvedInMatches(List<MatchModel> leagueMatches)
        {
            return leagueMatches.SelectMany(
                    m => new[]
                    {
                        m.HomeTeam,
                        m.AwayTeam
                    })
                .Distinct()
                .ToList();
        }

        private static List<LeagueTableRow> AddStatuses(
            IEnumerable<LeagueTableRow> table,
            IReadOnlyCollection<MatchModel> playOffMatches,
            LeagueModel leagueModel)
        {
            return table.Select(
                    row =>
                        {
                            row.Status = StatusCalculator.AddStatuses(
                                row,
                                playOffMatches,
                                leagueModel);
                            return row;
                        })
                .ToList();
        }

        private static List<LeagueTableRow> GetTable(
            List<MatchModel> matches,
            LeagueModel leagueModel,
            IReadOnlyCollection<PointsDeductionModel> pointDeductions)
        {
            var teams = GetTeamsInvolvedInMatches(matches);
            return teams
                .Select(team => CreateRowForTeam(matches, leagueModel, pointDeductions, team))
                .ToList();
        }

        private static LeagueTableRow CreateRowForTeam(
            IEnumerable<MatchModel> matches,
            LeagueModel leagueModel,
            IEnumerable<PointsDeductionModel> pointDeductions,
            string team)
        {
            var allMatches = matches.Where(m => MatchInvolvesTeam(m, team)).ToList();
            var homeMatches = allMatches.Where(m => m.HomeTeam == team).ToList();
            var awayMatches = allMatches.Where(m => m.AwayTeam == team).ToList();

            var pointsDeductionModel = pointDeductions.SingleOrDefault(p => p.Team == team);
            var pointsDeducted = pointsDeductionModel?.PointsDeducted ?? 0;

            var leagueTableRow = new LeagueTableRow
            {
                Team = team,
                Played = allMatches.Count,
                Won = CountWins(allMatches, team),
                Lost = CountDefeats(allMatches, team),
                Drawn = CountDraws(allMatches, team),
                GoalsFor = CalculateGoalsFor(homeMatches, awayMatches),
                GoalsAgainst = CalculateGoalsAgainst(homeMatches, awayMatches),
                GoalDifference =
                    CalculateGoalsFor(homeMatches, awayMatches)
                    - CalculateGoalsAgainst(homeMatches, awayMatches),
                Points = CalculatePoints(leagueModel, team, allMatches, pointsDeducted),
                PointsDeducted = pointsDeducted,
                PointsDeductionReason = pointsDeductionModel?.Reason
            };

            leagueTableRow.PointsPerGame = CalculatePointsPerGame(leagueTableRow);
            return leagueTableRow;
        }

        private static double CalculatePointsPerGame(LeagueTableRow leagueTableRow) =>
            (leagueTableRow.Points - leagueTableRow.PointsDeducted)
            / (double) leagueTableRow.Played;

        private static int CalculateGoalsAgainst(
            IEnumerable<MatchModel> homeMatches,
            IEnumerable<MatchModel> awayMatches)
        {
            return homeMatches.Sum(m => m.AwayGoals) + awayMatches.Sum(m => m.HomeGoals);
        }

        private static int CalculateGoalsFor(
            IEnumerable<MatchModel> homeMatches,
            IEnumerable<MatchModel> awayMatches)
        {
            return homeMatches.Sum(m => m.HomeGoals) + awayMatches.Sum(m => m.AwayGoals);
        }

        private static int CalculatePoints(
            LeagueModel leagueModel,
            string team,
            List<MatchModel> matches,
            int pointsDeducted) =>
            CountWins(matches, team) * leagueModel.PointsForWin
            + CountDraws(matches, team)
            - pointsDeducted;

        private static List<LeagueTableRow> AddMissingTeams(
            List<LeagueTableRow> leagueTable,
            List<string> teams)
        {
            var existingTeams = leagueTable.Select(r => r.Team);
            var missingTeams = teams.Except(existingTeams);

            leagueTable.AddRange(missingTeams.Select(team => new LeagueTableRow { Team = team }));
            return leagueTable;
        }

        private static int CountDraws(List<MatchModel> teamMatches, string team)
        {
            return teamMatches.Count(m => TeamDrewMatch(m, team));
        }

        private static int CountDefeats(List<MatchModel> matches, string team)
        {
            return matches.Count(m => TeamLostMatch(m, team));
        }

        private static int CountWins(List<MatchModel> matches, string team)
        {
            return matches.Count(m => TeamWonMatch(m, team));
        }

        private static bool MatchInvolvesTeam(MatchModel match, string team) =>
            match.HomeTeam == team || match.AwayTeam == team;

        private static bool TeamWonMatch(MatchModel match, string team) =>
            match.HomeTeam == team && HomeTeamWon(match)
            || match.AwayTeam == team && AwayTeamWon(match);

        private static bool TeamLostMatch(MatchModel match, string team) =>
            match.HomeTeam == team && AwayTeamWon(match)
            || match.AwayTeam == team && HomeTeamWon(match);

        private static bool TeamDrewMatch(MatchModel match, string team) =>
            !TeamWonMatch(match, team) && !TeamLostMatch(match, team);

        private static bool HomeTeamWon(MatchModel match) =>
            match.HomeGoals > match.AwayGoals
            || match.HomeGoalsExtraTime > match.AwayGoalsExtraTime
            || match.HomePenaltiesScored > match.AwayPenaltiesScored;

        private static bool AwayTeamWon(MatchModel match) =>
            match.HomeGoals < match.AwayGoals
            || match.HomeGoalsExtraTime < match.AwayGoalsExtraTime
            || match.HomePenaltiesScored < match.AwayPenaltiesScored;
    }
}
