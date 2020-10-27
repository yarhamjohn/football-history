using Microsoft.EntityFrameworkCore;

namespace football.history.api.Domain
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options) {}
    }
}
