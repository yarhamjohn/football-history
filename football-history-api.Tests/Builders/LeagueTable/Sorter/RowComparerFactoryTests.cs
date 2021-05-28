using FluentAssertions;
using football.history.api.Builders;
using football.history.api.Repositories.Competition;
using NUnit.Framework;

namespace football.history.api.Tests.Builders.LeagueTable.Sorter
{
    [TestFixture]
    public class RowComparerFactoryTests
    {
        [TestCase(2, 1992, 1993)]
        [TestCase(3, 1994, 1995)]
        [TestCase(4, 1997, 1998)]
        public void Gets_Points_GoalsFor_GoalDiff_comparer(int tier, int startYear, int endYear)
        {
            var competition = new CompetitionModel(
                0, 
                "name", 
                0, 
                startYear, 
                endYear, 
                tier, 
                null, 
                null, 
                0, 
                0,
                0, 
                0,
                0,
                0,
                0,
                null);
            var factory = new RowComparerFactory();

            var comparer = factory.GetLeagueTableComparer(competition);

            comparer.Should().BeOfType<PointsGoalsForGoalDiffComparer>();
        }
        
        [TestCase(1, 1992, 1993)]
        [TestCase(1, 1994, 1995)]
        [TestCase(1, 1997, 1998)]
        public void Doesnt_get_Points_GoalsFor_GoalDiff_comparer(int tier, int startYear, int endYear)
        {
            var competition = new CompetitionModel(
                0, 
                "name", 
                0, 
                startYear, 
                endYear, 
                tier, 
                null, 
                null, 
                0, 
                0,
                0, 
                0,
                0,
                0,
                0,
                null);
            var factory = new RowComparerFactory();

            var comparer = factory.GetLeagueTableComparer(competition);

            comparer.Should().BeOfType<PointsGoalDiffGoalsForComparer>();
        }
        
        [TestCase(1, 1888, 1889)]
        [TestCase(2, 1920, 1921)]
        [TestCase(3, 1951, 1952)]
        [TestCase(3, 1975, 1976)]
        public void Gets_Points_GoalsAvg_comparer(int tier, int startYear, int endYear)
        {
            var competition = new CompetitionModel(
                0, 
                "name", 
                0, 
                startYear, 
                endYear, 
                tier, 
                null, 
                null, 
                0, 
                0,
                0, 
                0,
                0,
                0,
                0,
                null);
            var factory = new RowComparerFactory();

            var comparer = factory.GetLeagueTableComparer(competition);

            comparer.Should().BeOfType<PointsGoalAvgComparer>();
        }
        
        [TestCase(3, 2019, 2020)]
        [TestCase(4, 2019, 2020)]
        public void Gets_Covid_affected_season_comparer(int tier, int startYear, int endYear)
        {
            var competition = new CompetitionModel(
                0, 
                "name", 
                0, 
                startYear, 
                endYear, 
                tier, 
                null, 
                null, 
                0, 
                0,
                0, 
                0,
                0,
                0,
                0,
                null);
            var factory = new RowComparerFactory();

            var comparer = factory.GetLeagueTableComparer(competition);

            comparer.Should().BeOfType<PointsPerGameGoalDiffGoalsForComparer>();
        }
        
        [TestCase(1, 2019, 2020)]
        [TestCase(2, 2019, 2020)]
        public void Doesnt_get_Covid_affected_season_comparer(int tier, int startYear, int endYear)
        {
            var competition = new CompetitionModel(
                0, 
                "name", 
                0, 
                startYear, 
                endYear, 
                tier, 
                null, 
                null, 
                0, 
                0,
                0, 
                0,
                0,
                0,
                0,
                null);
            var factory = new RowComparerFactory();

            var comparer = factory.GetLeagueTableComparer(competition);

            comparer.Should().BeOfType<PointsGoalDiffGoalsForComparer>();
        }
        
        [TestCase(1, 1985, 1986)]
        [TestCase(2, 1999, 2000)]
        [TestCase(3, 2010, 2011)]
        [TestCase(4, 2020, 2021)]
        public void Gets_Points_GoalDiff_GoalsFor_comparer(int tier, int startYear, int endYear)
        {
            var competition = new CompetitionModel(
                0, 
                "name", 
                0, 
                startYear, 
                endYear, 
                tier, 
                null, 
                null, 
                0, 
                0,
                0, 
                0,
                0,
                0,
                0,
                null);
            var factory = new RowComparerFactory();

            var comparer = factory.GetLeagueTableComparer(competition);

            comparer.Should().BeOfType<PointsGoalDiffGoalsForComparer>();
        }
    }
}