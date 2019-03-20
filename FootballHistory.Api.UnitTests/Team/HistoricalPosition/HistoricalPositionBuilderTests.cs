using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Domain;
using FootballHistory.Api.LeagueSeason.Table;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using FootballHistory.Api.Repositories.PointDeductionRepository;
using FootballHistory.Api.Repositories.TierRepository;
using FootballHistory.Api.Team.HistoricalPosition;
using Moq;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.Team.HistoricalPosition
{
    [TestFixture]
    public class HistoricalPositionBuilderTests
    {
        private HistoricalPositionBuilder _historicalPositionBuilder;
        private Mock<ILeagueTableBuilder> _mockBuilder;

        [SetUp]
        public void Setup()
        {
            _mockBuilder = new Mock<ILeagueTableBuilder>();
            _mockBuilder
                .Setup(x => x.BuildWithStatuses(
                    It.IsAny<List<MatchDetailModel>>(),
                    It.IsAny<List<PointDeductionModel>>(),
                    It.IsAny<LeagueDetailModel>(),
                    It.IsAny<List<MatchDetailModel>>()))
                .Returns((List<MatchDetailModel> lmd, List<PointDeductionModel> pdm, LeagueDetailModel ldm, List<MatchDetailModel> pom) => new LeagueTable
                {
                    Rows = new List<LeagueTableRow>
                    {
                        new LeagueTableRow {Team = "Team1", Position = 1, Status = "C"},
                        new LeagueTableRow {Team = "Team2", Position = 2, Status = "R"}
                    }
                });

            _historicalPositionBuilder = new HistoricalPositionBuilder(_mockBuilder.Object);
        }

        [Test]
        public void Build_ReturnsEmptyHistoricalPositions_GivenNoFilter()
        {
            var filters = new SeasonTierFilter[0];
            var result = _historicalPositionBuilder.Build(
                "", 
                filters, 
                new List<MatchDetailModel>(), 
                new List<MatchDetailModel>(), 
                new List<PointDeductionModel>(), 
                new List<LeagueDetailModel>());
            
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Build_ReturnsHistoricalPositionWithAbsolutePositionOfZero_GivenAnUnknownTier()
        {
            var filters = new[] { new SeasonTierFilter { SeasonStartYear = 2000, Tier = Tier.UnknownTier}};
            var result = _historicalPositionBuilder.Build(
                "", 
                filters, 
                new List<MatchDetailModel>(), 
                new List<MatchDetailModel>(), 
                new List<PointDeductionModel>(), 
                new List<LeagueDetailModel>());
            
            Assert.That(result.Single().AbsolutePosition, Is.EqualTo(0));
            Assert.That(result.Single().Season, Is.EqualTo("2000 - 2001"));
            Assert.That(result.Single().Status, Is.EqualTo(""));
        }     
        
        [Test]
        public void Build_ReturnsHistoricalPositionWithAbsolutePositionOfZero_GivenAMissingTier()
        {
            var filters = new[] { new SeasonTierFilter { SeasonStartYear = 2000 }};
            var result = _historicalPositionBuilder.Build(
                "", 
                filters, 
                new List<MatchDetailModel>(), 
                new List<MatchDetailModel>(), 
                new List<PointDeductionModel>(), 
                new List<LeagueDetailModel>());
            
            Assert.That(result.Single().AbsolutePosition, Is.EqualTo(0));
            Assert.That(result.Single().Season, Is.EqualTo("2000 - 2001"));
            Assert.That(result.Single().Status, Is.EqualTo(""));
        }
        
        [Test]
        public void Build_ReturnsTwoHistoricalPositionsInCorrectOrder()
        {
            var filters = new[]
            {
                new SeasonTierFilter { SeasonStartYear = 2000 }, 
                new SeasonTierFilter { SeasonStartYear = 1995 }
            };
            var result = _historicalPositionBuilder.Build(
                "", 
                filters, 
                new List<MatchDetailModel>(), 
                new List<MatchDetailModel>(), 
                new List<PointDeductionModel>(), 
                new List<LeagueDetailModel>());
            
            Assert.That(result.First().Season, Is.EqualTo("1995 - 1996"));
            Assert.That(result.Last().Season, Is.EqualTo("2000 - 2001"));
        }

        [Test]
        public void Build_ShouldThrowAnException_GivenNoLeagueMatches_ForAGivenYear()
        {
            var filters = new[] { new SeasonTierFilter { SeasonStartYear = 2000, Tier = Tier.TopTier}};
            
            var ex = Assert.Throws<Exception>(() => _historicalPositionBuilder.Build(
                "", 
                filters, 
                new List<MatchDetailModel>(), 
                new List<MatchDetailModel>(), 
                new List<PointDeductionModel>(), 
                new List<LeagueDetailModel> { new LeagueDetailModel() }));
            Assert.That(ex.Message, Is.EqualTo("No league matches or no league detail was found for the season (2000 - 2001)."));
        }
        
        [Test]
        public void Build_ShouldThrowAnException_GivenNoLeagueDetailModel_ForAGivenYear()
        {
            var filters = new[] { new SeasonTierFilter { SeasonStartYear = 2000, Tier = Tier.TopTier}};
            
            var ex = Assert.Throws<Exception>(() => _historicalPositionBuilder.Build(
                "", 
                filters, 
                new List<MatchDetailModel> { new MatchDetailModel() }, 
                new List<MatchDetailModel>(), 
                new List<PointDeductionModel>(), 
                new List<LeagueDetailModel>()));
            Assert.That(ex.Message, Is.EqualTo("No league matches or no league detail was found for the season (2000 - 2001)."));
        }
        
        [Test]
        public void Build_CorrectlyFiltersLeagueMatchDetails_UsedToCreateLeagueTable()
        {
            var filters = new[] { new SeasonTierFilter { SeasonStartYear = 2000, Tier = Tier.TopTier}};
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel {Date = new DateTime(2000, 10, 1), HomeTeam = "Team1", AwayTeam = "Team2"},
                new MatchDetailModel {Date = new DateTime(1995, 10, 1), HomeTeam = "Team1", AwayTeam = "Team3"}
            };
            
            _historicalPositionBuilder.Build(
                "Team1", 
                filters, 
                leagueMatches, 
                new List<MatchDetailModel>(), 
                new List<PointDeductionModel>(), 
                new List<LeagueDetailModel> { new LeagueDetailModel { Season = "2000 - 2001" } });

            var expectedLeagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel {Date = new DateTime(2000, 10, 1), HomeTeam = "Team1", AwayTeam = "Team2"}
            };
            
            _mockBuilder.Verify(mock => mock.BuildWithStatuses(
                It.Is<List<MatchDetailModel>>(actualLeagueMatches => MatchesAreEqual(actualLeagueMatches, expectedLeagueMatches)), 
                It.IsAny<List<PointDeductionModel>>(), 
                It.IsAny<LeagueDetailModel>(), 
                It.IsAny<List<MatchDetailModel>>()
                ), Times.Once());
        }

        [Test]
        public void Build_CorrectlyFiltersPointDeductions_UsedToCreateLeagueTable()
        {
            var filters = new[] { new SeasonTierFilter { SeasonStartYear = 2000, Tier = Tier.TopTier}};
            var leagueMatches = new List<MatchDetailModel> { new MatchDetailModel {Date = new DateTime(2000, 10, 1), HomeTeam = "Team1", AwayTeam = "Team2"} };
            var pointDeductions = new List<PointDeductionModel>
            {
                new PointDeductionModel {Season = "2000 - 2001", Team = "Team1"},
                new PointDeductionModel {Season = "1995 - 1996", Team = "Team1"}
            };
            
            _historicalPositionBuilder.Build(
                "Team1", 
                filters, 
                leagueMatches, 
                new List<MatchDetailModel>(), 
                pointDeductions, 
                new List<LeagueDetailModel> { new LeagueDetailModel { Season = "2000 - 2001" } });
            
            var expectedPointDeductions = new List<PointDeductionModel>
            {
                new PointDeductionModel {Season = "2000 - 2001", Team = "Team1"}
            };
            
            _mockBuilder.Verify(mock => mock.BuildWithStatuses(
                It.IsAny<List<MatchDetailModel>>(), 
                It.Is<List<PointDeductionModel>>(actualPointDeductions => PointDeductionsAreEqual(actualPointDeductions, expectedPointDeductions)), 
                It.IsAny<LeagueDetailModel>(), 
                It.IsAny<List<MatchDetailModel>>()
            ), Times.Once());
        }

        [Test]
        public void Build_CorrectlyFiltersPlayOffMatches_UsedToCreateLeagueTable()
        {
            var filters = new[] { new SeasonTierFilter { SeasonStartYear = 2000, Tier = Tier.TopTier}};
            var leagueMatches = new List<MatchDetailModel> { new MatchDetailModel {Date = new DateTime(2000, 10, 1), HomeTeam = "Team1", AwayTeam = "Team2"} };
            var playOffMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel {Date = new DateTime(2001, 5, 1), HomeTeam = "Team1", AwayTeam = "Team2"},
                new MatchDetailModel {Date = new DateTime(1996, 5, 1), HomeTeam = "Team1", AwayTeam = "Team3"}
            };
            
            _historicalPositionBuilder.Build(
                "Team1", 
                filters, 
                leagueMatches, 
                playOffMatches, 
                new List<PointDeductionModel>(), 
                new List<LeagueDetailModel> { new LeagueDetailModel { Season = "2000 - 2001" } });
            
            var expectedPlayOffMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel {Date = new DateTime(2001, 5, 1), HomeTeam = "Team1", AwayTeam = "Team2"}
            };

            
            _mockBuilder.Verify(mock => mock.BuildWithStatuses(
                It.IsAny<List<MatchDetailModel>>(), 
                It.IsAny<List<PointDeductionModel>>(), 
                It.IsAny<LeagueDetailModel>(), 
                It.Is<List<MatchDetailModel>>(actualPlayOffMatches => MatchesAreEqual(actualPlayOffMatches, expectedPlayOffMatches))
            ), Times.Once());
        }
        
        [Test]
        public void Build_CorrectlyFiltersLeagueDetailModels_UsedToCreateLeagueTable()
        {
            var filters = new[] { new SeasonTierFilter { SeasonStartYear = 2000, Tier = Tier.TopTier}};
            var leagueMatches = new List<MatchDetailModel> { new MatchDetailModel {Date = new DateTime(2000, 10, 1), HomeTeam = "Team1", AwayTeam = "Team2"} };
            var leagueDetailModels = new List<LeagueDetailModel>
            {
                new LeagueDetailModel { Season = "2000 - 2001" },
                new LeagueDetailModel { Season = "2001 - 2002" }
            };
            
            _historicalPositionBuilder.Build(
                "Team1", 
                filters, 
                leagueMatches, 
                new List<MatchDetailModel>(), 
                new List<PointDeductionModel>(), 
                leagueDetailModels);
            
            _mockBuilder.Verify(mock => mock.BuildWithStatuses(
                It.IsAny<List<MatchDetailModel>>(), 
                It.IsAny<List<PointDeductionModel>>(), 
                It.Is<LeagueDetailModel>(actualLeagueDetailModels => actualLeagueDetailModels.Season == "2000 - 2001"), 
                It.IsAny<List<MatchDetailModel>>()
            ), Times.Once());
        }
        
        [Test]
        public void Build_ThrowsAnException_GivenDuplicateLeagueDetailsForASelectedSeason()
        {
            var filters = new[] { new SeasonTierFilter { SeasonStartYear = 2000, Tier = Tier.TopTier}};
            var leagueMatches = new List<MatchDetailModel> { new MatchDetailModel {Date = new DateTime(2000, 10, 1), HomeTeam = "Team1", AwayTeam = "Team2"} };
            var leagueDetailModels = new List<LeagueDetailModel>
            {
                new LeagueDetailModel {Season = "2000 - 2001"},
                new LeagueDetailModel {Season = "2000 - 2001"}
            };

            var ex = Assert.Throws<Exception>(() => _historicalPositionBuilder.Build(
                "Team1", 
                filters, 
                leagueMatches, 
                new List<MatchDetailModel>(), 
                new List<PointDeductionModel>(), 
                leagueDetailModels));
            Assert.That(ex.Message, Is.EqualTo("The incorrect number of league details (2) were found for the given season."));
        }

        [TestCase(Tier.TopTier, 1)]
        [TestCase(Tier.SecondTier, 21)]
        [TestCase(Tier.ThirdTier, 45)]
        [TestCase(Tier.FourthTier, 69)]
        public void Build_AddsCorrectAbsolutePosition_ForAGivenTeamAndSeasonInTheEachTier_Since1995_1996(Tier tier, int position)
        {
            var filters = new[] { new SeasonTierFilter { SeasonStartYear = 2000, Tier = tier}};

            var result = _historicalPositionBuilder.Build(
                "Team1", 
                filters, 
                new List<MatchDetailModel> { new MatchDetailModel() }, 
                new List<MatchDetailModel>(), 
                new List<PointDeductionModel>(), 
                new List<LeagueDetailModel> { new LeagueDetailModel { Season = "2000 - 2001" } });
            
            Assert.Multiple(() =>
            {
                Assert.That(result.Single().AbsolutePosition, Is.EqualTo(position));
                Assert.That(result.Single().Season, Is.EqualTo("2000 - 2001"));
                Assert.That(result.Single().Status, Is.EqualTo("C"));
            });
        }
        
        [TestCase(Tier.TopTier, 1)]
        [TestCase(Tier.SecondTier, 23)]
        [TestCase(Tier.ThirdTier, 47)]
        [TestCase(Tier.FourthTier, 71)]
        public void Build_AddsCorrectAbsolutePosition_ForAGivenTeamAndSeasonInTheEachTier_Before1995_1996(Tier tier, int position)
        {
            var filters = new[] { new SeasonTierFilter { SeasonStartYear = 1993, Tier = tier}};

            var result = _historicalPositionBuilder.Build(
                "Team1", 
                filters, 
                new List<MatchDetailModel> { new MatchDetailModel() }, 
                new List<MatchDetailModel>(), 
                new List<PointDeductionModel>(), 
                new List<LeagueDetailModel> { new LeagueDetailModel { Season = "1993 - 1994" } });

            Assert.Multiple(() =>
            {
                Assert.That(result.Single().AbsolutePosition, Is.EqualTo(position));
                Assert.That(result.Single().Season, Is.EqualTo("1993 - 1994"));
                Assert.That(result.Single().Status, Is.EqualTo("C"));
            });
        }
        
        private static bool MatchesAreEqual(List<MatchDetailModel> actualMatches, List<MatchDetailModel> expectedMatches)
        {
            if (actualMatches.Count != expectedMatches.Count)
            {
                return false;
            }

            for (var i = 0; i < actualMatches.Count; i++)
            {
                var datesDoNotMatch = actualMatches[i].Date != expectedMatches[i].Date;
                var homeTeamsDoNotMatch = actualMatches[i].HomeTeam != expectedMatches[i].HomeTeam;
                var awayTeamsDoNotMatch = actualMatches[i].AwayTeam != expectedMatches[i].AwayTeam;

                if (datesDoNotMatch || homeTeamsDoNotMatch || awayTeamsDoNotMatch)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool PointDeductionsAreEqual(List<PointDeductionModel> actualPointDeductionModels, List<PointDeductionModel> expectedPointDeductionModels)
        {
            if (actualPointDeductionModels.Count != expectedPointDeductionModels.Count)
            {
                return false;
            }

            for (var i = 0; i < actualPointDeductionModels.Count; i++)
            {
                var teamsDoNotMatch = actualPointDeductionModels[i].Team != expectedPointDeductionModels[i].Team;
                var seasonsDoNotMatch = actualPointDeductionModels[i].Season != expectedPointDeductionModels[i].Season;

                if (teamsDoNotMatch || seasonsDoNotMatch)
                {
                    return false;
                }
            }

            return true;
        }
    }
}