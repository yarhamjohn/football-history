using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            IEnumerable<MatchModel> relegationPlayOffMatches,
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
                return IsPlayOffWinner(row, playOffMatches) ? "PlayOff Winner" : "PlayOffs";
            }

            if (InRelegationPlayOffPlaces(row, leagueModel))
            {
                Console.WriteLine(JsonSerializer.Serialize(row));
                Console.WriteLine(JsonSerializer.Serialize(relegationPlayOffMatches));
                return IsPlayOffWinner(row, relegationPlayOffMatches)
                    ? "Relegation PlayOffs"
                    : "Relegated - PlayOffs";
            }

            if (InRelegationZone(row, leagueModel))
            {
                return "Relegated";
            }

            return null;
        }

        private static bool IsPlayOffWinner(
            LeagueTableRow row,
            IEnumerable<MatchModel> playOffMatches)
        {
            var playOffFinalMatches = playOffMatches.Where(m => m.Round == "Final").ToList();
            return playOffFinalMatches.Count switch
            {
                1 => row.Team == GetOneLeggedFinalWinner(playOffFinalMatches.Single()),
                2 => row.Team == GetTwoLeggedFinalWinner(playOffFinalMatches),
                3 => row.Team == GetReplayFinalWinner(playOffFinalMatches),
                _ => false,
            };
        }

        private static string GetTwoLeggedFinalWinner(List<MatchModel> matches)
        {
            if (matches.Count != 2)
            {
                throw new InvalidOperationException($"Expected 2 matches but got {matches.Count}.");
            }

            var firstLeg = matches.OrderBy(m => m.Date).First();
            var secondLeg = matches.OrderBy(m => m.Date).Last();

            var teamOneGoals = firstLeg.HomeGoals
                + secondLeg.AwayGoals
                + secondLeg.AwayGoalsExtraTime
                + secondLeg.AwayPenaltiesScored;
            var teamTwoGoals = firstLeg.AwayGoals
                + secondLeg.HomeGoals
                + secondLeg.HomeGoalsExtraTime
                + secondLeg.HomePenaltiesScored;
            if (teamOneGoals > teamTwoGoals)
            {
                return firstLeg.HomeTeam;
            }

            if (teamOneGoals < teamTwoGoals)
            {
                return firstLeg.AwayTeam;
            }

            throw new InvalidOperationException("The specified two legged matches had no winner.");
        }

        private static string GetReplayFinalWinner(List<MatchModel> matches)
        {
            var replayMatch = matches.OrderBy(m => m.Date).Last();
            return GetOneLeggedFinalWinner(replayMatch);
        }

        private static bool InPlayOffPlaces(LeagueTableRow row, LeagueModel leagueModel) =>
            row.Position > leagueModel.PromotionPlaces
            && row.Position <= leagueModel.PromotionPlaces + leagueModel.PlayOffPlaces;

        private static bool InPromotionPlaces(LeagueTableRow row, LeagueModel leagueModel) =>
            row.Position > 1 && row.Position <= leagueModel.PromotionPlaces;

        private static bool InRelegationZone(LeagueTableRow row, LeagueModel leagueModel) =>
            row.Position > leagueModel.TotalPlaces - leagueModel.RelegationPlaces;

        private static bool
            InRelegationPlayOffPlaces(LeagueTableRow row, LeagueModel leagueModel) =>
            !InRelegationZone(row, leagueModel)
            && row.Position
            > leagueModel.TotalPlaces
            - (leagueModel.RelegationPlaces + leagueModel.RelegationPlayOffPlaces);

        private static string GetOneLeggedFinalWinner(MatchModel match)
        {
            if (HomeTeamWon(match))
            {
                return match.HomeTeam;
            }

            if (AwayTeamWon(match))
            {
                return match.AwayTeam;
            }

            throw new InvalidOperationException("The specified match had no winner.");
        }

        private static bool AwayTeamWon(MatchModel playOffFinal) =>
            playOffFinal.HomeGoals < playOffFinal.AwayGoals
            || playOffFinal.HomeGoalsExtraTime < playOffFinal.AwayGoalsExtraTime
            || playOffFinal.HomePenaltiesScored < playOffFinal.AwayPenaltiesScored;

        private static bool HomeTeamWon(MatchModel playOffFinal) =>
            playOffFinal.HomeGoals > playOffFinal.AwayGoals
            || playOffFinal.HomeGoalsExtraTime > playOffFinal.AwayGoalsExtraTime
            || playOffFinal.HomePenaltiesScored > playOffFinal.AwayPenaltiesScored;
    }
}
