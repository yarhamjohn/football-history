using System.Collections.Generic;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Repositories
{
    public interface ILeagueSeasonRepository
    {
        List<LeagueTableRow> GetLeagueTable(int tier, string season, List<MatchDetailModel> playOffMatches);
    }
}
