using System.Collections.Generic;
using FootballHistory.Api.Models.Controller;

namespace FootballHistory.Api.Repositories
{
    public interface ILeagueSeasonRepository
    {
        List<LeagueTableRow> GetLeagueTable(int tier, string season);
        LeagueRowDrillDown GetDrillDown(int tier, string season, string team);
    }
}
