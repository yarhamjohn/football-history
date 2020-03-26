using Microsoft.EntityFrameworkCore;

namespace FootballHistoryTest.Api.Domain
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
    }
}
