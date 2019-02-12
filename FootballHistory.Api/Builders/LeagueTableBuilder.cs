using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders
{
    public class LeagueTableBuilder : ILeagueTableBuilder
    {
        public List<LeagueTableRow> Build(List<MatchDetailModel> leagueMatchDetails, LeagueDetailModel leagueDetail, List<PointDeductionModel> pointDeductions, List<MatchDetailModel> playOffMatches)
        {
            var table = new List<LeagueTableRow>();

            CommonStuff.AddLeagueRows(table, leagueMatchDetails);
            CommonStuff.IncludePointDeductions(table, pointDeductions);

            table = CommonStuff.SortLeagueTable(table);

            CommonStuff.SetLeaguePosition(table);
            AddTeamStatus(table, leagueDetail, playOffMatches);            

            return table;
        }

        private static void AddTeamStatus(IEnumerable<LeagueTableRow> leagueTable, LeagueDetailModel leagueDetailModel, IEnumerable<MatchDetailModel> playOffMatchDetails)
        {
            var playOffFinal = playOffMatchDetails.Where(m => m.Round == "Final").ToList();
            
            foreach (var row in leagueTable)
            {
                if (row.Position == 1)
                {
                    row.Status = "C";
                }
                else if (row.Position <= leagueDetailModel.PromotionPlaces)
                {
                    row.Status = "P";
                }
                else if (playOffFinal.Count == 1 && row.Position <= leagueDetailModel.PlayOffPlaces + leagueDetailModel.PromotionPlaces)
                {
                    var final = playOffFinal.Single();
                    var winner = final.PenaltyShootout 
                        ? (final.HomePenaltiesScored > final.AwayPenaltiesScored ? final.HomeTeam : final.AwayTeam) 
                        : final.ExtraTime
                            ? (final.HomeGoalsET > final.AwayGoalsET ? final.HomeTeam : final.AwayTeam) 
                            : (final.HomeGoals > final.AwayGoals ? final.HomeTeam : final.AwayTeam);
                    
                    if (row.Team == winner)
                    {
                        row.Status = "PO (P)";
                    }
                    else
                    {
                        row.Status = "PO";
                    }
                }
                else if (row.Position > leagueDetailModel.TotalPlaces - leagueDetailModel.RelegationPlaces)
                {
                    row.Status = "R";
                }
                else
                {
                    row.Status = string.Empty;
                }
            }
        }
    }
}
