using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders.Models
{
    public class LeagueTable : ILeagueTable
    {
        public List<LeagueTableRow> Rows { get; set; }

        public LeagueTable()
        {
            Rows = new List<LeagueTableRow>();
        }

        public LeagueTable AddPositionsAndStatuses(LeagueDetailModel leagueDetailModel, List<MatchDetailModel> playOffMatches)
        {
            var positionedLeagueTable = AddPositions();
            return positionedLeagueTable.AddStatuses(leagueDetailModel, playOffMatches);
        }

        private LeagueTable AddStatuses(LeagueDetailModel leagueDetailModel, List<MatchDetailModel> playOffMatches)
        {
            if (leagueDetailModel.TotalPlaces != Rows.Count)
            {
                throw new Exception($"The League Detail Model ({leagueDetailModel.TotalPlaces} places) does not match the League Table ({Rows.Count} rows)");
            }

            if (leagueDetailModel.PlayOffPlaces > 0 && !playOffMatches.Exists(m => m.Round == "Final"))
            {
                throw new Exception(
                    $"The League Detail Model contains {leagueDetailModel.PlayOffPlaces} playoff places but the playoff matches provided contain no Final");
            }
            
            return new LeagueTable
            {
                Rows = Rows.Select(r =>
                {
                    if (r.Position == 1)
                    {
                        r.Status = "C";
                    }
                    else if (InPromotionPlaces(r, leagueDetailModel))
                    {
                        r.Status = "P";
                    }
                    else if (InPlayOffPosition(r, leagueDetailModel))
                    {
                        r.Status = IsPlayOffWinner(r, playOffMatches) ? "PO (P)" : "PO";
                    }
                    else if (InRelegationPlaces(r, leagueDetailModel))
                    {
                        r.Status = "R";
                    }
                    else
                    {
                        r.Status = string.Empty;
                    }

                    return r;
                }).ToList()
            };
        }

        private static bool IsPlayOffWinner(LeagueTableRow row, List<MatchDetailModel> playOffMatches)
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

            return row.Team == winner;
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

        private static bool InRelegationPlaces(LeagueTableRow row, LeagueDetailModel leagueDetailModel)
        {
            return row.Position > leagueDetailModel.TotalPlaces - leagueDetailModel.RelegationPlaces;
        }

        private static bool InPromotionPlaces(LeagueTableRow row, LeagueDetailModel leagueDetailModel)
        {
            return row.Position <= leagueDetailModel.PromotionPlaces;
        }

        private static bool InPlayOffPosition(LeagueTableRow row, LeagueDetailModel leagueDetailModel)
        {
            var placesAbovePlayOffs = leagueDetailModel.PromotionPlaces == 0 ? 1 : leagueDetailModel.PromotionPlaces;
            return row.Position > placesAbovePlayOffs && row.Position <= placesAbovePlayOffs + leagueDetailModel.PlayOffPlaces;
        }

        private LeagueTable AddPositions()
        {
            var sortedRows = SortTableRows();
            return new LeagueTable
            {
                Rows = sortedRows.Select((t, i) =>
                {
                    t.Position = i + 1;
                    return t;
                }).ToList()
            };
        }

        private List<LeagueTableRow> SortTableRows()
        {
            //TODO: Add extra ordering rules when required
            return Rows
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalDifference) // Goal ratio was used prior to 1976-77
                .ThenByDescending(t => t.GoalsFor)
                // head to head
                .ThenBy(t => t.Team) // unless it affects a promotion/relegation spot at the end of the season in which case a play-off occurs (this has never happened)
                .ToList(); 
        }
    }
}