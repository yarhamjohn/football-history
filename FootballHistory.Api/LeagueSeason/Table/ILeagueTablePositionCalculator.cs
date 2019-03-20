using FootballHistory.Api.Repositories.LeagueDetailRepository;

namespace FootballHistory.Api.LeagueSeason.Table
{
    public interface ILeagueTablePositionCalculator
    {
        LeagueTable AddPositions(LeagueTable leagueTable, LeagueDetailModel leagueDetailModel);
    }
}