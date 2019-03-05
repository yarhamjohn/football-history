using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Domain
{
    public class LeagueDetailRepositoryContext : DbContext
    {
        public LeagueDetailRepositoryContext(DbContextOptions<LeagueDetailRepositoryContext> options) : base(options)
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
    
    public class DivisionRepositoryContext : DbContext
    {
        public DivisionRepositoryContext(DbContextOptions<DivisionRepositoryContext> options) : base(options)
        {
        }
    }    
    
    public class TeamRepositoryContext : DbContext
    {
        public TeamRepositoryContext(DbContextOptions<TeamRepositoryContext> options) : base(options)
        {
        }
    }
}
