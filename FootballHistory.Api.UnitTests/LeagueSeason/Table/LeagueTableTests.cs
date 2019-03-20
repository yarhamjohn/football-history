using System;
using System.Collections.Generic;
using FootballHistory.Api.LeagueSeason.Table;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.LeagueSeason.Table
{
    [TestFixture]
    public class LeagueTableTests
    {
        [Test]
        public void GetPosition_ReturnsZero_IfPositionHasNotBeenSet()
        {
            var leagueTable = new Api.LeagueSeason.Table.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1"},
                    new LeagueTableRow {Team = "Team2"},
                    new LeagueTableRow {Team = "Team3"}
                }
            };

            var position = leagueTable.GetPosition("Team1");
            Assert.That(position, Is.EqualTo(0));
        }
        
        [Test]
        public void GetPosition_GetsTheCorrectPosition()
        {
            var leagueTable = new Api.LeagueSeason.Table.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Position = 3},
                    new LeagueTableRow {Team = "Team2", Position = 1},
                    new LeagueTableRow {Team = "Team3", Position = 2}
                }
            };

            var position = leagueTable.GetPosition("Team1");
            Assert.That(position, Is.EqualTo(3));
        }
        
        [Test]
        public void GetPosition_ShouldThrowAnException_GivenATeamNotInTheLeagueTable()
        {
            var leagueTable = new Api.LeagueSeason.Table.LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1", Position = 3},
                    new LeagueTableRow {Team = "Team2", Position = 1},
                    new LeagueTableRow {Team = "Team3", Position = 2}
                }
            };

            var ex = Assert.Throws<Exception>(() => leagueTable.GetPosition("Team4"));
            Assert.That(ex.Message, Is.EqualTo("The requested team (Team4) was not found in the league table."));
        }
    }
}