using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Domain;
using FootballHistoryTest.Api.Repositories.Season;
using Microsoft.EntityFrameworkCore;

namespace FootballHistoryTest.Api.Builders
{
    public interface ISeasonBuilder
    {
        List<Season> GetSeasons();
    }
    
    public class SeasonBuilder : ISeasonBuilder
    {
        private readonly DatabaseContext _context;
        private readonly ISeasonRepository _seasonRepository;

        public SeasonBuilder(DatabaseContext context, ISeasonRepository seasonRepository)
        {
            _context = context;
            _seasonRepository = seasonRepository;
        }
            
        public List<Season> GetSeasons()
        {
            using var conn = _context.Database.GetDbConnection();

            var seasonModels = _seasonRepository.GetSeasonModels(conn);
            return seasonModels.GroupBy(s => s.SeasonStartYear,
                (startYear, models) => new Season
                {
                    StartYear = startYear, EndYear = startYear + 1,
                    Divisions = models.Select(m => new Division {Name = m.Name, Tier = m.Tier}).ToList()
                }).ToList();
        }
    }

    public class Season
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public List<Division> Divisions { get; set; }
    }

    public class Division
    {
        public string Name { get; set; }
        public int Tier { get; set; }
    }
}