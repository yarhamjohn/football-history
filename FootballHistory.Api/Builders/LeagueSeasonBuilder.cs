using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders
{
    public class LeagueSeasonBuilder : ILeagueSeasonBuilder
    {
        private readonly IPlayOffMatchesRepository _playOffMatchesRepository;
        private readonly ILeagueMatchesRepository _leagueMatchesRepository;
        private readonly ILeagueRepository _leagueRepository;
        private readonly IPointDeductionsRepository _pointDeductionsRepository;

        public LeagueSeasonBuilder(IPlayOffMatchesRepository playOffMatchesRepository, 
            ILeagueMatchesRepository leagueMatchesRepository,
            ILeagueRepository leagueRepository,
            IPointDeductionsRepository pointDeductionsRepository)
        {
            _playOffMatchesRepository = playOffMatchesRepository;
            _leagueMatchesRepository = leagueMatchesRepository;
            _leagueRepository = leagueRepository;
            _pointDeductionsRepository = pointDeductionsRepository;
        }

        public List<LeagueTableRow> GetLeagueTable(int tier, string season)
        {
            var table = new List<LeagueTableRow>();

            var leagueMatchDetails = _leagueMatchesRepository.GetLeagueMatches(tier, season);
            var leagueDetail = _leagueRepository.GetLeagueInfo(tier, season);
            var pointDeductions = _pointDeductionsRepository.GetPointDeductions(tier, season);
            var playOffMatches = _playOffMatchesRepository.GetPlayOffMatches(tier, season);

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
