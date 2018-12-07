using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

public class LeagueSeasonContext : DbContext
{
    public LeagueSeasonContext(DbContextOptions<LeagueSeasonContext> options) : base(options)
    {
    }
}