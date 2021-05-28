using System;
using System.Collections.Generic;
using System.Data;
using FluentAssertions;
using football.history.api.Exceptions;
using football.history.api.Repositories;
using football.history.api.Repositories.Match;
using football.history.api.Tests.Repositories.TestUtilities;
using Microsoft.Data.SqlClient.Server;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Repositories.Match
{
    [TestFixture]
    public class MatchRepositoryTests
    {
        [Test]
        public void GetMatches_returns_empty_list_given_no_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<IMatchCommandBuilder>();
            mockQueryBuilder
                .Setup(x =>
                    x.Build(It.IsAny<IDatabaseConnection>(), null, null, null, null, null))
                .Returns(new MockDbCommand());
            var repository = new MatchRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var matches = repository.GetMatches();

            mockQueryBuilder.VerifyAll();
            matches.Should().BeEmpty();
        }

        [Test]
        public void GetMatches_returns_list_of_match_models_given_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<IMatchCommandBuilder>();
            mockQueryBuilder
                .Setup(x =>
                    x.Build(It.IsAny<IDatabaseConnection>(), null, null, null, null, null))
                .Returns(new MockDbCommand(new List<IDataRecord> {new SqlDataRecord()}));
            var repository = new MatchRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var matches = repository.GetMatches();

            mockQueryBuilder.VerifyAll();
            matches.Should().BeEquivalentTo(
                new List<MatchModel>
                {
                    new(0,
                        new DateTime(1, 1, 1),
                        2,
                        "3",
                        4,
                        5,
                        6,
                        "7",
                        "8",
                        "9",
                        false,
                        false,
                        12,
                        false,
                        false,
                        15,
                        "16",
                        "17",
                        18,
                        "19",
                        "20",
                        21,
                        22,
                        23,
                        24,
                        25,
                        26,
                        27,
                        28)
                });
        }

        [Test]
        public void GetLeagueMatches_returns_empty_list_given_no_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<IMatchCommandBuilder>();
            mockQueryBuilder
                .Setup(x =>
                    x.Build(It.IsAny<IDatabaseConnection>(), 0, null, null, "League", null))
                .Returns(new MockDbCommand());
            var repository = new MatchRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var matches = repository.GetLeagueMatches(0);

            mockQueryBuilder.VerifyAll();
            matches.Should().BeEmpty();
        }

        [Test]
        public void GetLeagueMatches_returns_list_of_match_models_given_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<IMatchCommandBuilder>();
            mockQueryBuilder
                .Setup(x =>
                    x.Build(It.IsAny<IDatabaseConnection>(), 0, null, null, "League", null))
                .Returns(new MockDbCommand(new List<IDataRecord> {new SqlDataRecord()}));
            var repository = new MatchRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var matches = repository.GetLeagueMatches(0);

            mockQueryBuilder.VerifyAll();
            matches.Should().BeEquivalentTo(
                new List<MatchModel>
                {
                    new(0,
                        new DateTime(1, 1, 1),
                        2,
                        "3",
                        4,
                        5,
                        6,
                        "7",
                        "8",
                        "9",
                        false,
                        false,
                        12,
                        false,
                        false,
                        15,
                        "16",
                        "17",
                        18,
                        "19",
                        "20",
                        21,
                        22,
                        23,
                        24,
                        25,
                        26,
                        27,
                        28)
                });
        }

        [Test]
        public void GetPlayOffMatches_returns_empty_list_given_no_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<IMatchCommandBuilder>();
            mockQueryBuilder
                .Setup(x =>
                    x.Build(It.IsAny<IDatabaseConnection>(), 0, null, null, "PlayOff", null))
                .Returns(new MockDbCommand());
            var repository = new MatchRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var matches = repository.GetPlayOffMatches(0);

            mockQueryBuilder.VerifyAll();
            matches.Should().BeEmpty();
        }

        [Test]
        public void GetPlayOffMatches_returns_list_of_match_models_given_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<IMatchCommandBuilder>();
            mockQueryBuilder
                .Setup(x =>
                    x.Build(It.IsAny<IDatabaseConnection>(), 0, null, null, "PlayOff", null))
                .Returns(new MockDbCommand(new List<IDataRecord> {new SqlDataRecord()}));
            var repository = new MatchRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var matches = repository.GetPlayOffMatches(0);

            mockQueryBuilder.VerifyAll();
            matches.Should().BeEquivalentTo(
                new List<MatchModel>
                {
                    new(0,
                        new DateTime(1, 1, 1),
                        2,
                        "3",
                        4,
                        5,
                        6,
                        "7",
                        "8",
                        "9",
                        false,
                        false,
                        12,
                        false,
                        false,
                        15,
                        "16",
                        "17",
                        18,
                        "19",
                        "20",
                        21,
                        22,
                        23,
                        24,
                        25,
                        26,
                        27,
                        28)
                });
        }

        [Test]
        public void GetMatch_returns_match_model_given_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<IMatchCommandBuilder>();
            mockQueryBuilder
                .Setup(x => x.Build(It.IsAny<IDatabaseConnection>(), 0))
                .Returns(new MockDbCommand(new List<IDataRecord> {new SqlDataRecord()}));
            var repository = new MatchRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var matches = repository.GetMatch(0);

            mockQueryBuilder.VerifyAll();
            matches.Should().BeEquivalentTo(
                new MatchModel(0,
                    new DateTime(1, 1, 1),
                    2,
                    "3",
                    4,
                    5,
                    6,
                    "7",
                    "8",
                    "9",
                    false,
                    false,
                    12,
                    false,
                    false,
                    15,
                    "16",
                    "17",
                    18,
                    "19",
                    "20",
                    21,
                    22,
                    23,
                    24,
                    25,
                    26,
                    27,
                    28)
            );
        }

        [Test]
        public void GetMatch_throws_given_multiple_matching_matches()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<IMatchCommandBuilder>();
            mockQueryBuilder
                .Setup(x => x.Build(It.IsAny<IDatabaseConnection>(), 0))
                .Returns(new MockDbCommand(new List<IDataRecord> {new SqlDataRecord(), new SqlDataRecord()}));
            var repository = new MatchRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var ex = Assert.Throws<DataInvalidException>(() => repository.GetMatch(0));

            mockQueryBuilder.VerifyAll();
            ex.Message.Should().Be("2 matches matched the specified id (0).");
        }

        [Test]
        public void GetMatch_throws_given_no_matching_matches()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<IMatchCommandBuilder>();
            mockQueryBuilder
                .Setup(x => x.Build(It.IsAny<IDatabaseConnection>(), 0))
                .Returns(new MockDbCommand(new List<IDataRecord>()));
            var repository = new MatchRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var ex = Assert.Throws<DataNotFoundException>(() => repository.GetMatch(0));

            mockQueryBuilder.VerifyAll();
            ex.Message.Should().Be("No match matched the specified id (0).");
        }
    }
}