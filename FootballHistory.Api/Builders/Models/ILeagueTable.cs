using System.Collections.Generic;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders.Models
{
    public interface ILeagueTable
    {
        List<LeagueTableRow> GetTable();
        void AddLeagueRows(List<MatchDetailModel> leagueMatchDetails);
        void IncludePointDeductions(List<PointDeductionModel> pointDeductions);
        void SetLeaguePosition();
        void SortLeagueTable();
        void AddTeamStatus(LeagueDetailModel leagueDetailModel, IEnumerable<MatchDetailModel> playOffMatchDetails);
        void AddMissingTeams(List<string> missingTeams);
        void RemoveRows();
    }
}
