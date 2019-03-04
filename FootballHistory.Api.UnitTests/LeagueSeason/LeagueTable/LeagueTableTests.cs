using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.LeagueSeason.LeagueTable;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.LeagueSeason.LeagueTable
{
    [TestFixture]
    public class LeagueTableTests
    {
        [Test]
        public void AddPositionsAndStatuses_AddsPositionToEachRow_BasedOnPoints()
        {
            var leagueTable = new Api.LeagueSeason.LeagueTable.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Points = 3},
                    new LeagueTableRow {Team = "Team2", Points = 1},
                    new LeagueTableRow {Team = "Team3", Points = 2}
                }
            };

            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 3 };
            var playOffMatches = new List<MatchDetailModel>();
            var leagueTableWithPositions = leagueTable.AddPositionsAndStatuses(leagueDetailModel, playOffMatches);
            
            var actual = leagueTableWithPositions.Rows.Select(r => (r.Position, r.Team)).ToList();
            var expected = new List<(int, string)> { (1, "Team1"), (2, "Team3"), (3, "Team2") };

            Assert.That(actual, Is.EqualTo(expected));
        }
        
        [Test]
        public void AddPositionsAndStatuses_AddsPositionToEachRow_BasedOnPointsThenGoalDifference()
        {
            var leagueTable = new Api.LeagueSeason.LeagueTable.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Points = 2, GoalDifference = 1},
                    new LeagueTableRow {Team = "Team2", Points = 1, GoalDifference = 2},
                    new LeagueTableRow {Team = "Team3", Points = 1, GoalDifference = 3}
                }
            };

            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 3 };
            var playOffMatches = new List<MatchDetailModel>();
            var leagueTableWithPositions = leagueTable.AddPositionsAndStatuses(leagueDetailModel, playOffMatches);
            
            var actual = leagueTableWithPositions.Rows.Select(r => (r.Position, r.Team)).ToList();
            var expected = new List<(int, string)> { (1, "Team1"), (2, "Team3"), (3, "Team2") };

            Assert.That(actual, Is.EqualTo(expected));
        }
        
        [Test]
        public void AddPositionsAndStatuses_AddsPositionToEachRow_BasedOnPointsThenGoalDifferenceThenGoalsFor()
        {
            var leagueTable = new Api.LeagueSeason.LeagueTable.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Points = 1, GoalDifference = 2, GoalsFor = 1},
                    new LeagueTableRow {Team = "Team2", Points = 1, GoalDifference = 1, GoalsFor = 2},
                    new LeagueTableRow {Team = "Team3", Points = 1, GoalDifference = 1, GoalsFor = 3}
                }
            };

            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 3 };
            var playOffMatches = new List<MatchDetailModel>();
            var leagueTableWithPositions = leagueTable.AddPositionsAndStatuses(leagueDetailModel, playOffMatches);
            
            var actual = leagueTableWithPositions.Rows.Select(r => (r.Position, r.Team)).ToList();
            var expected = new List<(int, string)> { (1, "Team1"), (2, "Team3"), (3, "Team2") };

            Assert.That(actual, Is.EqualTo(expected));
        }
                       
        [Test]
        public void AddPositionsAndStatuses_AddsEmptyStatusToTeamsThatFinishedInAStandardPosition()
        {
            var leagueTable = new Api.LeagueSeason.LeagueTable.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Points = 3},
                    new LeagueTableRow {Team = "Team2", Points = 2},
                    new LeagueTableRow {Team = "Team3", Points = 1}
                }
            };

            var leagueDetailModel = new LeagueDetailModel {TotalPlaces = 3};
            var playOffMatches = new List<MatchDetailModel>();
            var leagueTableWithPositions = leagueTable.AddPositionsAndStatuses(leagueDetailModel, playOffMatches);

            var actual = leagueTableWithPositions.Rows.Where(r => r.Status == string.Empty).Select(r => (r.Team, r.Position)).ToList();
            var expected = new List<(string, int)> { ("Team2", 2), ("Team3", 3) };
            Assert.That(actual, Is.EqualTo(expected));
        }
        
        [Test]
        public void AddPositionsAndStatuses_AddsPositionToEachRow_BasedOnPointsThenGoalDifferenceThenGoalsForThenTeamName()
        {
            var leagueTable = new Api.LeagueSeason.LeagueTable.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team3", Points = 1, GoalDifference = 1, GoalsFor = 2},
                    new LeagueTableRow {Team = "Team1", Points = 1, GoalDifference = 1, GoalsFor = 1},
                    new LeagueTableRow {Team = "Team2", Points = 1, GoalDifference = 1, GoalsFor = 1}
                }
            };
            
            var leagueDetailModel = new LeagueDetailModel {TotalPlaces = 3};
            var playOffMatches = new List<MatchDetailModel>();
            var leagueTableWithPositions = leagueTable.AddPositionsAndStatuses(leagueDetailModel, playOffMatches);
            
            var actual = leagueTableWithPositions.Rows.Select(r => (r.Position, r.Team)).ToList();
            var expected = new List<(int, string)> { (1, "Team3"), (2, "Team1"), (3, "Team2") };

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void AddPositionsAndStatuses_AddsCorrectStatusToTeamThatWonTheLeague()
        {
            var leagueTable = new Api.LeagueSeason.LeagueTable.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Points = 3},
                    new LeagueTableRow {Team = "Team2", Points = 2},
                    new LeagueTableRow {Team = "Team3", Points = 1}
                }
            };
            
            var leagueDetailModel = new LeagueDetailModel {TotalPlaces = 3};
            var playOffMatches = new List<MatchDetailModel>();
            var leagueTableWithPositions = leagueTable.AddPositionsAndStatuses(leagueDetailModel, playOffMatches);

            var actual = leagueTableWithPositions.Rows.Where(r => r.Status == "C").Select(r => (r.Team, r.Position)).ToList();
            var expected = new List<(string, int)> { ("Team1", 1) };
            Assert.That(actual, Is.EqualTo(expected));
        }
        
        [Test]
        public void AddPositionsAndStatuses_DoesNotAddPromotionStatusToChampion()
        {
            var leagueTable = new Api.LeagueSeason.LeagueTable.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Points = 3},
                    new LeagueTableRow {Team = "Team2", Points = 2},
                    new LeagueTableRow {Team = "Team3", Points = 1}
                }
            };

            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 3, PromotionPlaces = 1};
            var playOffMatches = new List<MatchDetailModel>();
            var leagueTableWithPositions = leagueTable.AddPositionsAndStatuses(leagueDetailModel, playOffMatches);

            var actual = leagueTableWithPositions.Rows.Where(r => r.Status == "P").Select(r => (r.Team, r.Position)).ToList();
            var expected = new List<(string, int)>();
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void AddPositionsAndStatuses_AddsCorrectStatusToTeamsThatFinishedInThePromotionPlaces()
        {
            var leagueTable = new Api.LeagueSeason.LeagueTable.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Points = 3},
                    new LeagueTableRow {Team = "Team2", Points = 2},
                    new LeagueTableRow {Team = "Team3", Points = 1}
                }
            };

            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 3, PromotionPlaces = 2};
            var playOffMatches = new List<MatchDetailModel>();
            var leagueTableWithPositions = leagueTable.AddPositionsAndStatuses(leagueDetailModel, playOffMatches);

            var actual = leagueTableWithPositions.Rows.Where(r => r.Status == "P").Select(r => (r.Team, r.Position)).ToList();
            var expected = new List<(string, int)> { ("Team2", 2) };
            Assert.That(actual, Is.EqualTo(expected));
        }
        
        [Test]
        public void AddPositionsAndStatuses_AddsCorrectStatusToTeamsThatFinishedInTheRelegationPlaces()
        {
            var leagueTable = new Api.LeagueSeason.LeagueTable.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Points = 3},
                    new LeagueTableRow {Team = "Team2", Points = 2},
                    new LeagueTableRow {Team = "Team3", Points = 1}
                }
            };

            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 3, RelegationPlaces = 1};
            var playOffMatches = new List<MatchDetailModel>();
            var leagueTableWithPositions = leagueTable.AddPositionsAndStatuses(leagueDetailModel, playOffMatches);

            var actual = leagueTableWithPositions.Rows.Where(r => r.Status == "R").Select(r => (r.Team, r.Position)).ToList();
            var expected = new List<(string, int)> { ("Team3", 3) };
            Assert.That(actual, Is.EqualTo(expected));
        }
                
        [Test]
        public void AddPositionsAndStatuses_AddsCorrectStatusToTeamsThatFinishedInThePlayOffPlacesButDidNotWin()
        {
            var leagueTable = new Api.LeagueSeason.LeagueTable.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Points = 5},
                    new LeagueTableRow {Team = "Team2", Points = 4},
                    new LeagueTableRow {Team = "Team3", Points = 3},
                    new LeagueTableRow {Team = "Team4", Points = 2},
                    new LeagueTableRow {Team = "Team5", Points = 1}
                }
            };

            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 5, PlayOffPlaces = 3};
            var playOffMatches = new List<MatchDetailModel> { new MatchDetailModel { Round = "Final", HomeTeam = "Team2", AwayTeam = "Team3", HomeGoals = 0, AwayGoals = 1}};
            var leagueTableWithPositions = leagueTable.AddPositionsAndStatuses(leagueDetailModel, playOffMatches);

            var actual = leagueTableWithPositions.Rows.Where(r => r.Status == "PO").Select(r => (r.Team, r.Position)).ToList();
            var expected = new List<(string, int)> { ("Team2", 2), ("Team4", 4) };
            Assert.That(actual, Is.EqualTo(expected));
        }
        
        [Test]
        public void AddPositionsAndStatuses_AddsCorrectStatusToTeamsThatFinishedInThePlayOffPlacesAndWon()
        {
            var leagueTable = new Api.LeagueSeason.LeagueTable.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Points = 4},
                    new LeagueTableRow {Team = "Team2", Points = 3},
                    new LeagueTableRow {Team = "Team3", Points = 2},
                    new LeagueTableRow {Team = "Team4", Points = 1}
                }
            };

            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 4, PlayOffPlaces = 2};
            var playOffMatches = new List<MatchDetailModel> { new MatchDetailModel {Round = "Final", HomeTeam = "Team2", AwayTeam = "Team3", HomeGoals = 1, AwayGoals = 2}};
            var leagueTableWithPositions = leagueTable.AddPositionsAndStatuses(leagueDetailModel, playOffMatches);

            var actual = leagueTableWithPositions.Rows.Where(r => r.Status == "PO (P)").Select(r => (r.Team, r.Position)).ToList();
            var expected = new List<(string, int)> { ("Team3", 3) };
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void AddPositionsAndStatuses_ThrowsAnException_GivenALeagueDetailModelThatDoesNotMatchTheLeagueTable()
        {
            var leagueTable = new Api.LeagueSeason.LeagueTable.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Points = 3},
                    new LeagueTableRow {Team = "Team2", Points = 2},
                    new LeagueTableRow {Team = "Team3", Points = 1}
                }
            };
            var leagueDetailModel = new LeagueDetailModel {TotalPlaces = 1};
            var playOffMatches = new List<MatchDetailModel>();
            
            var ex = Assert.Throws<Exception>(() => leagueTable.AddPositionsAndStatuses(leagueDetailModel, playOffMatches));
            Assert.That(ex.Message, Is.EqualTo("The League Detail Model (1 places) does not match the League Table (3 rows)"));
        }
        
        [Test]
        public void AddPositionsAndStatuses_ThrowsAnException_GivenALeagueDetailModelThatContainsPlayOffPlaces_AndPlayOffMatchesThatContainNoFinal()
        {
            var leagueTable = new Api.LeagueSeason.LeagueTable.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Points = 3},
                    new LeagueTableRow {Team = "Team2", Points = 2},
                    new LeagueTableRow {Team = "Team3", Points = 1}
                }
            };
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 3, PlayOffPlaces = 1 };
            var playOffMatches = new List<MatchDetailModel>();
            
            var ex = Assert.Throws<Exception>(() => leagueTable.AddPositionsAndStatuses(leagueDetailModel, playOffMatches));
            Assert.That(ex.Message, Is.EqualTo("The League Detail Model contains 1 playoff places but the playoff matches provided contain no Final"));
        }
    }
}