using System.Collections.Generic;
using FootballHistory.Api.Models.Controller;

namespace FootballHistory.Api.Repositories
{
    public interface ILeagueRepository
    {
        List<LeagueTableRow> GetLeagueTable(int tier, string season);
    }
}
