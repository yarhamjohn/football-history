using FluentAssertions;
using football.history.api.Repositories;
using football.history.api.Repositories.Team;
using football.history.api.Tests.Repositories.TestUtilities;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Repositories.Team
{
    [TestFixture]
    public class TeamCommandBuilderTests
    {
        [Test]
        public void Build_without_teamId_returns_correct_dbCommand()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            mockDatabaseConnection
                .Setup(x => x.CreateCommand())
                .Returns(new MockDbCommand());
            var builder = new TeamCommandBuilder();

            var dbCommand = builder.Build(mockDatabaseConnection.Object);

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Teams] AS t");
            dbCommand.CommandText.Should().NotContain("WHERE t.Id = @Id");
            dbCommand.Parameters.Should().BeEmpty();
        }
        
        [Test]
        public void Build_with_teamId_returns_correct_dbCommand()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            mockDatabaseConnection
                .Setup(x => x.CreateCommand())
                .Returns(new MockDbCommand());
            var builder = new TeamCommandBuilder();

            const int teamId = 1;
            var dbCommand = builder.Build(mockDatabaseConnection.Object, teamId);

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Teams] AS t");
            dbCommand.CommandText.Should().Contain("WHERE t.Id = @Id");
            dbCommand.Parameters.Should().HaveCount(1);
            dbCommand.Parameters["@Id"].Value.Should().Be(teamId);
        }
    }
}