using System.Linq;
using FluentAssertions;
using football.history.api.Builders;
using football.history.api.Repositories.Competition;
using NUnit.Framework;

namespace football.history.api.Tests.HistoricalPositions
{
    [TestFixture]
    public class HistoricalPositionTests
    {
        [Test]
        public void EmptyHistoricalPosition_should_return_correct_dto()
        {
            var competitionsInSeason = GetTestCompetitionModels();
            var competition = competitionsInSeason.Single(x => x.Id == 0);

            var historicalPosition = new EmptyHistoricalPosition(competitionsInSeason, competition);

            var result = historicalPosition.ToDto();

            result.Competitions.Should()
                .BeEquivalentTo(competitionsInSeason.Select(CompetitionModelExtensions.ToCompetitionDto));
            result.Team.Should().BeNull();
            result.SeasonId.Should().Be(1);
            result.SeasonStartYear.Should().Be(2000);
        }

        [TestCase(0, 1)]
        [TestCase(1, 21)]
        public void PopulatedHistoricalPosition_should_return_correct_dto(long id, int absolutePosition)
        {
            var competitionsInSeason = GetTestCompetitionModels();
            var competition = competitionsInSeason.Single(x => x.Id == id);
            var teamRow = GetTestLeagueTableRow();

            var historicalPosition = new PopulatedHistoricalPosition(competitionsInSeason, competition, teamRow);

            var result = historicalPosition.ToDto();

            result.Competitions.Should()
                .BeEquivalentTo(competitionsInSeason.Select(CompetitionModelExtensions.ToCompetitionDto));
            result.SeasonId.Should().Be(1);
            result.SeasonStartYear.Should().Be(2000);

            result.Team!.CompetitionId.Should().Be(id);
            result.Team.Status.Should().Be(teamRow.Status);
            result.Team.Position.Should().Be(teamRow.Position);
            result.Team.AbsolutePosition.Should().Be(absolutePosition);
        }

        private static LeagueTableRowDto GetTestLeagueTableRow()
        {
            return new()
            {
                Position = 1,
                TeamId   = 1,
                Team     = "Norwich City",
                Status   = "Champions",
            };
        }

        private static CompetitionModel[] GetTestCompetitionModels() =>
            new CompetitionModel[]
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