using System.Collections.Generic;
using FootballHistory.Api.Models.Controller;

namespace FootballHistory.Api.Repositories
{
    public interface ILeagueTableDrillDownRepository
    {
        LeagueRowDrillDown GetDrillDown(int tier, string season, string team);
    }
}
