using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.LeagueSeason.LeagueTableDrillDown;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using FootballHistory.Api.Repositories.PointDeductionRepository;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.LeagueSeason.LeagueTableDrillDown
{
    [TestFixture]
    public class LeagueTableDrillDownBuilderTests
    {
        private LeagueTableDrillDownBuilder _builder;
        private readonly DateTime _dayOne = new DateTime(2018, 1, 1);

        [SetUp]
        public void Setup()
        {
            _builder = new LeagueTableDrillDownBuilder();
        }
        
        [Test]
        public void Build_ShouldReturnNoForm_GivenNoMatches()
        {
            var matches = new List<MatchDetailModel>();
            
            var drillDown = _builder.Build("Team1", matches, new List<PointDeductionModel>());
            
            Assert.That(drillDown.Form.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void Build_ShouldReturnNoForm_GivenATeamWithNoMatches()
        {
            var matches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team2", AwayTeam = "Team3", HomeGoals = 1, AwayGoals = 1 }
            };

            var drillDown = _builder.Build("Team1", matches, new List<PointDeductionModel>());
            
            Assert.That(drillDown.Form.Count, Is.EqualTo(0));
        }
                
        [Test]
        public void Build_ShouldReturnCorrectForm_GivenATeamWithOneMatch()
        {
            var matches = new List<MatchDetailModel>
            {
                new MatchDetailModel { Date = _dayOne, HomeTeam = "Team1", AwayTeam = "Team2", HomeGoals = 1, AwayGoals = 1 },
                new MatchDetailModel { Date = _dayOne.AddDays(1), HomeTeam = "Team2", AwayTeam = "Team3", HomeGoals = 2, AwayGoals = 1 }
            };

            var drillDown = _builder.Build("Team1", matches, new List<PointDeductionModel>());
            
            var actual = drillDown.Form.Select(f => (f.MatchDate, f.Result)).ToList();
            var expected = new List<(DateTime, string)> { (_dayOne, "D") };
            Assert.That(actual, Is.EqualTo(expected));
        }
                        
        [Test]
        public void Build_ShouldReturnCorrectFormInCorrectOrder_GivenATeamWithTwoMatches()
        {
            var matches = new List<MatchDetailModel>
            {
                new MatchDetailModel { Date = _dayOne.AddDays(1), HomeTeam = "Team1", AwayTeam = "Team2", HomeGoals = 1, AwayGoals = 1 },
                new MatchDetailModel { Date = _dayOne, HomeTeam = "Team1", AwayTeam = "Team3", HomeGoals = 2, AwayGoals = 1 },
                new MatchDetailModel { Date = _dayOne.AddDays(2), HomeTeam = "Team2", AwayTeam = "Team1", HomeGoals = 1, AwayGoals = 2 },
                new MatchDetailModel { Date = _dayOne.AddDays(4), HomeTeam = "Team3", AwayTeam = "Team1", HomeGoals = 2, AwayGoals = 1 },
                new MatchDetailModel { Date = _dayOne.AddDays(3), HomeTeam = "Team1", AwayTeam = "Team4", HomeGoals = 1, AwayGoals = 2 }
            };

            var drillDown = _builder.Build("Team1", matches, new List<PointDeductionModel>());
            var actual = drillDown.Form.Select(f => (f.MatchDate, f.Result)).ToList();
            var expected = new List<(DateTime, string)>
            {
                (_dayOne, "W"), (_dayOne.AddDays(1), "D"), (_dayOne.AddDays(2), "W"), (_dayOne.AddDays(3), "L"), (_dayOne.AddDays(4), "L")
            };
            
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Build_ShouldThrowException_GivenTwoMatchesInvolvingTheSameTeamOnTheSameDay()
        {
            var matches = new List<MatchDetailModel>
            {
                new MatchDetailModel { Date = _dayOne, HomeTeam = "Team1", AwayTeam = "Team2", HomeGoals = 1, AwayGoals = 1 },
                new MatchDetailModel { Date = _dayOne, HomeTeam = "Team2", AwayTeam = "Team1", HomeGoals = 2, AwayGoals = 1 }
            };

            var ex = Assert.Throws<Exception>(() => _builder.Build("Team1", matches, new List<PointDeductionModel>()));
            Assert.That(ex.Message, Is.EqualTo("Multiple matches involving Team1 were found with the same match date."));
        }

        [Test]
        public void Build_ShouldReturnEmptyPositions_GivenNoMatches()
        {
            var matches = new List<MatchDetailModel>();
            
            var drillDown = _builder.Build("Team1", matches, new List<PointDeductionModel>());
            
            Assert.That(drillDown.Positions.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void Build_ShouldReturnTwoPositions_WithCorrectDates_GivenOneMatch()
        {
            var matches = new List<MatchDetailModel>
            {
                new MatchDetailModel { Date = _dayOne, HomeTeam = "Team1", AwayTeam = "Team2", HomeGoals = 2, AwayGoals = 1 }
            };

            var drillDown = _builder.Build("Team1", matches, new List<PointDeductionModel>());

            var actual = drillDown.Positions.Select(p => p.Date).ToList();
            var expected = new List<DateTime> { _dayOne, _dayOne.AddDays(1) };
            Assert.That(actual, Is.EqualTo(expected));
        }
                
        [Test]
        public void Build_ShouldIncludeMissingTeams_WhenCalculatingLeaguePositions()
        {
            var matches = new List<MatchDetailModel>
            {
                new MatchDetailModel { Date = _dayOne, HomeTeam = "Team1", AwayTeam = "Team2", HomeGoals = 1, AwayGoals = 2 },
                new MatchDetailModel { Date = _dayOne.AddDays(1), HomeTeam = "Team1", AwayTeam = "Team3", HomeGoals = 2, AwayGoals = 1 }
            };

            var drillDown = _builder.Build("Team1", matches, new List<PointDeductionModel>());

            var actual = drillDown.Positions.Select(p => (p.Date, p.Position)).ToList();
            var expected = new List<(DateTime, int)> { (_dayOne, 1), (_dayOne.AddDays(1), 3), (_dayOne.AddDays(2), 2) };
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}