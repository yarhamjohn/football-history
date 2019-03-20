using FootballHistory.Api.Repositories.LeagueDetailRepository;

namespace FootballHistory.Api.LeagueSeason.Table
{
    public interface ILeagueTableSorter
    {
        LeagueTable Sort(LeagueTable leagueTable, LeagueDetailModel leagueDetailModel);
    }
}