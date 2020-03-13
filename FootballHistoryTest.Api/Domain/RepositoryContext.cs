using Microsoft.EntityFrameworkCore;

namespace FootballHistoryTest.Api.Domain
{
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
    
    public class SeasonRepositoryContext : DbContext
    {
        public SeasonRepositoryContext(DbContextOptions<SeasonRepositoryContext> options) : base(options)
        {
        }
    }

    public class LeagueRepositoryContext : DbContext
    {
        public LeagueRepositoryContext(DbContextOptions<LeagueRepositoryContext> options) : base(options)
        {
        }
    }

    public class MatchRepositoryContext : DbContext
    {
        public MatchRepositoryContext(DbContextOptions<MatchRepositoryContext> options) : base(options)
        {
        }
    }
    
    public class PlayOffMatchRepositoryContext : DbContext
    {
        public PlayOffMatchRepositoryContext(DbContextOptions<PlayOffMatchRepositoryContext> options) : base(options)
        {
        }
    }

    public class PointsDeductionRepositoryContext : DbContext
    {
        public PointsDeductionRepositoryContext(DbContextOptions<PointsDeductionRepositoryContext> options) : base(options)
        {
        }
    }
}
