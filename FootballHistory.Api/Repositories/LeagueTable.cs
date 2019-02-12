using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories.Models;
using Microsoft.AspNetCore.JsonPatch.Helpers;

namespace FootballHistory.Api.Repositories
{
    public class LeagueTable
    {
        private List<LeagueTableRow> _leagueTable;
        
        public LeagueTable()
        {
            _leagueTable = new List<LeagueTableRow>();    
        }

        public List<LeagueTableRow> GetTable()
        {
            return _leagueTable;
        }
        
        public void AddLeagueRows(List<MatchDetailModel> leagueMatchDetails)
        {
            var filteredHomeTeams = leagueMatchDetails.Select(m => m.HomeTeam).ToList();
            var filteredAwayTeams = leagueMatchDetails.Select(m => m.AwayTeam).ToList();
            var teams = filteredHomeTeams.Union(filteredAwayTeams).ToList();
            
            foreach (var team in teams)
            {
                var homeGames = leagueMatchDetails.Where(m => m.HomeTeam == team).ToList();
                var awayGames = leagueMatchDetails.Where(m => m.AwayTeam == team).ToList();

                _leagueTable.Add(
                    new LeagueTableRow
                    {
                        Team = team,
                        Won = homeGames.Count(g => g.HomeGoals > g.AwayGoals) + awayGames.Count(g => g.AwayGoals > g.HomeGoals),
                        Drawn = homeGames.Count(g => g.HomeGoals == g.AwayGoals) + awayGames.Count(g => g.AwayGoals == g.HomeGoals),
                        Lost = homeGames.Count(g => g.HomeGoals < g.AwayGoals) + awayGames.Count(g => g.AwayGoals < g.HomeGoals),
                        GoalsFor = homeGames.Sum(g => g.HomeGoals) + awayGames.Sum(g => g.AwayGoals),
                        GoalsAgainst = homeGames.Sum(g => g.AwayGoals) + awayGames.Sum(g => g.HomeGoals),
                    }
                );
            }

            foreach (var row in _leagueTable)
            {
                row.Played = row.Won + row.Drawn + row.Lost;
                row.GoalDifference = row.GoalsFor - row.GoalsAgainst;
                row.Points = (row.Won * 3) + row.Drawn;
            }
        }

        public void IncludePointDeductions(List<PointDeductionModel> pointDeductions)
        {
            foreach (var row in _leagueTable)
            {
                var deduction = pointDeductions.Where(d => d.Team == row.Team).ToList();

                if (deduction.Count == 0)
                {
                    row.PointsDeducted = 0;
                    row.PointsDeductionReason = string.Empty;
                } 
                else 
                {
                    var d = deduction.Single();

                    row.PointsDeducted = d.PointsDeducted;
                    row.PointsDeductionReason = d.Reason;
                    row.Points -= d.PointsDeducted;
                }
            }
        }
        
        public void SetLeaguePosition()
        {
            _leagueTable = _leagueTable.Select((t, i) => { 
                t.Position = i + 1; 
                return t; 
            }).ToList();
        }
        
        public void SortLeagueTable()
        {
            _leagueTable = _leagueTable
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalDifference) // Goal ratio was used prior to 1976-77
                .ThenByDescending(t => t.GoalsFor)
                // head to head
                .ThenBy(t => t.Team) // unless it affects a promotion/relegation spot at the end of the season in which case a play-off occurs (this has never happened)
                .ToList(); 
        }
        
        public void AddTeamStatus(LeagueDetailModel leagueDetailModel, IEnumerable<MatchDetailModel> playOffMatchDetails)
        {
            var playOffFinal = playOffMatchDetails.Where(m => m.Round == "Final").ToList();
            
            foreach (var row in _leagueTable)
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

        public void AddMissingTeams(List<string> missingTeams)
        {
            foreach (var t in missingTeams)
            {
                _leagueTable.Add(new LeagueTableRow
                {
                    Team = t,
                    Won = 0,
                    Drawn = 0,
                    Lost = 0,
                    GoalsFor = 0,
                    GoalsAgainst = 0
                });
            }
        }
    }
}
