using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Repositories.DivisionRepository;

namespace FootballHistory.Api.LeagueSeason.LeagueSeasonFilter
{
    public class LeagueSeasonFilterBuilder : ILeagueSeasonFilterBuilder
    {
        private List<DivisionModel> _divisionModels;
        
        public LeagueSeasonFilter Build(List<DivisionModel> divisionModels)
        {
            _divisionModels = divisionModels;
            
            return new LeagueSeasonFilter
            {
                AllSeasons = GetSeasons(),
                AllTiers = GetTiers()
            };
        }

        private List<string> GetSeasons()
        {
            var seasons = new HashSet<string>();
            foreach (var divisionModel in _divisionModels)
            {
                for (var year = divisionModel.From; year < divisionModel.To; year++)
                {
                    seasons.Add($"{year} - {year + 1}");
                }
            }

            return seasons.ToList();
        }

        private List<Tier> GetTiers()
        {
            return _divisionModels.GroupBy(model => model.Tier).Select(group => new Tier
                {
                    Divisions = group.Select(d => new Division
                        {
                            Name = d.Name,
                            ActiveFrom = d.From,
                            ActiveTo = d.To
                        }
                    ).ToList(),
                    Level = group.Key
                }
            ).ToList();
        }
    }
}