using FluentAssertions;
using football.history.api.Repositories;
using football.history.api.Repositories.PointDeduction;
using football.history.api.Tests.Repositories.TestUtilities;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Repositories.PointDeduction
{
    [TestFixture]
    public class PointDeductionCommandBuilderTests
    {
        [Test]
        public void Build_returns_correct_dbCommand()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            mockDatabaseConnection
                .Setup(x => x.CreateCommand())
                .Returns(new MockDbCommand());
            var builder = new PointDeductionCommandBuilder();

            const int competitionId = 1;
            var dbCommand = builder.Build(mockDatabaseConnection.Object, competitionId);

            dbCommand.CommandText.Should().Contain("FROM [dbo].[Deductions] AS d");
            dbCommand.CommandText.Should().Contain("WHERE d.CompetitionId = @CompetitionId");
            dbCommand.Parameters.Should().HaveCount(1);
            dbCommand.Parameters["@CompetitionId"].Value.Should().Be(competitionId);
        }
    }
}