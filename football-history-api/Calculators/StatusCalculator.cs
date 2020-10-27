using System.Collections.Generic;
using System.Linq;
using football.history.api.Builders;
using football.history.api.Repositories.League;
using football.history.api.Repositories.Match;

namespace football.history.api.Calculators
{
    public static class StatusCalculator
    {
        public static List<LeagueTableRow> AddStatuses(List<LeagueTableRow> leagueTable,
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
        
        
        private static string? GetPlayOffWinner(List<MatchModel> playOffMatches)
        {
            var playOffFinal = playOffMatches.SingleOrDefault(m => m.Round == "Final");
            return playOffFinal == null ? null : GetMatchWinner(playOffFinal);
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
    }
}