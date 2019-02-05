using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Models.ControllerModels;
using FootballHistory.Api.Models.DatabaseModels;

namespace FootballHistory.Api.Builders.LeagueSeasonFilterBuilder
{
    public class LeagueSeasonFilterBuilder : ILeagueSeasonFilterBuilder
    {
        public LeagueSeasonFilter Build(List<DivisionModel> divisions)
        {
            return new LeagueSeasonFilter
            {
                AllSeasons = GetSeasons(divisions),
                AllTiers = GetTiers(divisions)
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