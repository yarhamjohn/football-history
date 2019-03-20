using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.LeagueSeason.Table;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.LeagueSeason.Table
{
    [TestFixture]
    public class LeagueTableStatusCalculatorTests
    {
        private readonly List<MatchDetailModel> _noPlayOffMatches = new List<MatchDetailModel>();
        private LeagueTableStatusCalculator _leagueTableStatusCalculator;

        [SetUp]
        public void Setup()
        {
            _leagueTableStatusCalculator = new LeagueTableStatusCalculator();
        }
        
        [Test]
        public void AddStatuses_AddsEmptyStatusToTeamsThatFinishedInAStandardPosition()
        {
            var leagueTable = new Api.LeagueSeason.Table.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Position = 1},
                    new LeagueTableRow {Team = "Team2", Position = 2},
                    new LeagueTableRow {Team = "Team3", Position = 3}
                }
            };

            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 3 };
            var leagueTableWithPositions = _leagueTableStatusCalculator.AddStatuses(leagueTable, leagueDetailModel, _noPlayOffMatches);

            var actual = leagueTableWithPositions.Rows.Where(r => r.Status == string.Empty).Select(r => r.Team).ToList();
            var expected = new List<string> { "Team2", "Team3" };
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void AddStatuses_AddsCorrectStatusToTeamThatWonTheLeague()
        {
            var leagueTable = new Api.LeagueSeason.Table.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Position = 1},
                    new LeagueTableRow {Team = "Team2", Position = 2},
                    new LeagueTableRow {Team = "Team3", Position = 3}
                }
            };
            
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 3 };
            var leagueTableWithPositions = _leagueTableStatusCalculator.AddStatuses(leagueTable, leagueDetailModel, _noPlayOffMatches);

            var actual = leagueTableWithPositions.Rows.Where(r => r.Status == "C").Select(r => r.Team).ToList();
            var expected = new List<string> { "Team1" };
            Assert.That(actual, Is.EqualTo(expected));
        }
        
        [Test]
        public void AddStatuses_DoesNotAddPromotionStatusToChampion()
        {
            var leagueTable = new Api.LeagueSeason.Table.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Position = 1},
                    new LeagueTableRow {Team = "Team2", Position = 2},
                    new LeagueTableRow {Team = "Team3", Position = 3}
                }
            };

            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 3, PromotionPlaces = 1 };
            var leagueTableWithPositions = _leagueTableStatusCalculator.AddStatuses(leagueTable, leagueDetailModel, _noPlayOffMatches);

            var actual = leagueTableWithPositions.Rows.Where(r => r.Status == "P").Select(r => r.Team).ToList();
            var expected = new List<string>();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void AddStatuses_AddsCorrectStatusToTeamsThatFinishedInThePromotionPlaces()
        {
            var leagueTable = new Api.LeagueSeason.Table.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Position = 1},
                    new LeagueTableRow {Team = "Team2", Position = 2},
                    new LeagueTableRow {Team = "Team3", Position = 3}
                }
            };

            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 3, PromotionPlaces = 2 };
            var leagueTableWithPositions = _leagueTableStatusCalculator.AddStatuses(leagueTable, leagueDetailModel, _noPlayOffMatches);

            var actual = leagueTableWithPositions.Rows.Where(r => r.Status == "P").Select(r => r.Team).ToList();
            var expected = new List<string> { "Team2" };
            Assert.That(actual, Is.EqualTo(expected));
        }
        
        [Test]
        public void AddStatuses_AddsCorrectStatusToTeamsThatFinishedInTheRelegationPlaces()
        {
            var leagueTable = new Api.LeagueSeason.Table.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Position = 1},
                    new LeagueTableRow {Team = "Team2", Position = 2},
                    new LeagueTableRow {Team = "Team3", Position = 3}
                }
            };

            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 3, RelegationPlaces = 1 };
            var leagueTableWithPositions = _leagueTableStatusCalculator.AddStatuses(leagueTable, leagueDetailModel, _noPlayOffMatches);

            var actual = leagueTableWithPositions.Rows.Where(r => r.Status == "R").Select(r => r.Team).ToList();
            var expected = new List<string> { "Team3" };
            Assert.That(actual, Is.EqualTo(expected));
        }
                
        [Test]
        public void AddStatuses_AddsCorrectStatusToTeamsThatFinishedInThePlayOffPlacesButDidNotWin()
        {
            var leagueTable = new Api.LeagueSeason.Table.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Position = 1},
                    new LeagueTableRow {Team = "Team2", Position = 2},
                    new LeagueTableRow {Team = "Team3", Position = 3},
                    new LeagueTableRow {Team = "Team4", Position = 4},
                    new LeagueTableRow {Team = "Team5", Position = 5}
                }
            };

            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 5, PlayOffPlaces = 3 };
            var playOffMatches = new List<MatchDetailModel> { new MatchDetailModel { Round = "Final", HomeTeam = "Team2", AwayTeam = "Team3", HomeGoals = 0, AwayGoals = 1}};
            var leagueTableWithPositions = _leagueTableStatusCalculator.AddStatuses(leagueTable, leagueDetailModel, playOffMatches);

            var actual = leagueTableWithPositions.Rows.Where(r => r.Status == "PO").Select(r => r.Team).ToList();
            var expected = new List<string> { "Team2", "Team4" };
            Assert.That(actual, Is.EqualTo(expected));
        }
        
        [Test]
        public void AddStatuses_AddsCorrectStatusToTeamsThatFinishedInThePlayOffPlacesAndWonInNormalTime()
        {
            var leagueTable = new Api.LeagueSeason.Table.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Position = 1},
                    new LeagueTableRow {Team = "Team2", Position = 2},
                    new LeagueTableRow {Team = "Team3", Position = 3},
                    new LeagueTableRow {Team = "Team4", Position = 4}
                }
            };

            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 4, PlayOffPlaces = 2 };
            var playOffMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel
                {
                    Round = "Final", 
                    HomeTeam = "Team2", 
                    AwayTeam = "Team3", 
                    HomeGoals = 1, 
                    AwayGoals = 2, 
                    ExtraTime = false, 
                    PenaltyShootout = false
                }
            };
            var leagueTableWithPositions = _leagueTableStatusCalculator.AddStatuses(leagueTable, leagueDetailModel, playOffMatches);

            var actual = leagueTableWithPositions.Rows.Where(r => r.Status == "PO (P)").Select(r => r.Team).ToList();
            var expected = new List<string> { "Team3" };
            Assert.That(actual, Is.EqualTo(expected));
        }
        
        [Test]
        public void AddStatuses_AddsCorrectStatusToTeamsThatFinishedInThePlayOffPlacesAndWonInExtraTime()
        {
            var leagueTable = new Api.LeagueSeason.Table.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Position = 1},
                    new LeagueTableRow {Team = "Team2", Position = 2},
                    new LeagueTableRow {Team = "Team3", Position = 3},
                    new LeagueTableRow {Team = "Team4", Position = 4}
                }
            };

            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 4, PlayOffPlaces = 2 };
            var playOffMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel
                {
                    Round = "Final", 
                    HomeTeam = "Team2", 
                    AwayTeam = "Team3", 
                    HomeGoals = 1, 
                    AwayGoals = 1, 
                    ExtraTime = true, 
                    HomeGoalsET = 0, 
                    AwayGoalsET = 1, 
                    PenaltyShootout = false
                }
            };
            var leagueTableWithPositions = _leagueTableStatusCalculator.AddStatuses(leagueTable, leagueDetailModel, playOffMatches);

            var actual = leagueTableWithPositions.Rows.Where(r => r.Status == "PO (P)").Select(r => r.Team).ToList();
            var expected = new List<string> { "Team3" };
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void AddStatuses_AddsCorrectStatusToTeamsThatFinishedInThePlayOffPlacesAndWonOnPenalties()
        {
            var leagueTable = new Api.LeagueSeason.Table.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Position = 1},
                    new LeagueTableRow {Team = "Team2", Position = 2},
                    new LeagueTableRow {Team = "Team3", Position = 3},
                    new LeagueTableRow {Team = "Team4", Position = 4}
                }
            };

            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 4, PlayOffPlaces = 2 };
            var playOffMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel
                {
                    Round = "Final", 
                    HomeTeam = "Team2", 
                    AwayTeam = "Team3", 
                    HomeGoals = 1, 
                    AwayGoals = 1, 
                    ExtraTime = true, 
                    HomeGoalsET = 1, 
                    AwayGoalsET = 1, 
                    PenaltyShootout = true, 
                    HomePenaltiesScored = 3, 
                    AwayPenaltiesScored = 4
                }
            };
            var leagueTableWithPositions = _leagueTableStatusCalculator.AddStatuses(leagueTable, leagueDetailModel, playOffMatches);

            var actual = leagueTableWithPositions.Rows.Where(r => r.Status == "PO (P)").Select(r => r.Team).ToList();
            var expected = new List<string> { "Team3" };
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void AddStatuses_ThrowsAnException_GivenALeagueDetailModelThatDoesNotMatchTheLeagueTable()
        {
            var leagueTable = new Api.LeagueSeason.Table.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Position = 1},
                    new LeagueTableRow {Team = "Team2", Position = 2},
                    new LeagueTableRow {Team = "Team3", Position = 3}
                }
            };
            var leagueDetailModel = new LeagueDetailModel {TotalPlaces = 1 };
            var playOffMatches = new List<MatchDetailModel>();
            
            var ex = Assert.Throws<Exception>(() => _leagueTableStatusCalculator.AddStatuses(leagueTable, leagueDetailModel, playOffMatches));
            Assert.That(ex.Message, Is.EqualTo("The League Detail Model (1 places) does not match the League Table (3 rows)"));
        }
        
        [Test]
        public void AddStatuses_ThrowsAnException_GivenALeagueDetailModelThatContainsPlayOffPlaces_AndPlayOffMatchesThatContainNoFinal()
        {
            var leagueTable = new Api.LeagueSeason.Table.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Position = 1},
                    new LeagueTableRow {Team = "Team2", Position = 2},
                    new LeagueTableRow {Team = "Team3", Position = 3}
                }
            };
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 3, PlayOffPlaces = 1 };
            var playOffMatches = new List<MatchDetailModel>();
            
            var ex = Assert.Throws<Exception>(() => _leagueTableStatusCalculator.AddStatuses(leagueTable, leagueDetailModel, playOffMatches));
            Assert.That(ex.Message, Is.EqualTo("The League Detail Model contains 1 playoff places but the playoff matches provided contain no Final"));
        }
    }
}