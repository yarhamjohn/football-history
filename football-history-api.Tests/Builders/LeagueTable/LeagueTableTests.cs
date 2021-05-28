using System;
using System.Collections.Generic;
using FluentAssertions;
using football.history.api.Builders;
using football.history.api.Exceptions;
using NUnit.Framework;

namespace football.history.api.Tests.Builders.LeagueTable
{
    [TestFixture]
    public class LeagueTableTests
    {
        [Test]
        public void GetRows_returns_league_table_rows()
        {
            var rows = BuildRows();
            var leagueTable = new api.Builders.LeagueTable(rows);

            var actualRows = leagueTable.GetRows();

            actualRows.Should().Equal(rows);
        }

        [Test]
        public void GetRow_returns_specified_row()
        {
            var rows = BuildRows();
            var leagueTable = new api.Builders.LeagueTable(rows);

            var row = leagueTable.GetRow(2);

            row.TeamId.Should().Be(2);
            row.Position.Should().Be(1);
            row.Team.Should().Be("Norwich City");
        }

        [Test]
        public void GetRow_throws_given_non_existent_team()
        {
            var rows = BuildRows();
            var leagueTable = new api.Builders.LeagueTable(rows);

            var ex = Assert.Throws<DataInvalidException>(() => leagueTable.GetRow(99));

            ex.Message.Should().Be("The requested team (99) appeared 2 times in the league table. Expected it to appear once.");
        }

        [Test]
        public void GetPosition_returns_position()
        {
            var rows = BuildRows();
            var leagueTable = new api.Builders.LeagueTable(rows);

            var position = leagueTable.GetPosition(1);

            position.Should().Be(2);
        }

        private static List<LeagueTableRowDto> BuildRows()
            =>
                new()
                {
                    new()
                    {
                        Position = 1,
                        TeamId   = 2,
                        Team     = "Norwich City"
                    },
                    new()
                    {
                        Position = 2,
                        TeamId   = 1,
                        Team     = "Newcastle United"
                    }
                };
    }
}