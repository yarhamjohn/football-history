using System;
using FluentAssertions;
using football.history.api.Builders;
using football.history.api.Repositories.Competition;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Builders.LeagueTable
{
    [TestFixture]
    public class StatusCalculatorTests
    {
        [Test]
        public void GetStatus_returns_Champions_for_top_league_position()
        {
            var competition = GetCompetitionModel();
            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerChecker>();
            var statusCalculator = new StatusCalculator(mockPlayOffWinnerCalculator.Object);
            
            var status = statusCalculator.GetStatus("team", 1, competition);
            
            status.Should().Be("Champions");
        }
        
        [Test]
        public void GetStatus_returns_Promoted_if_in_promotion_position()
        {
            var competition = GetCompetitionModel();
            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerChecker>();
            var statusCalculator = new StatusCalculator(mockPlayOffWinnerCalculator.Object);
            
            var status = statusCalculator.GetStatus("team", 2, competition);
            
            status.Should().Be("Promoted");
        }
        
        [Test]
        public void GetStatus_returns_PlayOffs_Winner_if_promoted_via_playoffs()
        {
            var competition = GetCompetitionModel();
            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerChecker>();
            mockPlayOffWinnerCalculator
                .Setup(x => x.IsPlayOffWinner(competition, "team"))
                .Returns(true);
            var statusCalculator = new StatusCalculator(mockPlayOffWinnerCalculator.Object);
            
            var status = statusCalculator.GetStatus("team", 3, competition);
            status.Should().Be("PlayOff Winner");
        }
        
        [Test]
        public void GetStatus_returns_PlayOffs_if_in_playoffs_but_not_promoted()
        {
            var competition = GetCompetitionModel();
            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerChecker>();
            mockPlayOffWinnerCalculator
                .Setup(x => x.IsPlayOffWinner(competition, "team"))
                .Returns(false);
            var statusCalculator = new StatusCalculator(mockPlayOffWinnerCalculator.Object);
            
            var status = statusCalculator.GetStatus("team", 4, competition);
            status.Should().Be("PlayOffs");
        }

        [Test]
        public void GetStatus_returns_Relegated_if_in_relegation_position()
        {
            var competition = GetCompetitionModel();
            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerChecker>();
            var statusCalculator = new StatusCalculator(mockPlayOffWinnerCalculator.Object);
            
            var status = statusCalculator.GetStatus("team", 24, competition);
            status.Should().Be("Relegated");
        }

        [Test]
        public void GetStatus_returns_Relegation_PlayOffs_if_in_relegation_playoff_position()
        {
            var competition = GetCompetitionModel();
            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerChecker>();
            mockPlayOffWinnerCalculator
                .Setup(x => x.IsRelegationPlayOffWinner(competition, "team"))
                .Returns(true);
            var statusCalculator = new StatusCalculator(mockPlayOffWinnerCalculator.Object);
            
            var status = statusCalculator.GetStatus("team", 21, competition);
            status.Should().Be("Relegation PlayOffs");
        }

        [Test]
        public void GetStatus_returns_Relegated_PlayOffs_if_relegated_from_relegation_playoff()
        {
            var competition = GetCompetitionModel();
            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerChecker>();
            mockPlayOffWinnerCalculator
                .Setup(x => x.IsRelegationPlayOffWinner(competition, "team"))
                .Returns(false);
            var statusCalculator = new StatusCalculator(mockPlayOffWinnerCalculator.Object);
            
            var status = statusCalculator.GetStatus("team", 21, competition);
            status.Should().Be("Relegated - PlayOffs");
        }

        [Test]
        public void GetStatus_returns_ReElected_if_reelected()
        {
            var competition = GetReElectionCompetitionModel();
            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerChecker>();
            var statusCalculator = new StatusCalculator(mockPlayOffWinnerCalculator.Object);
            
            var status = statusCalculator.GetStatus("team", 23, competition);
            
            status.Should().Be("Re-elected");
        }

        [Test]
        public void GetStatus_returns_Failed_ReElection_if_not_reelected()
        {
            var competition = GetReElectionCompetitionModel();
            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerChecker>();
            var statusCalculator = new StatusCalculator(mockPlayOffWinnerCalculator.Object);
            
            var status = statusCalculator.GetStatus("team", 24, competition);
            
            status.Should().Be("Failed Re-election");
        }

        [Test]
        public void GetStatus_returns_null_if_not_in_mid_table()
        {
            var competition = GetCompetitionModel();
            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerChecker>();
            var statusCalculator = new StatusCalculator(mockPlayOffWinnerCalculator.Object);
            
            var status = statusCalculator.GetStatus("team", 10, competition);
            
            status.Should().BeNull();
        }

        [Test]
        public void GetStatus_throws_given_competition_with_a_relegation_play_off_place_but_no_relegation_place()
        {
            var competition = new CompetitionModel(
                Id: 1,
                Name: "Championship",
                SeasonId: 1,
                StartYear: 2000,
                EndYear: 2001,
                Tier: 2,
                Region: null,
                Comment: null,
                PointsForWin: 3,
                TotalPlaces: 24,
                PromotionPlaces: 2,
                RelegationPlaces: 0,
                PlayOffPlaces: 4,
                RelegationPlayOffPlaces: 1,
                ReElectionPlaces: 0,
                FailedReElectionPosition: null);
            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerChecker>();
            var statusCalculator = new StatusCalculator(mockPlayOffWinnerCalculator.Object);
            
            var ex = Assert.Throws<InvalidOperationException>(() => statusCalculator.GetStatus("team", 10, competition));
            
            ex.Message.Should().Be("Invalid competition format found. Cannot have a relegation play off place without relegation places.");
        }

        [Test]
        public void GetStatus_throws_given_competition_with_both_a_relegation_place_and_a_reelection_place()
        {
            var competition = new CompetitionModel(
                Id: 1,
                Name: "Championship",
                SeasonId: 1,
                StartYear: 2000,
                EndYear: 2001,
                Tier: 2,
                Region: null,
                Comment: null,
                PointsForWin: 3,
                TotalPlaces: 24,
                PromotionPlaces: 2,
                RelegationPlaces: 1,
                PlayOffPlaces: 4,
                RelegationPlayOffPlaces: 0,
                ReElectionPlaces: 1,
                FailedReElectionPosition: null);
            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerChecker>();
            var statusCalculator = new StatusCalculator(mockPlayOffWinnerCalculator.Object);
            
            var ex = Assert.Throws<InvalidOperationException>(() => statusCalculator.GetStatus("team", 10, competition));
            
            ex.Message.Should().Be("Invalid competition format found. Cannot have relegation places and re-election places.");
        }
        
        private static CompetitionModel GetCompetitionModel()
        {
            return new(
                Id: 1,
                Name: "Championship",
                SeasonId: 1,
                StartYear: 2000,
                EndYear: 2001,
                Tier: 2,
                Region: null,
                Comment: null,
                PointsForWin: 3,
                TotalPlaces: 24,
                PromotionPlaces: 2,
                RelegationPlaces: 3,
                PlayOffPlaces: 4,
                RelegationPlayOffPlaces: 1,
                ReElectionPlaces: 0,
                FailedReElectionPosition: null);
        }        
        
        private static CompetitionModel GetReElectionCompetitionModel()
        {
            return new(
                Id: 1,
                Name: "Championship",
                SeasonId: 1,
                StartYear: 2000,
                EndYear: 2001,
                Tier: 2,
                Region: null,
                Comment: null,
                PointsForWin: 3,
                TotalPlaces: 24,
                PromotionPlaces: 2,
                RelegationPlaces: 0,
                PlayOffPlaces: 4,
                RelegationPlayOffPlaces: 0,
                ReElectionPlaces: 2,
                FailedReElectionPosition: 24);
        }
    }
}