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
        public static List<LeagueTableRow> GetFullLeagueTable(List<MatchModel> leagueMatches,
            List<MatchModel> playOffMatches, LeagueModel leagueModel, List<PointsDeductionModel> pointsDeductions)
        {
            var leagueTable = GetTable(leagueMatches, leagueModel, pointsDeductions);
            var sortedLeagueTable = LeagueTableSorter.SortTable(leagueTable, leagueModel);
            return StatusCalculator.AddStatuses(sortedLeagueTable, playOffMatches, leagueModel);
        }

        public static List<LeagueTableRow> GetPartialLeagueTable(List<MatchModel> leagueMatches,
            LeagueModel leagueModel, List<PointsDeductionModel> pointsDeductions, DateTime date)
        {
            var matchesToDate = leagueMatches.Where(m => m.Date < date).ToList();
            var leagueTable = GetTable(matchesToDate, leagueModel, pointsDeductions);
            
            var allTeamsInLeague = GetTeamsInvolvedInMatches(leagueMatches);
            var expandedLeagueTable = AddMissingTeams(leagueTable, allTeamsInLeague);
            
            return LeagueTableSorter.SortTable(expandedLeagueTable, leagueModel);
        }

        private static List<string> GetTeamsInvolvedInMatches(List<MatchModel> leagueMatches)
        {
            return leagueMatches.SelectMany(m => new[] {m.HomeTeam, m.AwayTeam}).Distinct().ToList();
        }

        private static List<LeagueTableRow> GetTable(List<MatchModel> matches, LeagueModel leagueModel,
            List<PointsDeductionModel> pointDeductions)
        {
            var leagueTable = new List<LeagueTableRow>();

            var teams = GetTeamsInvolvedInMatches(matches);
            foreach (var team in teams)
            {
                var teamMatches = matches.Where(m => MatchInvolvesTeam(m, team)).ToList();
                var numWins = CountWins(teamMatches, team);
                var numDefeats = CountDefeats(teamMatches, team);
                var numDraws = CountDraws(teamMatches, team);

                var homeTeamMatches = matches.Where(m => m.HomeTeam == team).ToList();
                var awayTeamMatches = matches.Where(m => m.AwayTeam == team).ToList();
                var goalsFor = homeTeamMatches.Sum(m => m.HomeGoals) + awayTeamMatches.Sum(m => m.AwayGoals);
                var goalsAgainst = homeTeamMatches.Sum(m => m.AwayGoals) + awayTeamMatches.Sum(m => m.HomeGoals);
                var pointsDeductionModel = pointDeductions.SingleOrDefault(p => p.Team == team);
                var pointsDeducted = pointsDeductionModel?.PointsDeducted ?? 0;

                var leagueTableRow = new LeagueTableRow
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
                };

                leagueTableRow.PointsPerGame = (leagueTableRow.Points - leagueTableRow.PointsDeducted) / (double) leagueTableRow.Played;
                
                leagueTable.Add(leagueTableRow);
            }

            return leagueTable;
        }

        private static List<LeagueTableRow> AddMissingTeams(List<LeagueTableRow> leagueTable, List<string> teams)
        {
            var existingTeams = leagueTable.Select(r => r.Team);
            var missingTeams = teams.Except(existingTeams);

            leagueTable.AddRange(missingTeams.Select(team => new LeagueTableRow {Team = team}));
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

        private static bool MatchInvolvesTeam(MatchModel match, string team)
        {
            return match.HomeTeam == team || match.AwayTeam == team;
        }

        private static bool TeamWonMatch(MatchModel match, string team)
        {
            return match.HomeTeam == team && HomeTeamWon(match) ||
                   match.AwayTeam == team && AwayTeamWon(match);
        }

        private static bool TeamLostMatch(MatchModel match, string team)
        {
            return match.HomeTeam == team && AwayTeamWon(match) ||
                   match.AwayTeam == team && HomeTeamWon(match);
        }

        private static bool TeamDrewMatch(MatchModel match, string team)
        {
            return !TeamWonMatch(match, team) && !TeamLostMatch(match, team);
        }

        private static bool HomeTeamWon(MatchModel match)
        {
            return match.HomeGoals > match.AwayGoals
                   || match.HomeGoalsExtraTime > match.AwayGoalsExtraTime
                   || match.HomePenaltiesScored > match.AwayPenaltiesScored;
        }

        private static bool AwayTeamWon(MatchModel match)
        {
            return match.HomeGoals < match.AwayGoals
                   || match.HomeGoalsExtraTime < match.AwayGoalsExtraTime
                   || match.HomePenaltiesScored < match.AwayPenaltiesScored;
        }
    }
}