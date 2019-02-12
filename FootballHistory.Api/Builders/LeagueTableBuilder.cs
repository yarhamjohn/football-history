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
        private readonly ILeagueTable _leagueTable;

        public LeagueTableBuilder(ILeagueTable leagueTable)
        {
            _leagueTable = leagueTable;
        }
        
        public List<LeagueTableRow> Build(
            List<MatchDetailModel> leagueMatchDetails, 
            LeagueDetailModel leagueDetail, 
            List<PointDeductionModel> pointDeductions, 
            List<MatchDetailModel> playOffMatches)
        {
            _leagueTable.AddLeagueRows(leagueMatchDetails);
            _leagueTable.IncludePointDeductions(pointDeductions);
            _leagueTable.SortLeagueTable();
            _leagueTable.SetLeaguePosition();
            _leagueTable.AddTeamStatus(leagueDetail, playOffMatches);            

            return _leagueTable.GetTable();
        }
    }
}
