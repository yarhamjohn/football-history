using FluentAssertions;
using football.history.api.Repositories;
using football.history.api.Repositories.Season;
using football.history.api.Tests.Repositories.TestUtilities;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Repositories.Season
{
    [TestFixture]
    public class SeasonCommandBuilderTests
    {
        [Test]
        public void Season_without_seasonId_returns_correct_dbCommand()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            mockDatabaseConnection
                .Setup(x => x.CreateCommand())
                .Returns(new MockDbCommand());
            var builder = new SeasonCommandBuilder();

            var dbCommand = builder.Build(mockDatabaseConnection.Object);

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Seasons] AS s");
            dbCommand.CommandText.Should().NotContain("WHERE s.Id = @Id");
            dbCommand.Parameters.Should().BeEmpty();
        }
        
        [Test]
        public void Build_with_seasonId_returns_correct_dbCommand()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            mockDatabaseConnection
                .Setup(x => x.CreateCommand())
                .Returns(new MockDbCommand());
            var builder = new SeasonCommandBuilder();

            const int seasonId = 1;
            var dbCommand = builder.Build(mockDatabaseConnection.Object, seasonId);

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Seasons] AS s");
            dbCommand.CommandText.Should().Contain("WHERE s.Id = @Id");
            dbCommand.Parameters.Should().HaveCount(1);
            dbCommand.Parameters["@Id"].Value.Should().Be(seasonId);
        }
    }
}