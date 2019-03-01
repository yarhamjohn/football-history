using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Builders;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Repositories.Models;
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
            _leagueTableBuilder = new LeagueTableBuilder();
        }

        [Test]
        public void Build_ShouldReturnLeagueTableWithNoRows_GivenNoMatches()
        {
            var leagueMatches = new List<MatchDetailModel>();
            
            var leagueTable = _leagueTableBuilder.Build(leagueMatches);
            
            Assert.That(leagueTable.Rows.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void Build_ShouldReturnLeagueTableWithTwoRows_GivenOneMatch()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" }
            };
            
            var leagueTable = _leagueTableBuilder.Build(leagueMatches);
            
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
            
            var leagueTable = _leagueTableBuilder.Build(leagueMatches);
            
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
            
            var leagueTable = _leagueTableBuilder.Build(leagueMatches);
            
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
            
            var ex = Assert.Throws<Exception>(() => _leagueTableBuilder.Build(leagueMatches));
            Assert.That(ex.Message, Is.EqualTo("An invalid set of league matches were provided."));
        }
                
        [Test]
        public void Build_ShouldThrowAnException_GivenOneMatchWithTheSameHomeAndAwayTeam()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team1" }
            };
            
            var ex = Assert.Throws<Exception>(() => _leagueTableBuilder.Build(leagueMatches));
            Assert.That(ex.Message, Is.EqualTo("An invalid set of league matches were provided."));
        }
    }
}
