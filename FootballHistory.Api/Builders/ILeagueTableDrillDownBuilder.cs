using FootballHistory.Api.Models.Controller;

namespace FootballHistory.Api.Builders
{
    public interface ILeagueTableDrillDownBuilder
    {
        LeagueRowDrillDown GetDrillDown(int tier, string season, string team);
    }
}
