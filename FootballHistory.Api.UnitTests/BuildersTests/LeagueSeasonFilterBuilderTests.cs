using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Builders;
using FootballHistory.Api.Repositories.Models;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.BuildersTests
{
    public class LeagueSeasonFilterBuilderTests
    {
        private ILeagueSeasonFilterBuilder _leagueSeasonFilterBuilder;

        [SetUp]
        public void SetUp()
        {
            _leagueSeasonFilterBuilder = new LeagueSeasonFilterBuilder();
        }

        [Test]
        public void GetCorrectSeasonFilter_GivenOneDivisionModel_WithOneSeason()
        {
            var divisionModel = new DivisionModel {From = 2015, To = 2016};
            
            var leagueSeasonFilter = _leagueSeasonFilterBuilder.Build(new List<DivisionModel> { divisionModel });

            Assert.AreEqual(new List<string> {"2015 - 2016"}, leagueSeasonFilter.AllSeasons);
        }
        
        [Test]
        public void GetCorrectSeasonFilter_GivenOneDivisionModel_WithMultipleSeasons()
        {
            var divisionModel = new DivisionModel {From = 2015, To = 2017};
            
            var leagueSeasonFilter = _leagueSeasonFilterBuilder.Build(new List<DivisionModel> { divisionModel });

            Assert.AreEqual(new List<string> {"2015 - 2016", "2016 - 2017"}, leagueSeasonFilter.AllSeasons);
        }
        
        [Test]
        public void GetCorrectSeasonFilter_GivenTwoDivisionModels_WithOneMatchingSeason()
        {
            var divisionModelOne = new DivisionModel {From = 2015, To = 2016};
            var divisionModelTwo = new DivisionModel {From = 2015, To = 2016};
            
            var leagueSeasonFilter = _leagueSeasonFilterBuilder.Build(new List<DivisionModel> { divisionModelOne, divisionModelTwo });

            Assert.AreEqual(new List<string> {"2015 - 2016"}, leagueSeasonFilter.AllSeasons);
        }
        
        [Test]
        public void GetCorrectSeasonFilter_GivenTwoDivisionModels_WithMultipleNonMatchingSeasons()
        {
            var divisionModelOne = new DivisionModel {From = 2015, To = 2017};
            var divisionModelTwo = new DivisionModel {From = 2017, To = 2019};
            
            var leagueSeasonFilter = _leagueSeasonFilterBuilder.Build(new List<DivisionModel> { divisionModelOne, divisionModelTwo });

            Assert.AreEqual(new List<string> {"2015 - 2016", "2016 - 2017", "2017 - 2018", "2018 - 2019"}, leagueSeasonFilter.AllSeasons);
        }
                
        [Test]
        public void GetCorrectSeasonFilter_GivenTwoDivisionModels_WithMultipleMatchingSeasons()
        {
            var divisionModelOne = new DivisionModel {From = 2015, To = 2017};
            var divisionModelTwo = new DivisionModel {From = 2015, To = 2017};
            
            var leagueSeasonFilter = _leagueSeasonFilterBuilder.Build(new List<DivisionModel> { divisionModelOne, divisionModelTwo });

            Assert.AreEqual(new List<string> {"2015 - 2016", "2016 - 2017"}, leagueSeasonFilter.AllSeasons);
        }
                
        [Test]
        public void GetCorrectSeasonFilter_GivenTwoDivisionModels_WithMultipleOverlappingSeasons()
        {
            var divisionModelOne = new DivisionModel {From = 2015, To = 2017};
            var divisionModelTwo = new DivisionModel {From = 2016, To = 2018};
            
            var leagueSeasonFilter = _leagueSeasonFilterBuilder.Build(new List<DivisionModel> { divisionModelOne, divisionModelTwo });

            Assert.AreEqual(new List<string> {"2015 - 2016", "2016 - 2017", "2017 - 2018"}, leagueSeasonFilter.AllSeasons);
        }

        [Test]
        public void GetCorrectLevels_GivenOneDivisionModel()
        {
            var divisionModel = new DivisionModel {Tier = 1};

            var leagueSeasonFilter = _leagueSeasonFilterBuilder.Build(new List<DivisionModel> {divisionModel});
            var levels = leagueSeasonFilter.AllTiers.Select(t => t.Level).ToArray();
            
            Assert.AreEqual(new[] { 1 }, levels);
        }

        [Test]
        public void GetCorrectLevels_GivenTwoDivisionModels_WithNonMatchingTiers()
        {
            var divisionModelOne = new DivisionModel {Tier = 1};
            var divisionModelTwo = new DivisionModel {Tier = 2};

            var leagueSeasonFilter = _leagueSeasonFilterBuilder.Build(new List<DivisionModel> { divisionModelOne, divisionModelTwo });
            var levels = leagueSeasonFilter.AllTiers.Select(t => t.Level).ToArray();
            
            Assert.AreEqual(new[] { 1, 2 }, levels);
        }

        [Test]
        public void GetCorrectLevels_GivenTwoDivisionModels_WithMatchingTiers()
        {
            var divisionModelOne = new DivisionModel {Tier = 1};
            var divisionModelTwo = new DivisionModel {Tier = 1};

            var leagueSeasonFilter = _leagueSeasonFilterBuilder.Build(new List<DivisionModel> { divisionModelOne, divisionModelTwo });
            var levels = leagueSeasonFilter.AllTiers.Select(t => t.Level).ToArray();
            
            Assert.AreEqual(new[] { 1 }, levels);
        }
        
        [Test]
        public void GetCorrectDivisions_GivenOneDivisionModel()
        {
            var divisionModel = new DivisionModel {Name = "DivisionName"};

            var leagueSeasonFilter = _leagueSeasonFilterBuilder.Build(new List<DivisionModel> { divisionModel });
            var divisionNames = leagueSeasonFilter.AllTiers.SelectMany(t => t.Divisions).Select(d => d.Name).ToArray();
            
            Assert.AreEqual(new[] { "DivisionName" }, divisionNames);
        }
        
        [Test]
        public void GetCorrectDivisions_GivenTwoDivisionModels_WithMatchingTiers()
        {
            var divisionModelOne = new DivisionModel {Name = "DivisionName", Tier = 1};
            var divisionModelTwo = new DivisionModel {Name = "AnotherDivisionName", Tier = 1};

            var leagueSeasonFilter = _leagueSeasonFilterBuilder.Build(new List<DivisionModel> { divisionModelOne, divisionModelTwo });
            var tier = leagueSeasonFilter.AllTiers.Single();
            var divisionNames = tier.Divisions.Select(d => d.Name).ToArray();
            
            Assert.AreEqual(new[] { "DivisionName", "AnotherDivisionName" }, divisionNames);
        }
                
        [Test]
        public void GetCorrectDivisions_GivenTwoDivisionModels_WithNonMatchingTiers()
        {
            var divisionModelOne = new DivisionModel {Name = "DivisionName", Tier = 1};
            var divisionModelTwo = new DivisionModel {Name = "AnotherDivisionName", Tier = 2};

            var leagueSeasonFilter = _leagueSeasonFilterBuilder.Build(new List<DivisionModel> { divisionModelOne, divisionModelTwo });
            var firstTier = leagueSeasonFilter.AllTiers.First();
            var firstTierDivisionNames = firstTier.Divisions.Select(d => d.Name).ToArray();
            var secondTier = leagueSeasonFilter.AllTiers.Last();
            var secondTierDivisionNames = secondTier.Divisions.Select(d => d.Name).ToArray();

            Assert.AreEqual(new[] { "DivisionName" }, firstTierDivisionNames);
            Assert.AreEqual(new[] { "AnotherDivisionName" }, secondTierDivisionNames);
        }
    }
}
