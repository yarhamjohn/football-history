using System.Collections.Generic;
using System.Data;
using FluentAssertions;
using football.history.api.Exceptions;
using football.history.api.Repositories;
using football.history.api.Repositories.Season;
using football.history.api.Tests.Repositories.TestUtilities;
using Microsoft.Data.SqlClient.Server;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Repositories.Season
{
    [TestFixture]
    public class SeasonRepositoryTests
    {
        [Test]
        public void GetAllSeasons_returns_empty_list_given_no_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ISeasonCommandBuilder>();
            mockQueryBuilder
                .Setup(x => x.Build(It.IsAny<IDatabaseConnection>()))
                .Returns(new MockDbCommand());
            var repository = new SeasonRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var seasons = repository.GetAllSeasons();

            seasons.Should().BeEmpty();
        }
        
        [Test]
        public void GetAllSeasons_returns_list_of_season_models_given_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ISeasonCommandBuilder>();
            mockQueryBuilder
                .Setup(x => x.Build(It.IsAny<IDatabaseConnection>()))
                .Returns(new MockDbCommand(new List<IDataRecord> { new SqlDataRecord()}));
            var repository = new SeasonRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var seasons = repository.GetAllSeasons();

            seasons.Should().BeEquivalentTo(new List<SeasonModel> { new(0, 1, 2)});
        }
        
        [Test]
        public void GetSeason_returns_season_model_given_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ISeasonCommandBuilder>();
            mockQueryBuilder
                .Setup(x => x.Build(It.IsAny<IDatabaseConnection>(), It.IsAny<long>()))
                .Returns(new MockDbCommand(new List<IDataRecord> { new SqlDataRecord()}));
            var repository = new SeasonRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var seasons = repository.GetSeason(0);

            seasons.Should().BeEquivalentTo(new SeasonModel(0, 1,2));
        }
        
        [Test]
        public void GetSeason_throws_given_multiple_matching_seasons()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ISeasonCommandBuilder>();
            mockQueryBuilder
                .Setup(x => x.Build(It.IsAny<IDatabaseConnection>(), 0))
                .Returns(new MockDbCommand(new List<IDataRecord> { new SqlDataRecord(), new SqlDataRecord()}));
            var repository = new SeasonRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var ex = Assert.Throws<DataInvalidException>(() => repository.GetSeason(0));

            mockQueryBuilder.VerifyAll();
            ex.Message.Should().Be("2 seasons matched the specified id (0).");
        }
        
        [Test]
        public void GetSeason_throws_given_no_matching_seasons()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ISeasonCommandBuilder>();
            mockQueryBuilder
                .Setup(x => x.Build(It.IsAny<IDatabaseConnection>(), 0))
                .Returns(new MockDbCommand(new List<IDataRecord>()));
            var repository = new SeasonRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var ex = Assert.Throws<DataNotFoundException>(() => repository.GetSeason(0));

            mockQueryBuilder.VerifyAll();
            ex.Message.Should().Be("No season matched the specified id (0).");
        }
    }
}