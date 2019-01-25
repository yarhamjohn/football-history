using System.Collections.Generic;
using FootballHistory.Server.Builders;
using FootballHistory.Server.Domain.Models;
using NUnit.Framework;

namespace FootballHistoryUnitTests.Server.Builders
{
    [TestFixture]
    public class ResultMatrixBuilderTests
    {
        private IResultMatrixBuilder _resultMatrixBuilder;
        
        [SetUp]
        public void SetUp()
        {
            _resultMatrixBuilder = new ResultMatrixBuilder(); 
        }

        [Test]
        public void ResultMatrix_IsEmpty_GivenNoMatches()
        {
            var matchDetails = new List<MatchDetailModel>();

            var resultMatrix = _resultMatrixBuilder.Build(matchDetails);
            
            Assert.That(resultMatrix.Rows, Is.Empty);
        }
    }
}
