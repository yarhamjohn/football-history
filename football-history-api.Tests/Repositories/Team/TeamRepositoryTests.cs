using System.Collections.Generic;
using System.Data;
using FluentAssertions;
using football.history.api.Exceptions;
using football.history.api.Repositories;
using football.history.api.Repositories.Team;
using football.history.api.Tests.Repositories.TestUtilities;
using Microsoft.Data.SqlClient.Server;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Repositories.Team
{
    [TestFixture]
    public class TeamRepositoryTests
    {
        [Test]
        public void GetAllTeams_returns_empty_list_given_no_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ITeamCommandBuilder>();
            mockQueryBuilder
                .Setup(x => x.Build(It.IsAny<IDatabaseConnection>()))
                .Returns(new MockDbCommand());
            var repository = new TeamRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var teams = repository.GetAllTeams();

            mockQueryBuilder.VerifyAll();
            teams.Should().BeEmpty();
        }
        
        [Test]
        public void GetAllTeams_returns_list_of_team_models_given_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ITeamCommandBuilder>();
            mockQueryBuilder
                .Setup(x => x.Build(It.IsAny<IDatabaseConnection>()))
                .Returns(new MockDbCommand(new List<IDataRecord> { new SqlDataRecord()}));
            var repository = new TeamRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var teams = repository.GetAllTeams();

            mockQueryBuilder.VerifyAll();
            teams.Should().BeEquivalentTo(new List<TeamModel> { new(0, "1", "2", "3")});
        }
        
        [Test]
        public void GetTeam_returns_team_model_given_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ITeamCommandBuilder>();
            mockQueryBuilder
                .Setup(x => x.Build(It.IsAny<IDatabaseConnection>(), 0))
                .Returns(new MockDbCommand(new List<IDataRecord> { new SqlDataRecord()}));
            var repository = new TeamRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var teams = repository.GetTeam(0);

            mockQueryBuilder.VerifyAll();
            teams.Should().BeEquivalentTo(new TeamModel(0, "1", "2", "3"));
        }
        
        [Test]
        public void GetTeam_throws_given_multiple_matching_teams()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ITeamCommandBuilder>();
            mockQueryBuilder
                .Setup(x => x.Build(It.IsAny<IDatabaseConnection>(), 0))
                .Returns(new MockDbCommand(new List<IDataRecord> { new SqlDataRecord(), new SqlDataRecord()}));
            var repository = new TeamRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var ex = Assert.Throws<DataInvalidException>(() => repository.GetTeam(0));

            mockQueryBuilder.VerifyAll();
            ex.Message.Should().Be("2 teams matched the specified id (0).");
        }
        
        [Test]
        public void GetTeam_throws_given_no_matching_teams()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ITeamCommandBuilder>();
            mockQueryBuilder
                .Setup(x => x.Build(It.IsAny<IDatabaseConnection>(), 0))
                .Returns(new MockDbCommand(new List<IDataRecord>()));
            var repository = new TeamRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var ex = Assert.Throws<DataNotFoundException>(() => repository.GetTeam(0));

            mockQueryBuilder.VerifyAll();
            ex.Message.Should().Be("No team matched the specified id (0).");
        }
    }
}