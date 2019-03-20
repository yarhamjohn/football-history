using FootballHistory.Api.Repositories.LeagueDetailRepository;

namespace FootballHistory.Api.LeagueSeason.LeagueTable
{
    public interface ILeagueTableSorter
    {
        LeagueTable Sort(LeagueTable leagueTable, LeagueDetailModel leagueDetailModel);
    }
}