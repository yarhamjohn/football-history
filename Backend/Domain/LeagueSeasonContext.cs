using Microsoft.EntityFrameworkCore;

namespace Backend.Domain
{
    public class LeagueSeasonContext : DbContext
    {
        public LeagueSeasonContext(DbContextOptions<LeagueSeasonContext> options) : base(options)
        {
        }
    }
}