using System;
using FluentAssertions;
using football.history.api.Builders;
using football.history.api.Calculators;
using football.history.api.Repositories.League;
using football.history.api.Repositories.Tier;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests
{
    [TestFixture]
    public class LeagueBuilderTests
    {
        private readonly LeagueModel _leagueModel = new();
        private readonly DateTime _seasonEndDate = new(2000, 6, 30);
        private const int CalculatedTier = 0;

        private ILeagueBuilder _leagueBuilder = null!;
        private Mock<ILeagueTableBuilder> _leagueTableBuilder = null!;
        private Mock<ILeagueRepository> _leagueRepository = null!;

        [SetUp]
        public void Setup()
        {
            _leagueRepository = new Mock<ILeagueRepository>();
            _leagueRepository
                .Setup(r => r.GetLeagueModel(It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(_leagueModel);

            _leagueTableBuilder = new Mock<ILeagueTableBuilder>();
            _leagueTableBuilder
                .Setup(r => r.Build(It.IsAny<LeagueModel>(), It.IsAny<DateTime>()));

            var tierRepository = new Mock<ITierRepository>();
            tierRepository
                .Setup(r => r.GetTierForTeamInYear(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(CalculatedTier);

            var dateCalculator = new Mock<IDateCalculator>();
            dateCalculator
                .Setup(r => r.GetSeasonEndDate(It.IsAny<int>()))
                .Returns(_seasonEndDate);

            _leagueBuilder = new LeagueBuilder(
                _leagueRepository.Object,
                _leagueTableBuilder.Object,
                tierRepository.Object,
                dateCalculator.Object);
        }

        [Test]
        public void Build_for_tier_should_use_specified_tier_and_season_end_date_given_start_year()
        {
            const int seasonStartYear = 2000;
            const int tier = 1;

            _leagueBuilder.BuildForTier(seasonStartYear, tier);

            _leagueRepository.Verify(mock => mock.GetLeagueModel(_seasonEndDate, tier), Times.Once());
            _leagueTableBuilder.Verify(mock => mock.Build(_leagueModel, _seasonEndDate), Times.Once());
        }

        [Test]
        public void Build_for_tier_should_use_specified_tier_and_date_if_specified()
        {
            var date = new DateTime(2010, 1, 1);
            const int tier = 1;

            _leagueBuilder.BuildForTier(date, tier);

            _leagueRepository.Verify(mock => mock.GetLeagueModel(date, tier), Times.Once());
            _leagueTableBuilder.Verify(mock => mock.Build(_leagueModel, date), Times.Once());
        }

        [Test]
        public void Build_for_team_should_get_team_tier_and_use_season_end_date_given_start_year()
        {
            const int seasonStartYear = 2000;
            const string team = "team";

            _leagueBuilder.BuildForTeam(seasonStartYear, team);

            _leagueRepository.Verify(mock => mock.GetLeagueModel(_seasonEndDate, CalculatedTier), Times.Once());
            _leagueTableBuilder.Verify(mock => mock.Build(_leagueModel, _seasonEndDate), Times.Once());
        }

        [Test]
        public void Build_for_team_should_get_team_tier_and_use_date_if_specified()
        {
            var date = new DateTime(2010, 1, 1);
            const string team = "team";

            _leagueBuilder.BuildForTeam(date, team);

            _leagueRepository.Verify(mock => mock.GetLeagueModel(date, CalculatedTier), Times.Once());
            _leagueTableBuilder.Verify(mock => mock.Build(_leagueModel, date), Times.Once());
        }
    }
}
