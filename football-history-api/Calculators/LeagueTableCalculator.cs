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

            // Due to COVID-19, tiers 3 and 4 were abandoned in 2019-202 and average points per game was used instead
            var covidAbandoned = leagueModel.StartYear == 2019 && (leagueModel.Tier == 3 || leagueModel.Tier == 4);
            if (covidAbandoned)
            {
                AdjustToAveragePoints(leagueTable); // TODO: Consider returning points and average points per game?
            }

            var sortedLeagueTable = SortTable(leagueTable, leagueModel);
            return AddStatuses(sortedLeagueTable, playOffMatches, leagueModel);
        }

        public static List<LeagueTableRow> GetPartialLeagueTable(List<MatchModel> leagueMatches,
            LeagueModel leagueModel, List<PointsDeductionModel> pointsDeductions, DateTime date)
        {
            var matches = leagueMatches.Where(m => m.Date < date).ToList();
            var leagueTable = GetTable(matches, leagueModel, pointsDeductions);
            var expandedLeagueTable =
                AddMissingTeams(leagueTable, leagueMatches.Select(m => m.HomeTeam).Distinct().ToList());
            return SortTable(expandedLeagueTable, leagueModel);
        }

        private static void AdjustToAveragePoints(List<LeagueTableRow> leagueTable)
        {
            foreach (var row in leagueTable)
            {
                row.Points /= row.Played;
            }
        }

        private static List<LeagueTableRow> GetTable(List<MatchModel> leagueMatches, LeagueModel leagueModel,
            List<PointsDeductionModel> pointDeductions)
        {
            var leagueTable = new List<LeagueTableRow>();

            var teams = leagueMatches.Select(m => m.HomeTeam).Distinct();
            foreach (var team in teams)
            {
                var teamMatches = leagueMatches.Where(m => MatchInvolvesTeam(m, team)).ToList();
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

        private static List<LeagueTableRow> AddStatuses(List<LeagueTableRow> leagueTable,
            List<MatchModel> playOffMatches, LeagueModel leagueModel)
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
                    r.Status = "PlayOffs";
                }

                if (r.Team == playOffWinner)
                {
                    r.Status = "PlayOff Winner";
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
                        t.Team)
                    .ToList(); // unless it affects a promotion/relegation spot at the end of the season in which case a play-off occurs (this has never happened)
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

        private static List<LeagueTableRow> AddMissingTeams(List<LeagueTableRow> leagueTable, List<string> teams)
        {
            var existingTeams = leagueTable.Select(r => r.Team);
            var missingTeams = teams.Except(existingTeams);

            leagueTable.AddRange(missingTeams.Select(team => new LeagueTableRow {Team = team}));
            return leagueTable;
        }

        private static bool PremierLeague_Or_FootballLeagueFrom1999(LeagueModel leagueModel)
        {
            return leagueModel.StartYear >= 1999 || leagueModel.Name == "Premier League";
        }

        private static int GetNumDraws(List<MatchModel> teamMatches, string team)
        {
            return teamMatches.Count(m => TeamDrewMatch(m, team));
        }

        private static int GetNumDefeats(List<MatchModel> matches, string team)
        {
            return matches.Count(m => TeamLostMatch(m, team));
        }

        private static int GetNumWins(List<MatchModel> matches, string team)
        {
            return matches.Count(m => TeamWonMatch(m, team));
        }

        private static string? GetPlayOffWinner(List<MatchModel> playOffMatches)
        {
            var playOffFinal = playOffMatches.SingleOrDefault(m => m.Round == "Final");
            return playOffFinal == null ? null : GetMatchWinner(playOffFinal);
        }

        public static bool MatchInvolvesTeam(MatchModel match, string team)
        {
            return match.HomeTeam == team || match.AwayTeam == team;
        }

        private static string? GetMatchWinner(MatchModel matchModel)
        {
            if (matchModel.HomeGoals > matchModel.AwayGoals)
            {
                return matchModel.HomeTeam;
            }

            if (matchModel.HomeGoals < matchModel.AwayGoals)
            {
                return matchModel.AwayTeam;
            }

            if (matchModel.HomeGoalsExtraTime > matchModel.AwayGoalsExtraTime)
            {
                return matchModel.HomeTeam;
            }

            if (matchModel.HomeGoalsExtraTime < matchModel.AwayGoalsExtraTime)
            {
                return matchModel.AwayTeam;
            }

            if (matchModel.HomePenaltiesScored > matchModel.AwayPenaltiesScored)
            {
                return matchModel.HomeTeam;
            }

            if (matchModel.HomePenaltiesScored < matchModel.AwayPenaltiesScored)
            {
                return matchModel.AwayTeam;
            }

            return null;
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

        public static bool TeamDrewMatch(MatchModel match, string team)
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