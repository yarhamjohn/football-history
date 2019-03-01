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
        
        [Test]
        public void Build_ShouldDeductNoPoints_GivenNoPointDeductionsForSpecifiedTeam()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2", HomeGoals = 3, AwayGoals = 1 }
            };
            var pointDeductions = new List<PointDeductionModel>
            {
                new PointDeductionModel { PointsDeducted = 1, Team = "Team3", Reason = "Reason1" }
            };

            var leagueTable = _leagueTableBuilder.Build(leagueMatches, pointDeductions);

            Assert.Multiple(() =>
                {
                    Assert.That(leagueTable.Rows.Single(r => r.Team == "Team1").Points, Is.EqualTo(3));
                    Assert.That(leagueTable.Rows.Single(r => r.Team == "Team1").PointsDeducted, Is.EqualTo(0));
                    Assert.That(leagueTable.Rows.Single(r => r.Team == "Team1").PointsDeductionReason, Is.EqualTo(""));
                }
            );        
        }
        
        [Test]
        public void Build_ShouldDeductCorrectPoints_GivenOnePointDeductionsForSpecifiedTeam()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2", HomeGoals = 3, AwayGoals = 1 }
            };
            var pointDeductions = new List<PointDeductionModel>
            {
                new PointDeductionModel { PointsDeducted = 1, Team = "Team1", Reason = "Reason1" }
            };

            var leagueTable = _leagueTableBuilder.Build(leagueMatches, pointDeductions);

            Assert.Multiple(() =>
                {
                    Assert.That(leagueTable.Rows.Single(r => r.Team == "Team1").Points, Is.EqualTo(2));
                    Assert.That(leagueTable.Rows.Single(r => r.Team == "Team1").PointsDeducted, Is.EqualTo(1));
                    Assert.That(leagueTable.Rows.Single(r => r.Team == "Team1").PointsDeductionReason, Is.EqualTo("Reason1"));
                }
            );           
        }
        
        [Test]
        public void Build_ShouldDeductCorrectPoints_GivenTwoPointDeductionsForSpecifiedTeam()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2", HomeGoals = 3, AwayGoals = 1 }
            };
            var pointDeductions = new List<PointDeductionModel>
            {
                new PointDeductionModel { PointsDeducted = 1, Team = "Team1", Reason = "Reason1" },
                new PointDeductionModel { PointsDeducted = 1, Team = "Team1", Reason = "Reason2" }
            };

            var leagueTable = _leagueTableBuilder.Build(leagueMatches, pointDeductions);

            Assert.Multiple(() =>
                {
                    Assert.That(leagueTable.Rows.Single(r => r.Team == "Team1").Points, Is.EqualTo(1));
                    Assert.That(leagueTable.Rows.Single(r => r.Team == "Team1").PointsDeducted, Is.EqualTo(2));
                    Assert.That(leagueTable.Rows.Single(r => r.Team == "Team1").PointsDeductionReason, Is.EqualTo("Reason1, Reason2"));
                }
            );           
        }
    }
}
