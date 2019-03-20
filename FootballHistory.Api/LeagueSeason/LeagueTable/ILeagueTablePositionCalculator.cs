using FootballHistory.Api.Repositories.LeagueDetailRepository;

namespace FootballHistory.Api.LeagueSeason.LeagueTable
{
    public interface ILeagueTablePositionCalculator
    {
        LeagueTable AddPositions(LeagueTable leagueTable, LeagueDetailModel leagueDetailModel);
    }
}