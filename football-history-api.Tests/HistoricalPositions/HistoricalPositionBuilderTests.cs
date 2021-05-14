using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using football.history.api.Builders;
using football.history.api.Exceptions;
using football.history.api.Repositories.Competition;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.HistoricalPositions
{
    [TestFixture]
    public class HistoricalPositionBuilderTests
    {
        [Test]
        public void Build_should_throw_if_no_competitions_found_for_season()
        {
            var mockLeagueTableBuilder = new Mock<ILeagueTableBuilder>();

            var mockCompetitionRepository = new Mock<ICompetitionRepository>();
            mockCompetitionRepository
                .Setup(x => x.GetCompetitionsInSeason(1))
                .Returns(Array.Empty<CompetitionModel>());

            var builder = new HistoricalPositionBuilder(
                    mockCompetitionRepository.Object, 
                    mockLeagueTableBuilder.Object);

            var exception = Assert.Throws<DataNotFoundException>(() => builder.Build(1, 1));

            exception.Message.Should().Be("No competitions were found for the requested season (1)");
        }

        [Test]
        public void Build_should_return_EmptyHistoricalPosition_given_team_not_in_season_competition()
        {
            var competitionModels = GetTestCompetitionModels();

            var mockLeagueTableBuilder = new Mock<ILeagueTableBuilder>();

            var mockCompetitionRepository = new Mock<ICompetitionRepository>();
            mockCompetitionRepository
                .Setup(x => x.GetCompetitionsInSeason(1))
                .Returns(competitionModels);
            mockCompetitionRepository
                .Setup(x => x.GetCompetitionForSeasonAndTeam(1, 1))
                .Returns((CompetitionModel?) null);

            var builder = new HistoricalPositionBuilder(
                mockCompetitionRepository.Object, 
                mockLeagueTableBuilder.Object);
            
            var result = builder.Build(1, 1);

            result.Should().BeOfType<EmptyHistoricalPosition>();
        }

        [Test]
        public void Build_should_return_PopulatedHistoricalPosition_given_team_in_season_competition()
        {
            var competitionModels = GetTestCompetitionModels();

            var mockLeagueTable = new Mock<ILeagueTable>();
            mockLeagueTable
                .Setup(x => x.GetRow(1))
                .Returns(new LeagueTableRowDto());

            var mockLeagueTableBuilder = new Mock<ILeagueTableBuilder>();
            mockLeagueTableBuilder
                .Setup(x => x.BuildFullLeagueTable(It.IsAny<CompetitionModel>()))
                .Returns(mockLeagueTable.Object);

            var mockCompetitionRepository = new Mock<ICompetitionRepository>();
            mockCompetitionRepository
                .Setup(x => x.GetCompetitionsInSeason(1))
                .Returns(competitionModels);
            mockCompetitionRepository
                .Setup(x => x.GetCompetitionForSeasonAndTeam(1, 1))
                .Returns(competitionModels.First());

            var builder =
                new HistoricalPositionBuilder(mockCompetitionRepository.Object, mockLeagueTableBuilder.Object);

            var result = builder.Build(1, 1);

            result.Should().BeOfType<PopulatedHistoricalPosition>();
        }

        private static List<CompetitionModel> GetTestCompetitionModels() =>
            new()
            {
                new(
                    0,
                    "Premier League",
                    1,
                    2000,
                    2001,
                    1,
                    null,
                    null,
                    0,
                    20,
                    0,
                    0,
                    0,
                    0,
                    0,
                    null),
                new(
                    1,
                    "Championship",
                    1,
                    2000,
                    2001,
                    2,
                    null,
                    null,
                    0,
                    24,
                    0,
                    0,
                    0,
                    0,
                    0,
                    null)
            };
    }
}