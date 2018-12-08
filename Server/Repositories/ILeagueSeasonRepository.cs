using System.Collections.Generic;
using static football_history.Server.Repositories.LeagueSeasonRepository;

namespace football_history.Server.Repositories
{
    public interface ILeagueSeasonRepository
    {
        DefaultFilter GetDefaultFilter();
        FilterOptions GetFilterOptions();
        List<ResultMatrixRow> GetResultMatrix(int tier, string season);
        PlayOffs GetPlayOffMatches(int tier, string season);
        List<LeagueTableRow> GetLeagueTable(int tier, string season);
        LeagueRowDrillDown GetDrillDown(int tier, string season, string team);
    }
}