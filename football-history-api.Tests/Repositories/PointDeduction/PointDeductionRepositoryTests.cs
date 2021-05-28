using System.Collections.Generic;
using System.Data;
using FluentAssertions;
using football.history.api.Repositories;
using football.history.api.Repositories.PointDeduction;
using football.history.api.Tests.Repositories.TestUtilities;
using Microsoft.Data.SqlClient.Server;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Repositories.PointDeduction
{
    [TestFixture]
    public class PointDeductionRepositoryTests
    {
        [Test]
        public void GetPointDeductions_returns_empty_list_given_no_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<IPointDeductionCommandBuilder>();
            mockQueryBuilder
                .Setup(x => x.Build(It.IsAny<IDatabaseConnection>(), 0))
                .Returns(new MockDbCommand());
            var repository = new PointDeductionRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var seasons = repository.GetPointDeductions(0);

            mockQueryBuilder.VerifyAll();
            seasons.Should().BeEmpty();
        }
        
        [Test]
        public void GetPointDeductions_returns_list_of_point_deduction_models_given_data()
        {
            var mockDatabaseConnection = new Mock<IDatabaseConnection>();
            var mockQueryBuilder = new Mock<IPointDeductionCommandBuilder>();
            mockQueryBuilder
                .Setup(x => x.Build(It.IsAny<IDatabaseConnection>(), 0))
                .Returns(new MockDbCommand(new List<IDataRecord> { new SqlDataRecord()}));
            var repository = new PointDeductionRepository(mockDatabaseConnection.Object, mockQueryBuilder.Object);

            var seasons = repository.GetPointDeductions(0);

            mockQueryBuilder.VerifyAll();
            seasons.Should().BeEquivalentTo(new List<PointDeductionModel> { new(0, 1, 2, 3, "4", "5")});
        }
    }
}