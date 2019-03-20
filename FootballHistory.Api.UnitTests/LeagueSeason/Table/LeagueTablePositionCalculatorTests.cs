using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.LeagueSeason.Table;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using Moq;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.LeagueSeason.Table
{
    [TestFixture]
    public class LeagueTablePositionCalculatorTests
    {
        private LeagueTablePositionCalculator _leagueTablePositionCalculator;

        [SetUp]
        public void Setup()
        {
            var mockSorter = new Mock<ILeagueTableSorter>();
            mockSorter
                .Setup(x => x.Sort(It.IsAny<LeagueTable>(), It.IsAny<LeagueDetailModel>()))
                .Returns((LeagueTable t, LeagueDetailModel m) => t);
            
            _leagueTablePositionCalculator = new LeagueTablePositionCalculator(mockSorter.Object);
        }
        
        [Test]
        public void AddPositions_CorrectlyCalculatesPositions()
        {
            var leagueTable = new LeagueTable
            {
                Rows = new List<LeagueTableRow>
                {
                    new LeagueTableRow {Team = "Team1"},
                    new LeagueTableRow {Team = "Team2"},
                    new LeagueTableRow {Team = "Team3"}
                }
            };

            var leagueTableWithPositions = _leagueTablePositionCalculator.AddPositions(leagueTable, new LeagueDetailModel());

            var actual = leagueTableWithPositions.Rows.Select(r => (r.Team, r.Position)).ToList();
            var expected = new List<(string, int)> { ("Team1", 1), ("Team2", 2), ("Team3", 3) };
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}