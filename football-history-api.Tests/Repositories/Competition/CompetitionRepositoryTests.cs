using System;
using System.Collections.Generic;
using System.Data;
using FluentAssertions;
using football.history.api.Exceptions;
using football.history.api.Repositories;
using football.history.api.Repositories.Competition;
using football.history.api.Tests.Repositories.TestUtilities;
using Microsoft.Data.SqlClient.Server;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Repositories.Competition
{
    [TestFixture]
    public class CompetitionRepositoryTests
    {
        [Test]
        public void GetAllCompetitions_returns_empty_list_given_no_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ICompetitionCommandBuilder>();
            mockQueryBuilder
                .Setup(x => 
                    x.Build(It.IsAny<IDatabaseConnection>(), null, null, null))
                .Returns(new MockDbCommand());
            var repository = new CompetitionRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var competitions = repository.GetAllCompetitions();

            mockQueryBuilder.VerifyAll();
            competitions.Should().BeEmpty();
        }

        [Test]
        public void GetAllCompetitions_returns_list_of_competition_models_given_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ICompetitionCommandBuilder>();
            mockQueryBuilder
                .Setup(x => 
                    x.Build(It.IsAny<IDatabaseConnection>(), null, null, null))
                .Returns(new MockDbCommand(new List<IDataRecord> {new SqlDataRecord()}));
            var repository = new CompetitionRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var competitions = repository.GetAllCompetitions();

            mockQueryBuilder.VerifyAll();
            competitions.Should().BeEquivalentTo(
                new List<CompetitionModel>
                {
                    new(0,
                        "1",
                        2,
                        3,
                        4,
                        5,
                        "6",
                        "7",
                        8,
                        9,
                        10,
                        11,
                        12,
                        13,
                        14,
                        15)
                });
        }

        [Test]
        public void GetCompetitions_returns_empty_list_given_no_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ICompetitionCommandBuilder>();
            mockQueryBuilder
                .Setup(x => 
                    x.Build(It.IsAny<IDatabaseConnection>(), null, 0, null))
                .Returns(new MockDbCommand());
            var repository = new CompetitionRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var competitions = repository.GetCompetitionsInSeason(0);

            mockQueryBuilder.VerifyAll();
            competitions.Should().BeEmpty();
        }

        [Test]
        public void GetCompetitions_returns_list_of_competition_models_given_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ICompetitionCommandBuilder>();
            mockQueryBuilder
                .Setup(x => 
                    x.Build(It.IsAny<IDatabaseConnection>(), null, 0, null))
                .Returns(new MockDbCommand(new List<IDataRecord> {new SqlDataRecord()}));
            var repository = new CompetitionRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var competitions = repository.GetCompetitionsInSeason(0);

            mockQueryBuilder.VerifyAll();
            competitions.Should().BeEquivalentTo(
                new List<CompetitionModel>
                {
                    new(0,
                        "1",
                        2,
                        3,
                        4,
                        5,
                        "6",
                        "7",
                        8,
                        9,
                        10,
                        11,
                        12,
                        13,
                        14,
                        15)
                });
        }

        [Test]
        public void GetCompetition_for_competitionId_returns_competition_model_given_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ICompetitionCommandBuilder>();
            mockQueryBuilder
                .Setup(x => 
                    x.Build(It.IsAny<IDatabaseConnection>(), 0, null, null))
                .Returns(new MockDbCommand(new List<IDataRecord> {new SqlDataRecord()}));
            var repository = new CompetitionRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var competitions = repository.GetCompetition(0);

            mockQueryBuilder.VerifyAll();
            competitions.Should().BeEquivalentTo(
                new CompetitionModel(0,
                    "1",
                    2,
                    3,
                    4,
                    5,
                    "6",
                    "7",
                    8,
                    9,
                    10,
                    11,
                    12,
                    13,
                    14,
                    15)
            );
        }

        [Test]
        public void GetCompetition_for_competitionId_throws_given_multiple_matching_competitions()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ICompetitionCommandBuilder>();
            mockQueryBuilder
                .Setup(x => 
                    x.Build(It.IsAny<IDatabaseConnection>(), 0, null, null))
                .Returns(new MockDbCommand(new List<IDataRecord> {new SqlDataRecord(), new SqlDataRecord()}));
            var repository = new CompetitionRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var ex = Assert.Throws<DataInvalidException>(() => repository.GetCompetition(0));

            mockQueryBuilder.VerifyAll();
            ex.Message.Should().Be("2 competitions matched the specified id (0).");
        }

        [Test]
        public void GetCompetition_for_competitionId_throws_given_no_matching_competitions()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ICompetitionCommandBuilder>();
            mockQueryBuilder
                .Setup(x => 
                    x.Build(It.IsAny<IDatabaseConnection>(), 0, null, null))
                .Returns(new MockDbCommand(new List<IDataRecord>()));
            var repository = new CompetitionRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var ex = Assert.Throws<DataNotFoundException>(() => repository.GetCompetition(0));

            mockQueryBuilder.VerifyAll();
            ex.Message.Should().Be("No competition matched the specified id (0).");
        }

        [Test]
        public void GetCompetition_for_seasonId_and_tier_returns_competition_model_given_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ICompetitionCommandBuilder>();
            mockQueryBuilder
                .Setup(x => 
                    x.Build(It.IsAny<IDatabaseConnection>(), null, 0, 1))
                .Returns(new MockDbCommand(new List<IDataRecord> {new SqlDataRecord()}));
            var repository = new CompetitionRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var competitions = repository.GetCompetitionForSeasonAndTier(0, 1);

            mockQueryBuilder.VerifyAll();
            competitions.Should().BeEquivalentTo(
                new CompetitionModel(0,
                    "1",
                    2,
                    3,
                    4,
                    5,
                    "6",
                    "7",
                    8,
                    9,
                    10,
                    11,
                    12,
                    13,
                    14,
                    15)
            );
        }

        [Test]
        public void GetCompetition_for_seasonId_and_tier_throws_given_multiple_matching_competitions()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ICompetitionCommandBuilder>();
            mockQueryBuilder
                .Setup(x => 
                    x.Build(It.IsAny<IDatabaseConnection>(), null, 0, 1))
                .Returns(new MockDbCommand(new List<IDataRecord> {new SqlDataRecord(), new SqlDataRecord()}));
            var repository = new CompetitionRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var ex = Assert.Throws<DataInvalidException>(() => repository.GetCompetitionForSeasonAndTier(0, 1));

            mockQueryBuilder.VerifyAll();
            ex.Message.Should().Be("2 competitions matched the specified seasonId (0) and tier (1).");
        }

        [Test]
        public void GetCompetition_for_seasonId_and_tier_throws_given_no_matching_competitions()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ICompetitionCommandBuilder>();
            mockQueryBuilder
                .Setup(x => 
                    x.Build(It.IsAny<IDatabaseConnection>(), null, 0, 1))
                .Returns(new MockDbCommand(new List<IDataRecord>()));
            var repository = new CompetitionRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var ex = Assert.Throws<DataNotFoundException>(() => repository.GetCompetitionForSeasonAndTier(0, 1));

            mockQueryBuilder.VerifyAll();
            ex.Message.Should().Be("No competition matched the specified seasonId (0) and tier (1).");
        }

        [Test]
        public void GetCompetition_for_seasonId_and_teamId_returns_competition_model_given_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ICompetitionCommandBuilder>();
            mockQueryBuilder
                .Setup(x => 
                    x.BuildForCompetitionId(It.IsAny<IDatabaseConnection>(), 0, 1))
                .Returns(new MockDbCommand(null, 2));
            mockQueryBuilder
                .Setup(x => 
                    x.Build(It.IsAny<IDatabaseConnection>(), 2, null, null))
                .Returns(new MockDbCommand(new List<IDataRecord> {new SqlDataRecord()}));
            var repository = new CompetitionRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var competitions = repository.GetCompetitionForSeasonAndTeam(0, 1L);

            mockQueryBuilder.VerifyAll();
            competitions.Should().BeEquivalentTo(
                new CompetitionModel(0,
                    "1",
                    2,
                    3,
                    4,
                    5,
                    "6",
                    "7",
                    8,
                    9,
                    10,
                    11,
                    12,
                    13,
                    14,
                    15)
            );
        }

        [Test]
        public void GetCompetition_for_seasonId_and_teamId_returns_null_given_no_competitionId()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ICompetitionCommandBuilder>();
            mockQueryBuilder
                .Setup(x => 
                    x.BuildForCompetitionId(It.IsAny<IDatabaseConnection>(), 0, 1))
                .Returns(new MockDbCommand());
            var repository = new CompetitionRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var result = repository.GetCompetitionForSeasonAndTeam(0, 1L);

            mockQueryBuilder.VerifyAll();
            result.Should().BeNull();
        }

        [Test]
        public void GetCompetition_for_seasonId_and_teamId_returns_null_given_no_matching_competition()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ICompetitionCommandBuilder>();
            mockQueryBuilder
                .Setup(x => 
                    x.BuildForCompetitionId(It.IsAny<IDatabaseConnection>(), 0, 1))
                .Returns(new MockDbCommand(null, 2));
            mockQueryBuilder
                .Setup(x => 
                    x.Build(It.IsAny<IDatabaseConnection>(), 2, null, null))
                .Returns(new MockDbCommand());
            var repository = new CompetitionRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var result = repository.GetCompetitionForSeasonAndTeam(0, 1L);

            mockQueryBuilder.VerifyAll();
            result.Should().BeNull();
        }

        [Test]
        public void GetCompetition_for_seasonId_and_teamId_throws_given_multiple_matching_competitions()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<ICompetitionCommandBuilder>();
            mockQueryBuilder
                .Setup(x => 
                    x.BuildForCompetitionId(It.IsAny<IDatabaseConnection>(), 0, 1))
                .Returns(new MockDbCommand(null, 2));
            mockQueryBuilder
                .Setup(x => 
                    x.Build(It.IsAny<IDatabaseConnection>(), 2, null, null))
                .Returns(new MockDbCommand(new List<IDataRecord> {new SqlDataRecord(), new SqlDataRecord()}));

            var repository = new CompetitionRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var ex = Assert.Throws<DataInvalidException>(() => repository.GetCompetitionForSeasonAndTeam(0, 1L));

            mockQueryBuilder.VerifyAll();
            ex.Message.Should().Be("2 competitions matched the specified id (2).");
        }
    }
}