using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

public class FootballHistoryContext : DbContext
{
    public FootballHistoryContext(DbContextOptions<FootballHistoryContext> options) : base(options)
    {
    }
}