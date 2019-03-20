using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using FootballHistory.Api.Repositories.MatchDetailRepository;

namespace FootballHistory.Api.LeagueSeason.Table
{
    public class LeagueTableStatusCalculator : ILeagueTableStatusCalculator
    {
        public LeagueTable AddStatuses(LeagueTable leagueTable, LeagueDetailModel leagueDetailModel, List<MatchDetailModel> playOffMatches)
        {
            if (leagueDetailModel.TotalPlaces != leagueTable.Rows.Count)
            {
                throw new Exception(
                    $"The League Detail Model ({leagueDetailModel.TotalPlaces} places) does not match the League Table ({leagueTable.Rows.Count} rows)");
            }

            if (leagueDetailModel.PlayOffPlaces > 0 && !playOffMatches.Exists(m => m.Round == "Final"))
            {
                throw new Exception(
                    $"The League Detail Model contains {leagueDetailModel.PlayOffPlaces} playoff places but the playoff matches provided contain no Final");
            }

            foreach (var row in leagueTable.Rows)
            {
                row.Status = CalculateStatus(row.Position, row.Team, leagueDetailModel, playOffMatches);
            }

            return leagueTable;
        }        
        
        private string CalculateStatus(int position, string team, LeagueDetailModel leagueDetailModel, List<MatchDetailModel> playOffMatches)
        {
            if (position == 1)
            {
                return "C";
            }
            
            if (InPromotionPlaces(position, leagueDetailModel))
            {
                return "P";
            }
            
            if (InPlayOffPosition(position, leagueDetailModel))
            {
                return IsPlayOffWinner(team, playOffMatches) ? "PO (P)" : "PO";
            }
            
            if (InRelegationPlaces(position, leagueDetailModel))
            {
                return "R";
            }
            
            return string.Empty;
        }

        private static bool IsPlayOffWinner(string team, List<MatchDetailModel> playOffMatches)
        {
            var final = playOffMatches.Single(m => m.Round == "Final");
            
            string winner;
            if (final.PenaltyShootout)
            {
                winner = PenaltyShootoutWinner(final);
            }
            else
            {
                winner = final.ExtraTime ? ExtraTimeWinner(final) : NormalTimeWinner(final);
            }

            return team == winner;
        }

        private static string NormalTimeWinner(MatchDetailModel final)
        {
            return final.HomeGoals > final.AwayGoals ? final.HomeTeam : final.AwayTeam;
        }

        private static string ExtraTimeWinner(MatchDetailModel final)
        {
            return final.HomeGoalsET > final.AwayGoalsET ? final.HomeTeam : final.AwayTeam;
        }

        private static string PenaltyShootoutWinner(MatchDetailModel final)
        {
            return final.HomePenaltiesScored > final.AwayPenaltiesScored ? final.HomeTeam : final.AwayTeam;
        }

        private static bool InRelegationPlaces(int position, LeagueDetailModel leagueDetailModel)
        {
            return position > leagueDetailModel.TotalPlaces - leagueDetailModel.RelegationPlaces;
        }

        private static bool InPromotionPlaces(int position, LeagueDetailModel leagueDetailModel)
        {
            return position <= leagueDetailModel.PromotionPlaces;
        }

        private static bool InPlayOffPosition(int position, LeagueDetailModel leagueDetailModel)
        {
            var placesAbovePlayOffs = leagueDetailModel.PromotionPlaces == 0 ? 1 : leagueDetailModel.PromotionPlaces;
            return position > placesAbovePlayOffs && position <= placesAbovePlayOffs + leagueDetailModel.PlayOffPlaces;
        }
    }
}