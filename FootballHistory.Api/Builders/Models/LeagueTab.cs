using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders.Models
{
    public class LeagueTab
    {
        public List<LeagueTableRow> Rows { get; set; }

        public LeagueTab()
        {
            Rows = new List<LeagueTableRow>();
        }

        public LeagueTab AddPositionsAndStatuses(LeagueDetailModel leagueDetailModel, List<MatchDetailModel> playOffMatches)
        {
            var positionedLeagueTable = AddPositions();
            return positionedLeagueTable.AddStatuses(leagueDetailModel, playOffMatches);

        }

        private LeagueTab AddStatuses(LeagueDetailModel leagueDetailModel, List<MatchDetailModel> playOffMatches)
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
            
            return new LeagueTab
            {
                Rows = Rows.Select(r =>
                {
                    if (r.Position == 1)
                    {
                        r.Status = "C";
                    }
                    else if (r.Position <= leagueDetailModel.PromotionPlaces)
                    {
                        r.Status = "P";
                    }
                    else
                    {
                        var placesAbovePlayOffs = leagueDetailModel.PromotionPlaces == 0 ? 1 : leagueDetailModel.PromotionPlaces;
                        if (r.Position <= placesAbovePlayOffs + leagueDetailModel.PlayOffPlaces)
                        {
                            var final = playOffMatches.Single(m => m.Round == "Final");
                            var winner = final.PenaltyShootout 
                                ? final.HomePenaltiesScored > final.AwayPenaltiesScored ? final.HomeTeam : final.AwayTeam 
                                : final.ExtraTime
                                    ? (final.HomeGoalsET > final.AwayGoalsET ? final.HomeTeam : final.AwayTeam) 
                                    : (final.HomeGoals > final.AwayGoals ? final.HomeTeam : final.AwayTeam);

                            r.Status = r.Team == winner ? "PO (P)" : "PO";
                        }
                        else if (r.Position > leagueDetailModel.TotalPlaces - leagueDetailModel.RelegationPlaces)
                        {
                            r.Status = "R";
                        }
                        else
                        {
                            r.Status = string.Empty;
                        }
                    }

                    return r;
                }).ToList()
            };
        }

        private LeagueTab AddPositions()
        {
            var sortedRows = SortTableRows();
            return new LeagueTab
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