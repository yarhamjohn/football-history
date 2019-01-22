using System.Collections.Generic;
using System.Linq;

namespace FootballHistory.Server.Models
{
    public interface ILeagueSeasonFilterBuilder
    {
        LeagueSeasonFilter Build(List<DivisionModel> divisionModels);
    }
    
    public class LeagueSeasonFilterBuilder : ILeagueSeasonFilterBuilder
    {
        public LeagueSeasonFilter Build(List<DivisionModel> divisionModels)
        {
            var leagueSeasonFilter = new LeagueSeasonFilter
            {
                AllSeasons = new List<string>(),
                AllTiers = new List<Tier>
                {
                    new Tier
                    {
                        Divisions = new List<Division>
                        {
                            new Division
                            {
                                Name = divisionModels.First().Name,
                                ActiveFrom = divisionModels.First().From,
                                ActiveTo = divisionModels.First().To
                            }
                        },
                        Level = divisionModels.First().Tier
                    }

                }
            };
            
            var from = divisionModels.First().From;
            var to = divisionModels.First().To;
            for (var year = from; year < to; year++)
            {
                leagueSeasonFilter.AllSeasons.Add($"{year}-{year+1}");
            }

            return leagueSeasonFilter;
        }    
    }
    
    public class LeagueSeasonFilter
    {
        public List<string> AllSeasons;
        public List<Tier> AllTiers;
    }
}
