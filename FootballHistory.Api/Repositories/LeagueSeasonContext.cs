using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Models
{
    public class LeagueSeasonContext : DbContext
    {
        public LeagueSeasonContext(DbContextOptions<LeagueSeasonContext> options) : base(options)
        {
        }
    }
}