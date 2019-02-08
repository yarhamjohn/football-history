using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Domain
{
    public class LeagueSeasonContext : DbContext
    {
        public LeagueSeasonContext(DbContextOptions<LeagueSeasonContext> options) : base(options)
        {
        }
    }
    
    public class PlayOffMatchesContext : DbContext
    {
        public PlayOffMatchesContext(DbContextOptions<PlayOffMatchesContext> options) : base(options)
        {
        }
    }
}
