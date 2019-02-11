using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Domain
{
    public class LeagueRepositoryContext : DbContext
    {
        public LeagueRepositoryContext(DbContextOptions<LeagueRepositoryContext> options) : base(options)
        {
        }
    }
    
    public class PlayOffMatchesRepositoryContext : DbContext
    {
        public PlayOffMatchesRepositoryContext(DbContextOptions<PlayOffMatchesRepositoryContext> options) : base(options)
        {
        }
    }
        
    public class LeagueMatchesRepositoryContext : DbContext
    {
        public LeagueMatchesRepositoryContext(DbContextOptions<LeagueMatchesRepositoryContext> options) : base(options)
        {
        }
    }
                
    public class PointDeductionsRepositoryContext : DbContext
    {
        public PointDeductionsRepositoryContext(DbContextOptions<PointDeductionsRepositoryContext> options) : base(options)
        {
        }
    }            
    
    public class LeagueFormRepositoryContext : DbContext
    {
        public LeagueFormRepositoryContext(DbContextOptions<LeagueFormRepositoryContext> options) : base(options)
        {
        }
    }    
    
    public class DivisionRepositoryContext : DbContext
    {
        public DivisionRepositoryContext(DbContextOptions<DivisionRepositoryContext> options) : base(options)
        {
        }
    }
}
