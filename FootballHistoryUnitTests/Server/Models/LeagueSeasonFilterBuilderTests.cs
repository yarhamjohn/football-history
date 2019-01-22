using System.Collections.Generic;
using System.Linq;
using FootballHistory.Server.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace FootballHistoryUnitTests.Server.Models
{
    public class LeagueSeasonFilterBuilderTests
    {
        private ILeagueSeasonFilterBuilder LeagueSeasonFilterBuilder;

        [Test]
        public void BuildCorrectFilter_GivenOneDivisionModel_WithOneSeason()
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
                AllTiers = new List<Tier> { GetOneTier((2015, 2016)) }
            };

            AssertSeasonsAreEqual(expectedFilter, filters);
            AssertTiersAreEqual(expectedFilter, filters);
        }
        
        [Test]
        public void BuildCorrectFilter_GivenOneDivisionModel_WithMultipleSeasons()
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
                AllTiers = new List<Tier> { GetOneTier((2015, 2017)) }
            };

            AssertSeasonsAreEqual(expectedFilter, filters);
            AssertTiersAreEqual(expectedFilter, filters);
        }

        [Test]
        public void BuildCorrectFilter_GivenTwoDivisionModels_InTheSameTier()
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
                },
                new DivisionModel
                {
                    Name = "DivisionNameTwo",
                    Tier = 1,
                    From = 2016,
                    To = 2018
                }
            };
            var filters = LeagueSeasonFilterBuilder.Build(fakeDivisionModels);

            var expectedFilter = new LeagueSeasonFilter
            {
                AllSeasons = new List<string> {"2015-2016", "2016-2017", "2017-2018"},
                AllTiers = new List<Tier> { GetOneTierWithMultipleDivisions((2015, 2016), (2016, 2018)) }
            };

            AssertSeasonsAreEqual(expectedFilter, filters);
            AssertTiersAreEqual(expectedFilter, filters);        
        }

        [Test]
        public void BuildCorrectFilter_GivenTwoDivisionModels_WithOneMatchingSeason_InDifferentTiers()
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
                },
                new DivisionModel
                {
                    Name = "DivisionNameTwo",
                    Tier = 2,
                    From = 2015,
                    To = 2016
                }
            };
            var filters = LeagueSeasonFilterBuilder.Build(fakeDivisionModels);

            var expectedFilter = new LeagueSeasonFilter
            {
                AllSeasons = new List<string> {"2015-2016"},
                AllTiers = GetTwoTiers((2015, 2016), (2015, 2016))
            };

            AssertSeasonsAreEqual(expectedFilter, filters);
            AssertTiersAreEqual(expectedFilter, filters);
        }
        
        [Test]
        public void BuildCorrectFilter_GivenTwoDivisionModels_WithOneNonMatchingSeason_InDifferentTiers()
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
                },
                new DivisionModel
                {
                    Name = "DivisionNameTwo",
                    Tier = 2,
                    From = 2016,
                    To = 2017
                }
            };
            var filters = LeagueSeasonFilterBuilder.Build(fakeDivisionModels);

            var expectedFilter = new LeagueSeasonFilter
            {
                AllSeasons = new List<string> {"2015-2016", "2016-2017"},
                AllTiers = GetTwoTiers((2015, 2016), (2016, 2017))
            };

            AssertSeasonsAreEqual(expectedFilter, filters);
            AssertTiersAreEqual(expectedFilter, filters);
        }
        
        [Test]
        public void BuildCorrectFilter_GivenTwoDivisionModels_WithMultipleMatchingSeasons_InDifferentTiers()
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
                },
                new DivisionModel
                {
                    Name = "DivisionNameTwo",
                    Tier = 2,
                    From = 2015,
                    To = 2017
                }
            };
            var filters = LeagueSeasonFilterBuilder.Build(fakeDivisionModels);

            var expectedFilter = new LeagueSeasonFilter
            {
                AllSeasons = new List<string> {"2015-2016", "2016-2017"},
                AllTiers = GetTwoTiers((2015, 2017), (2015, 2017))
            };

            AssertSeasonsAreEqual(expectedFilter, filters);
            AssertTiersAreEqual(expectedFilter, filters);
        }
        
        [Test]
        public void BuildCorrectFilter_GivenTwoDivisionModels_WithMultipleNonMatchingSeasons_InDifferentTiers()
        {
            LeagueSeasonFilterBuilder = new LeagueSeasonFilterBuilder();
            var fakeDivisionModels = new List<DivisionModel>
            {
                new DivisionModel
                {
                    Name = "DivisionName",
                    Tier = 1,
                    From = 2014,
                    To = 2016
                },
                new DivisionModel
                {
                    Name = "DivisionNameTwo",
                    Tier = 2,
                    From = 2017,
                    To = 2019
                }
            };
            var filters = LeagueSeasonFilterBuilder.Build(fakeDivisionModels);

            var expectedFilter = new LeagueSeasonFilter
            {
                AllSeasons = new List<string> {"2014-2015", "2015-2016", "2017-2018", "2018-2019"},
                AllTiers = GetTwoTiers((2014, 2016), (2017, 2019))
            };

            AssertSeasonsAreEqual(expectedFilter, filters);
            AssertTiersAreEqual(expectedFilter, filters);
        }
        
        [Test]
        public void BuildCorrectFilter_GivenTwoDivisionModels_WithMultipleOverlappingSeasons_InDifferentTiers()
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
                },
                new DivisionModel
                {
                    Name = "DivisionNameTwo",
                    Tier = 2,
                    From = 2016,
                    To = 2018
                }
            };
            var filters = LeagueSeasonFilterBuilder.Build(fakeDivisionModels);

            var expectedFilter = new LeagueSeasonFilter
            {
                AllSeasons = new List<string> {"2015-2016", "2016-2017", "2017-2018"},
                AllTiers = GetTwoTiers((2015, 2017), (2016, 2018))
            };

            AssertSeasonsAreEqual(expectedFilter, filters);
            AssertTiersAreEqual(expectedFilter, filters);
        }

        private Tier GetOneTier((int from, int to) seasons)
        {
            return new Tier
            {
                Divisions = new List<Division>
                {
                    new Division
                    {
                        Name = "DivisionName",
                        ActiveFrom = seasons.from,
                        ActiveTo = seasons.to
                    }
                },
                Level = 1
            };
        }
        
        private Tier GetOneTierWithMultipleDivisions((int from, int to) divisionOneSeason, (int from, int to) divisionTwoSeason)
        {
            return new Tier
            {
                Divisions = new List<Division>
                {
                    new Division
                    {
                        Name = "DivisionName",
                        ActiveFrom = divisionOneSeason.from,
                        ActiveTo = divisionOneSeason.to
                    },
                    new Division
                    {
                        Name = "DivisionNameTwo",
                        ActiveFrom = divisionTwoSeason.from,
                        ActiveTo = divisionTwoSeason.to
                    }
                },
                Level = 1
            };
        }
        
        private List<Tier> GetTwoTiers((int from, int to) tierOneSeasons, (int from, int to) tierTwoSeasons)
        {
            return new List<Tier>
            {
                GetOneTier(tierOneSeasons),
                new Tier
                {
                    Divisions = new List<Division>
                    {
                        new Division
                        {
                            Name = "DivisionNameTwo",
                            ActiveFrom = tierTwoSeasons.from,
                            ActiveTo = tierTwoSeasons.to
                        }
                    },
                    Level = 2
                }
            };
        }

        private void AssertSeasonsAreEqual(LeagueSeasonFilter expectedFilter, LeagueSeasonFilter actualFilter)
        {
            Assert.AreEqual(expectedFilter.AllSeasons, actualFilter.AllSeasons);
        }
        
        private void AssertTiersAreEqual(LeagueSeasonFilter expectedFilter, LeagueSeasonFilter actualFilter)
        {
            var expectedLevels = expectedFilter.AllTiers.Select(t => t.Level).ToList();
            var actualLevels = actualFilter.AllTiers.Select(t => t.Level).ToList();
            Assert.AreEqual(expectedLevels, actualLevels);
            
            var expectedDivisions = expectedFilter.AllTiers.SelectMany(t => t.Divisions).ToList();
            var actualDivisions = actualFilter.AllTiers.SelectMany(t => t.Divisions).ToList();
            Assert.IsTrue(DivisionsMatch(expectedDivisions, actualDivisions));
        }

        private bool DivisionsMatch(List<Division> expectedDivisions, List<Division> actualDivisions)
        {
            var expected = expectedDivisions.OrderBy(d => d.Name).ToList();
            var actual = actualDivisions.OrderBy(d => d.Name).ToList();

            var listsSameLength = expected.Count == actual.Count;
            var allElementsMatch = expected.Select((t, i) => t.Name == actual[i].Name && t.ActiveTo == actual[i].ActiveTo && t.ActiveFrom == actual[i].ActiveFrom).All(match => match);
            return listsSameLength && allElementsMatch;
        }
    }
}
