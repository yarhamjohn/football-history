using System;
using System.Collections.Generic;
using FootballHistory.Api.LeagueSeason.LeagueTable;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using FootballHistory.Api.Repositories.PointDeductionRepository;
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
                .Returns(() => new Mock<ILeagueTableCalculator>().Object);
            _leagueTableBuilder = new LeagueTableBuilder(mockFactory.Object);
        }

        [Test]
        public void Build_ShouldReturnLeagueTableWithNoRows_GivenNoMatches()
        {
            var leagueMatches = new List<MatchDetailModel>();
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel();
            var playOffMatches = new List<MatchDetailModel>();
            
            var leagueTable = _leagueTableBuilder.Build(leagueMatches, pointDeductions, leagueDetailModel, playOffMatches);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void Build_ShouldReturnLeagueTableWithTwoRows_GivenOneMatch()
        {
            var leagueMatches = new List<MatchDetailModel> { new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" } };
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 2 };
            var playOffMatches = new List<MatchDetailModel>();
            
            var leagueTable = _leagueTableBuilder.Build(leagueMatches, pointDeductions, leagueDetailModel, playOffMatches);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(2));
        }

        [Test]
        public void Build_ShouldReturnLeagueTableWithTwoRows_GivenAHomeAndAwayPairOfMatchesBetweenTheSameTeams()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" },
                new MatchDetailModel { HomeTeam = "Team2", AwayTeam = "Team1" }
            };
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 2 };
            var playOffMatches = new List<MatchDetailModel>();
            
            var leagueTable = _leagueTableBuilder.Build(leagueMatches, pointDeductions, leagueDetailModel, playOffMatches);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(2));
        }

        [Test]
        public void Build_ShouldReturnLeagueTableWithThreeRows_GivenTwoMatchesBetweenThreeTeams()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" },
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team3" }
            };
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 3 };
            var playOffMatches = new List<MatchDetailModel>();
            
            var leagueTable = _leagueTableBuilder.Build(leagueMatches, pointDeductions, leagueDetailModel, playOffMatches);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(3));
        }

        [Test]
        public void Build_ShouldThrowAnException_GivenTwoMatchesWithTheSameHomeAndAwayTeams()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" },
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" }
            };
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 2 };
            var playOffMatches = new List<MatchDetailModel>();
            
            var ex = Assert.Throws<Exception>(() => _leagueTableBuilder.Build(leagueMatches, pointDeductions, leagueDetailModel, playOffMatches));
            Assert.That(ex.Message, Is.EqualTo("An invalid set of league matches were provided."));
        }
                
        [Test]
        public void Build_ShouldThrowAnException_GivenOneMatchWithTheSameHomeAndAwayTeam()
        {
            var leagueMatches = new List<MatchDetailModel> { new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team1" } };
            var pointDeductions = new List<PointDeductionModel>();
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 1 };
            var playOffMatches = new List<MatchDetailModel>();
                        
            var ex = Assert.Throws<Exception>(() => _leagueTableBuilder.Build(leagueMatches, pointDeductions, leagueDetailModel, playOffMatches));
            Assert.That(ex.Message, Is.EqualTo("An invalid set of league matches were provided."));
        }
    }
}
