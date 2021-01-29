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
            LeagueTableRowDto rowDto,
            IEnumerable<MatchModel> playOffMatches,
            IEnumerable<MatchModel> relegationPlayOffMatches,
            LeagueModel leagueModel)
        {
            if (rowDto.Position == 1)
            {
                return "Champions";
            }

            if (InPromotionPlaces(rowDto, leagueModel))
            {
                return "Promoted";
            }

            if (InPlayOffPlaces(rowDto, leagueModel))
            {
                return IsPlayOffWinner(rowDto, playOffMatches, leagueModel) ? "PlayOff Winner" : "PlayOffs";
            }

            if (InRelegationPlayOffPlaces(rowDto, leagueModel))
            {
                Console.WriteLine(JsonSerializer.Serialize(rowDto));
                Console.WriteLine(JsonSerializer.Serialize(relegationPlayOffMatches));
                return IsPlayOffWinner(rowDto, relegationPlayOffMatches, leagueModel)
                    ? "Relegation PlayOffs"
                    : "Relegated - PlayOffs";
            }

            if (InRelegationZone(rowDto, leagueModel))
            {
                return "Relegated";
            }

            if (InReElectionPlaces(rowDto, leagueModel))
            {
                return FailedReElection(rowDto, leagueModel) ? "Failed Re-election" : "Re-elected";
            }

            return null;
        }

        private static bool IsPlayOffWinner(
            LeagueTableRowDto rowDto,
            IEnumerable<MatchModel> playOffMatches,
            LeagueModel leagueModel)
        {
            var playOffFinalMatches = playOffMatches.Where(m => m.Round == "Final").ToList();
            var result = playOffFinalMatches.Count switch
            {
                1 => rowDto.Team == GetOneLeggedFinalWinner(playOffFinalMatches.Single()),
                2 => rowDto.Team == GetTwoLeggedFinalWinner(playOffFinalMatches),
                3 => rowDto.Team == GetReplayFinalWinner(playOffFinalMatches),
                _ => false,
            };

            return leagueModel.StartYear == 1989 && leagueModel.Tier == 2 ? FixPlayOffWinnerFor1989(rowDto) : result;
        }

        private static bool FixPlayOffWinnerFor1989(LeagueTableRowDto rowDto)
        {
            return rowDto.Team switch
            {
                // Sunderland were promoted instead of Swindon Town despite Swindon winning the play-offs due to financial irregularities.
                "Sunderland" => true,
                _ => false
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

        private static bool InPlayOffPlaces(LeagueTableRowDto rowDto, LeagueModel leagueModel) =>
            rowDto.Position > leagueModel.PromotionPlaces
            && rowDto.Position <= leagueModel.PromotionPlaces + leagueModel.PlayOffPlaces;

        private static bool InPromotionPlaces(LeagueTableRowDto rowDto, LeagueModel leagueModel) =>
            rowDto.Position > 1 && rowDto.Position <= leagueModel.PromotionPlaces;

        private static bool InRelegationZone(LeagueTableRowDto rowDto, LeagueModel leagueModel) =>
            rowDto.Position > leagueModel.TotalPlaces - leagueModel.RelegationPlaces;

        private static bool InReElectionPlaces(LeagueTableRowDto rowDto, LeagueModel leagueModel) =>
            rowDto.Position > leagueModel.TotalPlaces - leagueModel.ReElectionPlaces;

        private static bool FailedReElection(LeagueTableRowDto rowDto, LeagueModel leagueModel) =>
            leagueModel.FailedReElectionPosition == rowDto.Position;

        private static bool
            InRelegationPlayOffPlaces(LeagueTableRowDto rowDto, LeagueModel leagueModel) =>
            !InRelegationZone(rowDto, leagueModel)
            && rowDto.Position
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
