using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Builders.Models;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.ModelsTests
{
    [TestFixture]
    public class LeagueTabTests
    {
        [Test]
        public void AddPosition_AddsPositionToEachRow_BasedOnPoints()
        {
            var leagueTable = new LeagueTab
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Points = 3},
                    new LeagueTableRow {Team = "Team2", Points = 1},
                    new LeagueTableRow {Team = "Team3", Points = 2}
                }
            };

            var leagueTableWithPositions = leagueTable.AddPositions();
            
            var actual = leagueTableWithPositions.Rows.Select(r => (r.Position, r.Team)).ToList();
            var expected = new List<(int, string)> { (1, "Team1"), (2, "Team3"), (3, "Team2") };

            Assert.That(actual, Is.EqualTo(expected));
        }
        
        [Test]
        public void AddPosition_AddsPositionToEachRow_BasedOnPointsThenGoalDifference()
        {
            var leagueTable = new LeagueTab
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Points = 2, GoalDifference = 1},
                    new LeagueTableRow {Team = "Team2", Points = 1, GoalDifference = 2},
                    new LeagueTableRow {Team = "Team3", Points = 1, GoalDifference = 3}
                }
            };

            var leagueTableWithPositions = leagueTable.AddPositions();
            
            var actual = leagueTableWithPositions.Rows.Select(r => (r.Position, r.Team)).ToList();
            var expected = new List<(int, string)> { (1, "Team1"), (2, "Team3"), (3, "Team2") };

            Assert.That(actual, Is.EqualTo(expected));
        }
        
        [Test]
        public void AddPosition_AddsPositionToEachRow_BasedOnPointsThenGoalDifferenceThenGoalsFor()
        {
            var leagueTable = new LeagueTab
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Points = 1, GoalDifference = 2, GoalsFor = 1},
                    new LeagueTableRow {Team = "Team2", Points = 1, GoalDifference = 1, GoalsFor = 2},
                    new LeagueTableRow {Team = "Team3", Points = 1, GoalDifference = 1, GoalsFor = 3}
                }
            };

            var leagueTableWithPositions = leagueTable.AddPositions();
            
            var actual = leagueTableWithPositions.Rows.Select(r => (r.Position, r.Team)).ToList();
            var expected = new List<(int, string)> { (1, "Team1"), (2, "Team3"), (3, "Team2") };

            Assert.That(actual, Is.EqualTo(expected));
        }
                
        [Test]
        public void AddPosition_AddsPositionToEachRow_BasedOnPointsThenGoalDifferenceThenGoalsForThenTeamName()
        {
            var leagueTable = new LeagueTab
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team3", Points = 1, GoalDifference = 1, GoalsFor = 2},
                    new LeagueTableRow {Team = "Team1", Points = 1, GoalDifference = 1, GoalsFor = 1},
                    new LeagueTableRow {Team = "Team2", Points = 1, GoalDifference = 1, GoalsFor = 1}
                }
            };

            var leagueTableWithPositions = leagueTable.AddPositions();
            
            var actual = leagueTableWithPositions.Rows.Select(r => (r.Position, r.Team)).ToList();
            var expected = new List<(int, string)> { (1, "Team3"), (2, "Team1"), (3, "Team2") };

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}