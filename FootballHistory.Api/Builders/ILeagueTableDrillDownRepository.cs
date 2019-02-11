using FootballHistory.Api.Models.Controller;

namespace FootballHistory.Api.Builders
{
    public interface ILeagueTableDrillDownRepository
    {
        LeagueRowDrillDown GetDrillDown(int tier, string season, string team);
    }
}
