using System.Collections.Generic;
using System.Linq;
using football.history.api.Repositories.Competition;
using football.history.api.Repositories.Match;
using football.history.api.Repositories.PointDeduction;
using football.history.api.Repositories.Team;

namespace football.history.api.Builders
{
    public interface IRowBuilder
    {
        LeagueTableRowDto Build(
            CompetitionModel competition,
            TeamModel team,
            List<MatchModel> matches,
            List<PointDeductionModel> pointDeductions);
    }

    public class RowBuilder : IRowBuilder
    {
        public LeagueTableRowDto Build(
            CompetitionModel competition,
            TeamModel team,
            List<MatchModel> matches,
            List<PointDeductionModel> pointDeductions)
        {
            var teamMatches = matches.Where(m => MatchInvolvesTeam(m, team)).ToList();
            var teamHomeMatches = teamMatches.Where(m => m.HomeTeamId == team.Id).ToList();
            var teamAwayMatches = teamMatches.Where(m => m.AwayTeamId == team.Id).ToList();

            var pointsDeductionModel = pointDeductions.SingleOrDefault(p => p.TeamId == team.Id);
            var pointsDeducted = pointsDeductionModel?.PointsDeducted ?? 0;

            var goalsFor = CalculateGoalsFor(teamHomeMatches, teamAwayMatches);
            var goalsAgainst = CalculateGoalsAgainst(teamHomeMatches, teamAwayMatches);
            var leagueTableRow = new LeagueTableRowDto
            {
                TeamId = team.Id,
                Team = team.Name,
                Played = teamMatches.Count,
                Won = CountWins(teamMatches, team),
                Lost = CountDefeats(teamMatches, team),
                Drawn = CountDraws(teamMatches, team),
                GoalsFor = goalsFor,
                GoalsAgainst = goalsAgainst,
                GoalDifference = goalsFor - goalsAgainst,
                GoalAverage = goalsAgainst == 0 ? null : goalsFor / (double) goalsAgainst,
                Points = CalculatePoints(competition.PointsForWin, team, teamMatches, pointsDeducted),
                PointsDeducted = pointsDeducted,
                PointsDeductionReason = pointsDeductionModel?.Reason
            };

            leagueTableRow.PointsPerGame = CalculatePointsPerGame(leagueTableRow);
            return leagueTableRow;
        }

        private static double? CalculatePointsPerGame(LeagueTableRowDto leagueTableRowDto) =>
            leagueTableRowDto.Played == 0 ? null : leagueTableRowDto.Points / (double) leagueTableRowDto.Played;

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
            int pointsForWin,
            TeamModel team,
            List<MatchModel> matches,
            int pointsDeducted) =>
            CountWins(matches, team) * pointsForWin
            + CountDraws(matches, team)
            - pointsDeducted;

        private static int CountDraws(List<MatchModel> teamMatches, TeamModel team)
        {
            return teamMatches.Count(m => TeamDrewMatch(m, team));
        }

        private static int CountDefeats(List<MatchModel> matches, TeamModel team)
        {
            return matches.Count(m => TeamLostMatch(m, team));
        }

        private static int CountWins(List<MatchModel> matches, TeamModel team)
        {
            return matches.Count(m => TeamWonMatch(m, team));
        }

        private static bool MatchInvolvesTeam(MatchModel match, TeamModel team) =>
            match.HomeTeamId == team.Id || match.AwayTeamId == team.Id;

        private static bool TeamWonMatch(MatchModel match, TeamModel team) =>
            match.HomeTeamId == team.Id && HomeTeamWon(match)
            || match.AwayTeamId == team.Id && AwayTeamWon(match);

        private static bool TeamLostMatch(MatchModel match, TeamModel team) =>
            match.HomeTeamId == team.Id && AwayTeamWon(match)
            || match.AwayTeamId == team.Id && HomeTeamWon(match);

        private static bool TeamDrewMatch(MatchModel match, TeamModel team) =>
            !TeamWonMatch(match, team) && !TeamLostMatch(match, team);

        private static bool HomeTeamWon(MatchModel match) =>
            match.HomeGoals > match.AwayGoals;

        private static bool AwayTeamWon(MatchModel match) =>
            match.HomeGoals < match.AwayGoals;
    }
}