using System.Collections.Generic;
using FootballHistory.Api.Models.LeagueSeason;

namespace FootballHistory.Api.Repositories
{
    public interface ILeagueSeasonRepository
    {
        PlayOffs GetPlayOffMatches(int tier, string season);
        List<LeagueTableRow> GetLeagueTable(int tier, string season);
        LeagueRowDrillDown GetDrillDown(int tier, string season, string team);
    }
}
