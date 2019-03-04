using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.LeagueSeason.LeagueSeasonFilter;
using FootballHistory.Api.Repositories.DivisionRepository;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.LeagueSeason.LeagueSeasonFilter
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
            var divisionModel = new DivisionModel {Name = "DivisionName", From = 2012, To = 2014};

            var leagueSeasonFilter = _leagueSeasonFilterBuilder.Build(new List<DivisionModel> { divisionModel });

            var divisions = leagueSeasonFilter
                .AllTiers.Single()
                .Divisions.Select(d => (d.Name, d.ActiveFrom, d.ActiveTo))
                .ToArray();

            Assert.AreEqual(new[] { ("DivisionName", 2012, 2014) }, divisions);
        }
        
        [Test]
        public void GetCorrectDivisions_GivenTwoDivisionModels_WithMatchingTiers()
        {
            var divisionModelOne = new DivisionModel {Name = "DivisionName", Tier = 1, From = 2012, To = 2014};
            var divisionModelTwo = new DivisionModel {Name = "AnotherDivisionName", Tier = 1, From = 2014, To = 2016};

            var leagueSeasonFilter = _leagueSeasonFilterBuilder.Build(new List<DivisionModel> { divisionModelOne, divisionModelTwo });

            var divisions = leagueSeasonFilter
                .AllTiers.Single().Divisions
                .Select(d => (d.Name, d.ActiveFrom, d.ActiveTo))
                .ToArray();
            
            Assert.AreEqual(new[] { ("DivisionName", 2012, 2014), ("AnotherDivisionName", 2014, 2016) }, divisions);
        }
                
        [Test]
        public void GetCorrectDivisions_GivenTwoDivisionModels_WithNonMatchingTiers()
        {
            var divisionModelOne = new DivisionModel {Name = "DivisionName", Tier = 1, From = 2012, To = 2014};
            var divisionModelTwo = new DivisionModel {Name = "AnotherDivisionName", Tier = 2, From = 2013, To = 2015};

            var leagueSeasonFilter = _leagueSeasonFilterBuilder.Build(new List<DivisionModel> { divisionModelOne, divisionModelTwo });

            var firstTierDivisions = leagueSeasonFilter
                .AllTiers.First().Divisions
                .Select(d => (d.Name, d.ActiveFrom, d.ActiveTo))
                .ToArray();

            var secondTierDivisions = leagueSeasonFilter
                .AllTiers.Last()
                .Divisions.Select(d => (d.Name, d.ActiveFrom, d.ActiveTo))
                .ToArray();

            Assert.AreEqual(new[] { ("DivisionName", 2012, 2014) }, firstTierDivisions);
            Assert.AreEqual(new[] { ("AnotherDivisionName", 2013, 2015) }, secondTierDivisions);
        }
    }
}
