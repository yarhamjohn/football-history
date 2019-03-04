using System.Collections.Generic;
using FootballHistory.Api.LeagueSeason.LeagueTable;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using FootballHistory.Api.Repositories.PointDeductionRepository;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.LeagueSeason.LeagueTable
{
    [TestFixture]
    public class LeagueTableCalculatorFactoryTests
    {
        [Test]
        public void Create_ReturnsALeagueTableCalculator()
        {
            var factory = new LeagueTableCalculatorFactory();

            var calculator = factory.Create(new List<MatchDetailModel>(), new List<PointDeductionModel>(), "");
            
            Assert.That(calculator, Is.InstanceOf<LeagueTableCalculator>());
        }
    }
}
