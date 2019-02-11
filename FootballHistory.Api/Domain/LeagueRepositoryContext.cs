using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Domain
{
    public class LeagueRepositoryContext : DbContext
    {
        public LeagueRepositoryContext(DbContextOptions<LeagueRepositoryContext> options) : base(options)
        {
        }
    }
    
    public class PlayOffMatchesContext : DbContext
    {
        public PlayOffMatchesContext(DbContextOptions<PlayOffMatchesContext> options) : base(options)
        {
        }
    }
        
    public class LeagueMatchesContext : DbContext
    {
        public LeagueMatchesContext(DbContextOptions<LeagueMatchesContext> options) : base(options)
        {
        }
    }
                
    public class PointDeductionsContext : DbContext
    {
        public PointDeductionsContext(DbContextOptions<PointDeductionsContext> options) : base(options)
        {
        }
    }
}
