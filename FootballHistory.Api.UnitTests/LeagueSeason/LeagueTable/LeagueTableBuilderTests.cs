using System;
using System.Collections.Generic;
using FootballHistory.Api.LeagueSeason.LeagueTable;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using FootballHistory.Api.Repositories.PointDeductionRepository;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.LeagueSeason.LeagueTable
{
    [TestFixture]
    public class LeagueTableBuilderTests
    {
        private LeagueTableBuilder _leagueTableBuilder;
        
        [SetUp]
        public void SetUp()
        {
            var mockFactory = new Mock<ILeagueTableCalculatorFactory>();
            mockFactory
                .Setup(x => x.Create(It.IsAny<List<MatchDetailModel>>(), It.IsAny<List<PointDeductionModel>>(), It.IsAny<string>()))
                .Returns(() => new LeagueTableCalculator(new List<MatchDetailModel>(), new List<PointDeductionModel>(), ""));
            _leagueTableBuilder = new LeagueTableBuilder(mockFactory.Object);
        }

        [Test]
        public void BuildWithStatuses_ShouldReturnLeagueTableWithNoRows_GivenNoMatches()
        {
            var leagueMatches = new List<MatchDetailModel>();
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel { Competition = "Test", Season = "0000 - 0000"};
            var playOffMatches = new List<MatchDetailModel>();
            
            var leagueTable = _leagueTableBuilder.BuildWithStatuses(leagueMatches, pointDeductions, leagueDetailModel, playOffMatches);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void BuildWithStatuses_ShouldReturnLeagueTableWithTwoRows_GivenOneMatch()
        {
            var leagueMatches = new List<MatchDetailModel> { new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" } };
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 2, Competition = "Test", Season = "0000 - 0000"};
            var playOffMatches = new List<MatchDetailModel>();
            
            var leagueTable = _leagueTableBuilder.BuildWithStatuses(leagueMatches, pointDeductions, leagueDetailModel, playOffMatches);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(2));
        }

        [Test]
        public void BuildWithStatuses_ShouldReturnLeagueTableWithTwoRows_GivenAHomeAndAwayPairOfMatchesBetweenTheSameTeams()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" },
                new MatchDetailModel { HomeTeam = "Team2", AwayTeam = "Team1" }
            };
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 2, Competition = "Test", Season = "0000 - 0000"};
            var playOffMatches = new List<MatchDetailModel>();
            
            var leagueTable = _leagueTableBuilder.BuildWithStatuses(leagueMatches, pointDeductions, leagueDetailModel, playOffMatches);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(2));
        }

        [Test]
        public void BuildWithStatuses_ShouldReturnLeagueTableWithThreeRows_GivenTwoMatchesBetweenThreeTeams()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" },
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team3" }
            };
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 3, Competition = "Test", Season = "0000 - 0000"};
            var playOffMatches = new List<MatchDetailModel>();
            
            var leagueTable = _leagueTableBuilder.BuildWithStatuses(leagueMatches, pointDeductions, leagueDetailModel, playOffMatches);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(3));
        }

        [Test]
        public void BuildWithStatuses_ShouldThrowAnException_GivenTwoMatchesWithTheSameHomeAndAwayTeams()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" },
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" }
            };
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 2};
            var playOffMatches = new List<MatchDetailModel>();
            
            var ex = Assert.Throws<Exception>(() => _leagueTableBuilder.BuildWithStatuses(leagueMatches, pointDeductions, leagueDetailModel, playOffMatches));
            Assert.That(ex.Message, Is.EqualTo("An invalid set of league matches were provided."));
        }
                
        [Test]
        public void BuildWithStatuses_ShouldThrowAnException_GivenOneMatchWithTheSameHomeAndAwayTeam()
        {
            var leagueMatches = new List<MatchDetailModel> { new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team1" } };
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 1 };
            var playOffMatches = new List<MatchDetailModel>();
                        
            var ex = Assert.Throws<Exception>(() => _leagueTableBuilder.BuildWithStatuses(leagueMatches, pointDeductions, leagueDetailModel, playOffMatches));
            Assert.That(ex.Message, Is.EqualTo("An invalid set of league matches were provided."));
        }
        
        [Test]
        public void BuildWithoutStatuses_ShouldReturnLeagueTableWithNoRows_GivenNoMatches()
        {
            var leagueMatches = new List<MatchDetailModel>();
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel { Competition = "Test", Season = "0000 - 0000"};
            var missingTeams = new List<string>();
            
            var leagueTable = _leagueTableBuilder.BuildWithoutStatuses(leagueMatches, pointDeductions, leagueDetailModel, missingTeams);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void BuildWithoutStatuses_ShouldReturnLeagueTableWithTwoRows_GivenOneMatch_AndNoMissingTeams()
        {
            var leagueMatches = new List<MatchDetailModel> { new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" } };
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 2, Competition = "Test", Season = "0000 - 0000"};
            var missingTeams = new List<string>();

            var leagueTable = _leagueTableBuilder.BuildWithoutStatuses(leagueMatches, pointDeductions, leagueDetailModel, missingTeams);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(2));
        }

        [Test]
        public void BuildWithoutStatuses_ShouldReturnLeagueTableWithTwoRows_GivenAHomeAndAwayPairOfMatchesBetweenTheSameTeams_AndNoMissingTeams()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" },
                new MatchDetailModel { HomeTeam = "Team2", AwayTeam = "Team1" }
            };
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 2, Competition = "Test", Season = "0000 - 0000"};
            var missingTeams = new List<string>();
            
            var leagueTable = _leagueTableBuilder.BuildWithoutStatuses(leagueMatches, pointDeductions, leagueDetailModel, missingTeams);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(2));
        }

        [Test]
        public void BuildWithoutStatuses_ShouldReturnLeagueTableWithThreeRows_GivenTwoMatchesBetweenThreeTeams_AndNoMissingTeams()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" },
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team3" }
            };
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 3, Competition = "Test", Season = "0000 - 0000"};
            var missingTeams = new List<string>();
            
            var leagueTable = _leagueTableBuilder.BuildWithoutStatuses(leagueMatches, pointDeductions, leagueDetailModel, missingTeams);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(3));
        }

        [Test]
        public void BuildWithoutStatuses_ShouldReturnLeagueTableWithThreeRows_GivenTwoMatchesBetweenTwoTeams_AndOneMissingTeam()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" },
                new MatchDetailModel { HomeTeam = "Team2", AwayTeam = "Team1" }
            };
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 3, Competition = "Test", Season = "0000 - 0000"};
            var missingTeams = new List<string> { "Team3" };
            
            var leagueTable = _leagueTableBuilder.BuildWithoutStatuses(leagueMatches, pointDeductions, leagueDetailModel, missingTeams);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(3));
        }
        
        [Test]
        public void BuildWithoutStatuses_ShouldThrowAnException_GivenTwoMatchesWithTheSameHomeAndAwayTeams()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" },
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" }
            };
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 2};
            var missingTeams = new List<string>();
            
            var ex = Assert.Throws<Exception>(() => _leagueTableBuilder.BuildWithoutStatuses(leagueMatches, pointDeductions, leagueDetailModel, missingTeams));
            Assert.That(ex.Message, Is.EqualTo("An invalid set of league matches were provided."));
        }
                
        [Test]
        public void BuildWithoutStatuses_ShouldThrowAnException_GivenOneMatchWithTheSameHomeAndAwayTeam()
        {
            var leagueMatches = new List<MatchDetailModel> { new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team1" } };
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 1 };
            var missingTeams = new List<string>();
                        
            var ex = Assert.Throws<Exception>(() => _leagueTableBuilder.BuildWithoutStatuses(leagueMatches, pointDeductions, leagueDetailModel, missingTeams));
            Assert.That(ex.Message, Is.EqualTo("An invalid set of league matches were provided."));
        }
    }
}
