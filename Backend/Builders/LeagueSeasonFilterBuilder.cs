using System.Collections.Generic;
using System.Linq;
using Backend.Builders.Models;
using Backend.Domain.Models;
using Backend.Models.LeagueSeason;

namespace Backend.Builders
{
    public class LeagueSeasonFilterBuilder : ILeagueSeasonFilterBuilder
    {
        public LeagueSeasonFilter Build(List<DivisionModel> divisionModels)
        {
            return new LeagueSeasonFilter
            {
                AllSeasons = GetSeasons(divisionModels),
                AllTiers = GetTiers(divisionModels)
            };
        }

        private static List<string> GetSeasons(IEnumerable<DivisionModel> divisionModels)
        {
            var seasons = new HashSet<string>();
            foreach (var divisionModel in divisionModels)
            {
                for (var year = divisionModel.From; year < divisionModel.To; year++)
                {
                    seasons.Add($"{year} - {year+1}");
                }
            }

            return seasons.ToList();
        }

        private static List<Tier> GetTiers(IEnumerable<DivisionModel> divisionModels)
        {
            return divisionModels.GroupBy(model => model.Tier).Select(group => new Tier
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