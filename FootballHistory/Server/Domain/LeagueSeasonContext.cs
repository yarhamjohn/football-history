using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Server.Domain
{
    public class LeagueSeasonContext : DbContext
    {
        public LeagueSeasonContext(DbContextOptions<LeagueSeasonContext> options) : base(options)
        {
        }
    }
}