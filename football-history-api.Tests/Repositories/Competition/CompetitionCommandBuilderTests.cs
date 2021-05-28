using FluentAssertions;
using football.history.api.Repositories;
using football.history.api.Repositories.Competition;
using football.history.api.Tests.Repositories.TestUtilities;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Repositories.Competition
{
    [TestFixture]
    public class CompetitionCommandBuilderTests
    {
        [Test]
        public void Build_returns_correct_dbCommand_given_no_parameters()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            mockDatabaseConnection
                .Setup(x => x.CreateCommand())
                .Returns(new MockDbCommand());
            var builder = new CompetitionCommandBuilder();

            var dbCommand = builder.Build(mockDatabaseConnection.Object);

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Competitions] AS c");
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
            var builder = new CompetitionCommandBuilder();

            var dbCommand = builder.Build(mockDatabaseConnection.Object, 1, 2, 3);

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Competitions] AS c");
            dbCommand.CommandText.Should().Contain("WHERE c.Id = @CompetitionId AND s.Id = @SeasonId AND c.Tier = @Tier");
            dbCommand.Parameters.Should().HaveCount(3);
            dbCommand.Parameters["@CompetitionId"].Value.Should().Be(1);
            dbCommand.Parameters["@SeasonId"].Value.Should().Be(2);
            dbCommand.Parameters["@Tier"].Value.Should().Be(3);
        }    
        
        [Test]
        public void Build_returns_correct_dbCommand_given_only_competitionId()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            mockDatabaseConnection
                .Setup(x => x.CreateCommand())
                .Returns(new MockDbCommand());
            var builder = new CompetitionCommandBuilder();

            var dbCommand = builder.Build(mockDatabaseConnection.Object, 1);

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Competitions] AS c");
            dbCommand.CommandText.Should().Contain("WHERE c.Id = @CompetitionId");
            dbCommand.CommandText.Should().NotContain("s.Id = @SeasonId");
            dbCommand.CommandText.Should().NotContain("c.Tier = @Tier");
            
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
            var builder = new CompetitionCommandBuilder();

            var dbCommand = builder.Build(mockDatabaseConnection.Object, null, 1);

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Competitions] AS c");
            dbCommand.CommandText.Should().Contain("WHERE s.Id = @SeasonId");
            dbCommand.CommandText.Should().NotContain("c.Id = @CompetitionId");
            dbCommand.CommandText.Should().NotContain("c.Tier = @Tier");
            
            dbCommand.Parameters.Should().HaveCount(1);
            dbCommand.Parameters["@SeasonId"].Value.Should().Be(1);
        }
        
        [Test]
        public void Build_returns_correct_dbCommand_given_only_tier()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            mockDatabaseConnection
                .Setup(x => x.CreateCommand())
                .Returns(new MockDbCommand());
            var builder = new CompetitionCommandBuilder();

            var dbCommand = builder.Build(mockDatabaseConnection.Object, null, null, 1);

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Competitions] AS c");
            dbCommand.CommandText.Should().Contain("WHERE c.Tier = @Tier");
            dbCommand.CommandText.Should().NotContain("c.Id = @CompetitionId");
            dbCommand.CommandText.Should().NotContain("s.Id = @SeasonId");
            
            dbCommand.Parameters.Should().HaveCount(1);
            dbCommand.Parameters["@Tier"].Value.Should().Be(1);
        }
        
        [Test]
        public void BuildForCompetitionId_returns_correct_dbCommand()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            mockDatabaseConnection
                .Setup(x => x.CreateCommand())
                .Returns(new MockDbCommand());
            var builder = new CompetitionCommandBuilder();

            var dbCommand = builder.BuildForCompetitionId(mockDatabaseConnection.Object, 1, 2);

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Competitions] AS c");
            dbCommand.CommandText.Should().Contain("FROM [dbo].[Matches] AS m");
            dbCommand.CommandText.Should().Contain("WHERE c.SeasonId = @SeasonId");
            dbCommand.CommandText.Should().Contain("WHERE m.HomeTeamId = @TeamId");
            
            dbCommand.Parameters.Should().HaveCount(2);
            dbCommand.Parameters["@SeasonId"].Value.Should().Be(1);
            dbCommand.Parameters["@TeamId"].Value.Should().Be(2);
        }
    }
}