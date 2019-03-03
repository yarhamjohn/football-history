using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Builders;
using FootballHistory.Api.Repositories.Models;
using Moq;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.BuildersTests
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
            
            var leagueTable = _leagueTableBuilder.Build(leagueMatches, pointDeductions);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void Build_ShouldReturnLeagueTableWithTwoRows_GivenOneMatch()
        {
            var leagueMatches = new List<MatchDetailModel> { new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" } };
            var pointDeductions = new List<PointDeductionModel>();
            
            var leagueTable = _leagueTableBuilder.Build(leagueMatches, pointDeductions);
            
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

            var leagueTable = _leagueTableBuilder.Build(leagueMatches, pointDeductions);
            
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

            var leagueTable = _leagueTableBuilder.Build(leagueMatches, pointDeductions);
            
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

            var ex = Assert.Throws<Exception>(() => _leagueTableBuilder.Build(leagueMatches, pointDeductions));
            Assert.That(ex.Message, Is.EqualTo("An invalid set of league matches were provided."));
        }
                
        [Test]
        public void Build_ShouldThrowAnException_GivenOneMatchWithTheSameHomeAndAwayTeam()
        {
            var leagueMatches = new List<MatchDetailModel> { new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team1" } };
            var pointDeductions = new List<PointDeductionModel>();

            var ex = Assert.Throws<Exception>(() => _leagueTableBuilder.Build(leagueMatches, pointDeductions));
            Assert.That(ex.Message, Is.EqualTo("An invalid set of league matches were provided."));
        }
    }
}
