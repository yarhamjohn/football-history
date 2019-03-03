using System.Collections.Generic;
using FootballHistory.Api.Builders;
using FootballHistory.Api.Repositories.Models;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.BuildersTests
{
    [TestFixture]
    public class LeagueTableCalculatorFactoryTests
    {
        [Test]
        public void Create_ReturnsALeagueTableCalculator()
        {
            var factory = new LeagueTableCalculatorFactory();

            var calculator = factory.Create(new List<MatchDetailModel>(), "");
            
            Assert.That(calculator, Is.InstanceOf<LeagueTableCalculator>());
        }
    }
}
