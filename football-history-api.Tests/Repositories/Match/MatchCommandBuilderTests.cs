using System;
using FluentAssertions;
using football.history.api.Repositories;
using football.history.api.Repositories.Match;
using football.history.api.Tests.Repositories.TestUtilities;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Repositories.Match
{
    [TestFixture]
    public class MatchCommandBuilderTests
    {
        [Test]
        public void Build_returns_correct_dbCommand_given_matchId()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            mockDatabaseConnection
                .Setup(x => x.CreateCommand())
                .Returns(new MockDbCommand());
            var builder = new MatchCommandBuilder();

            var dbCommand = builder.Build(mockDatabaseConnection.Object, 1);

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Matches] AS m");
            dbCommand.CommandText.Should().Contain("WHERE m.Id = @Id");
            dbCommand.Parameters.Should().HaveCount(1);
            dbCommand.Parameters["@Id"].Value.Should().Be(1);
        }

        [Test]
        public void Build_returns_correct_dbCommand_given_no_parameters()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            mockDatabaseConnection
                .Setup(x => x.CreateCommand())
                .Returns(new MockDbCommand());
            var builder = new MatchCommandBuilder();

            var dbCommand = builder.Build(mockDatabaseConnection.Object, null, null, null, null, null);

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Matches] AS m");
            dbCommand.CommandText.Should().NotContain("WHERE");
            dbCommand.Parameters.Should().BeEmpty();
        }

        [Test]
        public void Build_returns_correct_dbCommand_given_all_parameters()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            mockDatabaseConnection
                .Setup(x => x.CreateCommand())
                .Returns(new MockDbCommand());
            var builder = new MatchCommandBuilder();

            var dbCommand = builder.Build(mockDatabaseConnection.Object, 1, 2, 3, "League", new DateTime(2000, 1, 1));

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Matches] AS m");
            dbCommand.CommandText.Should()
                .Contain(
                    "WHERE c.Id = @CompetitionId AND s.Id = @SeasonId AND (ht.Id = @HomeTeamId OR at.Id = @AwayTeamId) AND r.Type = @Type AND m.MatchDate < @MatchDate");
            dbCommand.Parameters.Should().HaveCount(6);
            dbCommand.Parameters["@CompetitionId"].Value.Should().Be(1);
            dbCommand.Parameters["@SeasonId"].Value.Should().Be(2);
            dbCommand.Parameters["@HomeTeamId"].Value.Should().Be(3);
            dbCommand.Parameters["@AwayTeamId"].Value.Should().Be(3);
            dbCommand.Parameters["@Type"].Value.Should().Be("League");
            dbCommand.Parameters["@MatchDate"].Value.Should().Be(new DateTime(2000, 1, 1));
        }

        [Test]
        public void Build_returns_correct_dbCommand_given_only_competitionId()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            mockDatabaseConnection
                .Setup(x => x.CreateCommand())
                .Returns(new MockDbCommand());
            var builder = new MatchCommandBuilder();

            var dbCommand = builder.Build(mockDatabaseConnection.Object, 1, null, null, null, null);

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Matches] AS m");
            dbCommand.CommandText.Should().Contain("WHERE c.Id = @CompetitionId");
            dbCommand.CommandText.Should().NotContain("s.Id = @SeasonId");
            dbCommand.CommandText.Should().NotContain("ht.Id = @HomeTeamId");
            dbCommand.CommandText.Should().NotContain("at.Id = @AwayTeamId");
            dbCommand.CommandText.Should().NotContain("r.Type = @Type");
            dbCommand.CommandText.Should().NotContain("m.MatchDate < @MatchDate");

            dbCommand.Parameters.Should().HaveCount(1);
            dbCommand.Parameters["@CompetitionId"].Value.Should().Be(1);
        }

        [Test]
        public void Build_returns_correct_dbCommand_given_only_seasonId()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            mockDatabaseConnection
                .Setup(x => x.CreateCommand())
                .Returns(new MockDbCommand());
            var builder = new MatchCommandBuilder();

            var dbCommand = builder.Build(mockDatabaseConnection.Object, null, 1, null, null, null);

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Matches] AS m");
            dbCommand.CommandText.Should().Contain("WHERE s.Id = @SeasonId");
            dbCommand.CommandText.Should().NotContain("c.Id = @CompetitionId");
            dbCommand.CommandText.Should().NotContain("ht.Id = @HomeTeamId");
            dbCommand.CommandText.Should().NotContain("at.Id = @AwayTeamId");
            dbCommand.CommandText.Should().NotContain("r.Type = @Type");
            dbCommand.CommandText.Should().NotContain("m.MatchDate < @MatchDate");

            dbCommand.Parameters.Should().HaveCount(1);
            dbCommand.Parameters["@SeasonId"].Value.Should().Be(1);
        }

        [Test]
        public void Build_returns_correct_dbCommand_given_only_teamId()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            mockDatabaseConnection
                .Setup(x => x.CreateCommand())
                .Returns(new MockDbCommand());
            var builder = new MatchCommandBuilder();

            var dbCommand = builder.Build(mockDatabaseConnection.Object, null, null, 1, null, null);

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Matches] AS m");
            dbCommand.CommandText.Should().Contain("WHERE (ht.Id = @HomeTeamId OR at.Id = @AwayTeamId");
            dbCommand.CommandText.Should().NotContain("s.Id = @SeasonId");
            dbCommand.CommandText.Should().NotContain("c.Id = @CompetitionId");
            dbCommand.CommandText.Should().NotContain("r.Type = @Type");
            dbCommand.CommandText.Should().NotContain("m.MatchDate < @MatchDate");

            dbCommand.Parameters.Should().HaveCount(2);
            dbCommand.Parameters["@HomeTeamId"].Value.Should().Be(1);
            dbCommand.Parameters["@AWayTeamId"].Value.Should().Be(1);
        }

        [Test]
        public void Build_returns_correct_dbCommand_given_only_type()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            mockDatabaseConnection
                .Setup(x => x.CreateCommand())
                .Returns(new MockDbCommand());
            var builder = new MatchCommandBuilder();

            var dbCommand = builder.Build(mockDatabaseConnection.Object, null, null, null, "League", null);

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Matches] AS m");
            dbCommand.CommandText.Should().Contain("WHERE r.Type = @Type");
            dbCommand.CommandText.Should().NotContain("s.Id = @SeasonId");
            dbCommand.CommandText.Should().NotContain("c.Id = @CompetitionId");
            dbCommand.CommandText.Should().NotContain("ht.Id = @HomeTeamId");
            dbCommand.CommandText.Should().NotContain("at.Id = @AwayTeamId");
            dbCommand.CommandText.Should().NotContain("m.MatchDate < @MatchDate");

            dbCommand.Parameters.Should().HaveCount(1);
            dbCommand.Parameters["@Type"].Value.Should().Be("League");
        }

        [Test]
        public void Build_returns_correct_dbCommand_given_only_matchDate()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            mockDatabaseConnection
                .Setup(x => x.CreateCommand())
                .Returns(new MockDbCommand());
            var builder = new MatchCommandBuilder();

            var dbCommand = builder.Build(mockDatabaseConnection.Object, null, null, null, null,
                new DateTime(2000, 1, 1));

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Matches] AS m");
            dbCommand.CommandText.Should().Contain("WHERE m.MatchDate < @MatchDate");
            dbCommand.CommandText.Should().NotContain("s.Id = @SeasonId");
            dbCommand.CommandText.Should().NotContain("c.Id = @CompetitionId");
            dbCommand.CommandText.Should().NotContain("ht.Id = @HomeTeamId");
            dbCommand.CommandText.Should().NotContain("at.Id = @AwayTeamId");
            dbCommand.CommandText.Should().NotContain("r.Type = @Type");

            dbCommand.Parameters.Should().HaveCount(1);
            dbCommand.Parameters["@MatchDate"].Value.Should().Be(new DateTime(2000, 1, 1));
        }
    }
}