using FootballHistory.Api.Builders.Models;

namespace FootballHistory.Api.Builders
{
    public interface ILeagueTableDrillDownBuilder
    {
        LeagueRowDrillDown GetDrillDown(int tier, string season, string team);
    }
}
