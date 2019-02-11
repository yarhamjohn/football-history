using System.Collections.Generic;
using FootballHistory.Api.Models.Controller;

namespace FootballHistory.Api.Builders
{
    public interface ILeagueSeasonBuilder
    {
        List<LeagueTableRow> GetLeagueTable(int tier, string season);
    }
}
