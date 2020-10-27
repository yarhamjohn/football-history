using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Builders;
using football.history.api.Repositories.League;
using football.history.api.Repositories.Match;

namespace football.history.api.Calculators
{
    public static class StatusCalculator
    {
        public static string? AddStatuses(
            LeagueTableRow row,
            IEnumerable<MatchModel> playOffMatches,
            LeagueModel leagueModel)
        {
            if (row.Position == 1)
            {
                return "Champions";
            }

            if (InPromotionPlaces(row, leagueModel))
            {
                return "Promoted";
            }

            if (InPlayOffPlaces(row, leagueModel))
            {
                return PlayOffWinner(row, playOffMatches) ? "PlayOff Winner" : "PlayOffs";
            }

            if (InRelegationZone(row, leagueModel))
            {
                return "Relegated";
            }

            return null;
        }

        private static bool PlayOffWinner(
            LeagueTableRow row,
            IEnumerable<MatchModel> playOffMatches) =>
            row.Team == GetPlayOffWinner(playOffMatches);

        private static bool InPlayOffPlaces(LeagueTableRow row, LeagueModel leagueModel) =>
            row.Position > leagueModel.PromotionPlaces
            && row.Position <= leagueModel.PromotionPlaces + leagueModel.PlayOffPlaces;

        private static bool InPromotionPlaces(LeagueTableRow row, LeagueModel leagueModel) =>
            row.Position > 1 && row.Position <= leagueModel.PromotionPlaces;

        private static bool InRelegationZone(LeagueTableRow r, LeagueModel leagueModel) =>
            r.Position > leagueModel.TotalPlaces - leagueModel.RelegationPlaces;

        private static string? GetPlayOffWinner(IEnumerable<MatchModel> playOffMatches)
        {
            var playOffFinal = playOffMatches.SingleOrDefault(m => m.Round == "Final");
            return playOffFinal == null ? null : GetMatchWinner(playOffFinal);
        }

        private static string GetMatchWinner(MatchModel playOffFinal)
        {
            if (HomeTeamWon(playOffFinal))
            {
                return playOffFinal.HomeTeam;
            }

            if (AwayTeamWon(playOffFinal))
            {
                return playOffFinal.AwayTeam;
            }

            throw new InvalidOperationException("The specified play off final had no winner.");
        }

        private static bool AwayTeamWon(MatchModel playOffFinal) =>
            playOffFinal.HomeGoals < playOffFinal.AwayGoals
            || playOffFinal.HomeGoalsExtraTime > playOffFinal.AwayGoalsExtraTime
            || playOffFinal.HomePenaltiesScored > playOffFinal.AwayPenaltiesScored;

        private static bool HomeTeamWon(MatchModel playOffFinal) =>
            playOffFinal.HomeGoals > playOffFinal.AwayGoals
            || playOffFinal.HomeGoalsExtraTime < playOffFinal.AwayGoalsExtraTime
            || playOffFinal.HomePenaltiesScored < playOffFinal.AwayPenaltiesScored;
    }
}
