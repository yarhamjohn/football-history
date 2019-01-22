using System.Collections.Generic;
using System.Linq;
using FootballHistory.Server.Models;
using NUnit.Framework;

namespace FootballHistoryUnitTests.Server.Models
{
    public class LeagueSeasonFilterBuilderTests
    {
        private ILeagueSeasonFilterBuilder LeagueSeasonFilterBuilder;

        [Test]
        public void GetLeagueSeasonFilters_Returns_CorrectFilter_Given_OneDivisionAndOneTierAndOneSeason()
        {
            LeagueSeasonFilterBuilder = new LeagueSeasonFilterBuilder();
            var fakeDivisionModels = new List<DivisionModel>
            {
                new DivisionModel
                {
                    Name = "DivisionName",
                    Tier = 1,
                    From = 2015,
                    To = 2016
                }
            };
            var filters = LeagueSeasonFilterBuilder.Build(fakeDivisionModels);

            var expectedFilter = new LeagueSeasonFilter
            {
                AllSeasons = new List<string> {"2015-2016"},
                AllTiers = new List<Tier>
                {
                    new Tier
                    {
                        Divisions = new List<Division>
                        {
                            new Division
                            {
                                Name = "DivisionName",
                                ActiveFrom = 2015,
                                ActiveTo = 2016
                            }
                        },
                        Level = 1
                    }
                }
            };

            AssertFiltersAreEqual(expectedFilter, filters);
        }
        
        
        [Test]
        public void GetLeagueSeasonFilters_Returns_CorrectFilter_Given_OneDivisionAndOneTierAndMultipleSeasons()
        {
            LeagueSeasonFilterBuilder = new LeagueSeasonFilterBuilder();
            var fakeDivisionModels = new List<DivisionModel>
            {
                new DivisionModel
                {
                    Name = "DivisionName",
                    Tier = 1,
                    From = 2015,
                    To = 2017
                }
            };
            var filters = LeagueSeasonFilterBuilder.Build(fakeDivisionModels);

            var expectedFilter = new LeagueSeasonFilter
            {
                AllSeasons = new List<string> {"2015-2016", "2016-2017"},
                AllTiers = new List<Tier>
                {
                    new Tier
                    {
                        Divisions = new List<Division>
                        {
                            new Division
                            {
                                Name = "DivisionName",
                                ActiveFrom = 2015,
                                ActiveTo = 2017
                            }
                        },
                        Level = 1
                    }
                }
            };

            AssertFiltersAreEqual(expectedFilter, filters);

        }

        [Test]
        public void GetLeagueSeasonFilters_Returns_CorrectFilter_Given_OneDivisionAndOneTierAndNoEndSeason()
        {
            
        }
        
        private void AssertFiltersAreEqual(LeagueSeasonFilter expectedFilter, LeagueSeasonFilter actualFilter)
        {
            Assert.AreEqual(expectedFilter.AllSeasons, actualFilter.AllSeasons);
            Assert.AreEqual(expectedFilter.AllTiers.SelectMany(t => t.Divisions).Select(d => d.Name), actualFilter.AllTiers.SelectMany(t => t.Divisions).Select(d => d.Name));
            Assert.AreEqual(expectedFilter.AllTiers.SelectMany(t => t.Divisions).Select(d => d.ActiveFrom), actualFilter.AllTiers.SelectMany(t => t.Divisions).Select(d => d.ActiveFrom));
            Assert.AreEqual(expectedFilter.AllTiers.SelectMany(t => t.Divisions).Select(d => d.ActiveTo), actualFilter.AllTiers.SelectMany(t => t.Divisions).Select(d => d.ActiveTo));
            Assert.AreEqual(expectedFilter.AllTiers.Select(t => t.Level).ToList(), actualFilter.AllTiers.Select(t => t.Level).ToList());
        }
    }
}
