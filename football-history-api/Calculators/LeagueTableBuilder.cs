using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Builders;
using football.history.api.Repositories.League;
using football.history.api.Repositories.Match;
using football.history.api.Repositories.PointDeductions;
using football.history.api.Repositories.Tier;

namespace football.history.api.Calculators
{
    public interface ILeagueTableBuilder
    {
        List<LeagueTableRowDto> Build(
            int seasonStartYear,
            LeagueModel leagueModel,
            DateTime date);

        List<LeagueTableRowDto> Build(
            int seasonStartYear,
            LeagueModel leagueModel);
    }

    public class LeagueTableBuilder : ILeagueTableBuilder
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IPointsDeductionRepository _pointDeductionsRepository;

        public LeagueTableBuilder(
            IMatchRepository matchRepository,
            IPointsDeductionRepository pointDeductionsRepository)
        {
            _matchRepository = matchRepository;
            _pointDeductionsRepository = pointDeductionsRepository;

        }

        public List<LeagueTableRowDto> Build(
            int seasonStartYear,
            LeagueModel leagueModel)
        {
            var pointsDeductions =
                _pointDeductionsRepository.GetPointsDeductionModels(seasonStartYear, leagueModel.Tier);
            var playOffMatches = _matchRepository.GetPlayOffMatchModels(seasonStartYear, leagueModel.Tier);
            var relegationPlayOffMatches = _matchRepository.GetPlayOffMatchModels(seasonStartYear, leagueModel.Tier + 1);
            var leagueMatches = _matchRepository.GetLeagueMatchModels(seasonStartYear, leagueModel.Tier);

            return GetFullLeagueTable(
                leagueMatches,
                playOffMatches,
                relegationPlayOffMatches,
                leagueModel,
                pointsDeductions);
        }

        public List<LeagueTableRowDto> Build(
            int seasonStartYear,
            LeagueModel leagueModel,
            DateTime date)
        {
            var pointsDeductions =
                _pointDeductionsRepository.GetPointsDeductionModels(seasonStartYear, leagueModel.Tier);
            var playOffMatches = _matchRepository.GetPlayOffMatchModels(seasonStartYear, leagueModel.Tier);
            var relegationPlayOffMatches = _matchRepository.GetPlayOffMatchModels(seasonStartYear, leagueModel.Tier + 1);
            var leagueMatches = _matchRepository.GetLeagueMatchModels(seasonStartYear, leagueModel.Tier);

            return AllMatchesHaveBeenPlayed(date, playOffMatches, leagueMatches)
                ? GetFullLeagueTable(
                    leagueMatches,
                    playOffMatches,
                    relegationPlayOffMatches,
                    leagueModel,
                    pointsDeductions)
                : GetPartialLeagueTable(
                    leagueMatches,
                    leagueModel,
                    pointsDeductions,
                    date);
        }

        private static bool AllMatchesHaveBeenPlayed(
            DateTime date,
            IEnumerable<MatchModel> playOffMatches,
            IEnumerable<MatchModel> leagueMatches)
        {
            var playOffMatchesAfterDate = playOffMatches.Any(match => match.Date >= date);
            var leagueMatchesAfterDate = leagueMatches.Any(match => match.Date >= date);
            return !playOffMatchesAfterDate && !leagueMatchesAfterDate;
        }

        private List<LeagueTableRowDto> GetFullLeagueTable(
            List<MatchModel> leagueMatches,
            List<MatchModel> playOffMatches,
            List<MatchModel> relegationPlayOffMatches,
            LeagueModel leagueModel,
            List<PointsDeductionModel> pointsDeductions)
        {
            var leagueTable = GetTable(leagueMatches, leagueModel, pointsDeductions);
            var sortedLeagueTable = LeagueTableSorter.SortTable(leagueTable, leagueModel);
            return AddStatuses(
                sortedLeagueTable,
                playOffMatches,
                relegationPlayOffMatches,
                leagueModel);
        }

        private List<LeagueTableRowDto> GetPartialLeagueTable(
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

        private List<string> GetTeamsInvolvedInMatches(List<MatchModel> leagueMatches)
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

        private List<LeagueTableRowDto> AddStatuses(
            IEnumerable<LeagueTableRowDto> table,
            IReadOnlyCollection<MatchModel> playOffMatches,
            IReadOnlyCollection<MatchModel> relegationPlayOffMatches,
            LeagueModel leagueModel)
        {
            return table.Select(
                    row =>
                        {
                            row.Status = StatusCalculator.AddStatuses(
                                row,
                                playOffMatches,
                                relegationPlayOffMatches,
                                leagueModel);
                            return row;
                        })
                .ToList();
        }

        private List<LeagueTableRowDto> GetTable(
            List<MatchModel> matches,
            LeagueModel leagueModel,
            IReadOnlyCollection<PointsDeductionModel> pointDeductions)
        {
            var teams = GetTeamsInvolvedInMatches(matches);
            return teams
                .Select(team => CreateRowForTeam(matches, leagueModel, pointDeductions, team))
                .ToList();
        }

        private LeagueTableRowDto CreateRowForTeam(
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

            var goalsFor = CalculateGoalsFor(homeMatches, awayMatches);
            var goalsAgainst = CalculateGoalsAgainst(homeMatches, awayMatches);
            var leagueTableRow = new LeagueTableRowDto
            {
                Team = team,
                Played = allMatches.Count,
                Won = CountWins(allMatches, team),
                Lost = CountDefeats(allMatches, team),
                Drawn = CountDraws(allMatches, team),
                GoalsFor = goalsFor,
                GoalsAgainst = goalsAgainst,
                GoalDifference = goalsFor - goalsAgainst,
                GoalAverage = goalsFor / (double) goalsAgainst,
                Points = CalculatePoints(leagueModel, team, allMatches, pointsDeducted),
                PointsDeducted = pointsDeducted,
                PointsDeductionReason = pointsDeductionModel?.Reason
            };

            leagueTableRow.PointsPerGame = CalculatePointsPerGame(leagueTableRow);
            return leagueTableRow;
        }

        private double CalculatePointsPerGame(LeagueTableRowDto leagueTableRowDto) =>
            leagueTableRowDto.Points / (double) leagueTableRowDto.Played;

        private int CalculateGoalsAgainst(
            IEnumerable<MatchModel> homeMatches,
            IEnumerable<MatchModel> awayMatches)
        {
            return homeMatches.Sum(m => m.AwayGoals) + awayMatches.Sum(m => m.HomeGoals);
        }

        private int CalculateGoalsFor(
            IEnumerable<MatchModel> homeMatches,
            IEnumerable<MatchModel> awayMatches)
        {
            return homeMatches.Sum(m => m.HomeGoals) + awayMatches.Sum(m => m.AwayGoals);
        }

        private int CalculatePoints(
            LeagueModel leagueModel,
            string team,
            List<MatchModel> matches,
            int pointsDeducted) =>
            CountWins(matches, team) * leagueModel.PointsForWin
            + CountDraws(matches, team)
            - pointsDeducted;

        private List<LeagueTableRowDto> AddMissingTeams(
            List<LeagueTableRowDto> leagueTable,
            List<string> teams)
        {
            var existingTeams = leagueTable.Select(r => r.Team);
            var missingTeams = teams.Except(existingTeams);

            leagueTable.AddRange(missingTeams.Select(team => new LeagueTableRowDto { Team = team }));
            return leagueTable;
        }

        private int CountDraws(List<MatchModel> teamMatches, string team)
        {
            return teamMatches.Count(m => TeamDrewMatch(m, team));
        }

        private int CountDefeats(List<MatchModel> matches, string team)
        {
            return matches.Count(m => TeamLostMatch(m, team));
        }

        private int CountWins(List<MatchModel> matches, string team)
        {
            return matches.Count(m => TeamWonMatch(m, team));
        }

        private bool MatchInvolvesTeam(MatchModel match, string team) =>
            match.HomeTeam == team || match.AwayTeam == team;

        private bool TeamWonMatch(MatchModel match, string team) =>
            match.HomeTeam == team && HomeTeamWon(match)
            || match.AwayTeam == team && AwayTeamWon(match);

        private bool TeamLostMatch(MatchModel match, string team) =>
            match.HomeTeam == team && AwayTeamWon(match)
            || match.AwayTeam == team && HomeTeamWon(match);

        private bool TeamDrewMatch(MatchModel match, string team) =>
            !TeamWonMatch(match, team) && !TeamLostMatch(match, team);

        private bool HomeTeamWon(MatchModel match) =>
            match.HomeGoals > match.AwayGoals
            || match.HomeGoalsExtraTime > match.AwayGoalsExtraTime
            || match.HomePenaltiesScored > match.AwayPenaltiesScored;

        private bool AwayTeamWon(MatchModel match) =>
            match.HomeGoals < match.AwayGoals
            || match.HomeGoalsExtraTime < match.AwayGoalsExtraTime
            || match.HomePenaltiesScored < match.AwayPenaltiesScored;
    }
}
