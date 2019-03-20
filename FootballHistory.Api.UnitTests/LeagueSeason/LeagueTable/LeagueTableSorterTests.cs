using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.LeagueSeason.LeagueTable;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.LeagueSeason.LeagueTable
{
    [TestFixture]
    public class LeagueTableSorterTests
    {
        private LeagueTableSorter _leagueTableSorter;
        private Api.LeagueSeason.LeagueTable.LeagueTable _leagueTable;
        private List<string> _sortedByPointsThenGoalDifferenceThenGoalsForThenTeamName;
        private List<string> _sortedByPointsThenGoalsForThenGoalDifferenceThenTeamName;

        [SetUp]
        public void Setup()
        {
            _leagueTableSorter = new LeagueTableSorter();
            
            _leagueTable = new Api.LeagueSeason.LeagueTable.LeagueTable
                {
                    Rows = new List<LeagueTableRow>
                    {
                        new LeagueTableRow {Team = "Team1", Points = 3, GoalDifference = 0, GoalsFor = 0},
                        new LeagueTableRow {Team = "Team2", Points = 3, GoalDifference = 0, GoalsFor = 0},
                        new LeagueTableRow {Team = "Team3", Points = 1, GoalDifference = 2, GoalsFor = 1},
                        new LeagueTableRow {Team = "Team4", Points = 1, GoalDifference = 1, GoalsFor = 2},
                        new LeagueTableRow {Team = "Team5", Points = 2, GoalDifference = -1, GoalsFor = -1}
                    }
                };

            _sortedByPointsThenGoalDifferenceThenGoalsForThenTeamName =
                new List<string> { "Team1", "Team2", "Team5", "Team3", "Team4" };

            _sortedByPointsThenGoalsForThenGoalDifferenceThenTeamName =
                new List<string> { "Team1", "Team2", "Team5", "Team4", "Team3" };
        }

        [Test]
        public void Sort_OrdersTopTierLeagueTable_ByPointsThenGoalDifferenceThenGoalsForThenTeamName_Between1992And2017([Range(1992, 2017, 1)] int seasonStartYear)
        {
            var season = $"{seasonStartYear} - {seasonStartYear + 1}";
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 5, Competition = "Premier League", Season = season};
            
            var sortedLeagueTable = _leagueTableSorter.Sort(_leagueTable, leagueDetailModel);
            var actualSortOrder = sortedLeagueTable.Rows.Select(r => r.Team).ToList();

            Assert.That(actualSortOrder, Is.EqualTo(_sortedByPointsThenGoalDifferenceThenGoalsForThenTeamName));
        }
                
        [Test]
        public void Sort_OrdersSecondTierLeagueTable_ByPointsThenGoalsForThenGoalDifferenceThenTeamName_Between1992And1998([Range(1992, 1998, 1)] int seasonStartYear)
        {
            var season = $"{seasonStartYear} - {seasonStartYear + 1}";
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 5, Competition = "First Division", Season = season};

            var sortedLeagueTable = _leagueTableSorter.Sort(_leagueTable, leagueDetailModel);
            var actualSortOrder = sortedLeagueTable.Rows.Select(r => r.Team).ToList();

            Assert.That(actualSortOrder, Is.EqualTo(_sortedByPointsThenGoalsForThenGoalDifferenceThenTeamName));
        }
                        
        [Test]
        public void Sort_OrdersThirdTierLeagueTable_ByPointsThenGoalsForThenGoalDifferenceThenTeamName_Between1992And1998([Range(1992, 1998, 1)] int seasonStartYear)
        {
            var season = $"{seasonStartYear} - {seasonStartYear + 1}";
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 5, Competition = "Second Division", Season = season};
            
            var sortedLeagueTable = _leagueTableSorter.Sort(_leagueTable, leagueDetailModel);
            var actualSortOrder = sortedLeagueTable.Rows.Select(r => r.Team).ToList();

            Assert.That(actualSortOrder, Is.EqualTo(_sortedByPointsThenGoalsForThenGoalDifferenceThenTeamName));
        }
                                
        [Test]
        public void Sort_OrdersFourthTierLeagueTable_ByPointsThenGoalsForThenGoalDifferenceThenTeamName_Between1992And1998([Range(1992, 1998, 1)] int seasonStartYear)
        {
            var season = $"{seasonStartYear} - {seasonStartYear + 1}";
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 5, Competition = "Third Division", Season = season};
            
            var sortedLeagueTable = _leagueTableSorter.Sort(_leagueTable, leagueDetailModel);
            var actualSortOrder = sortedLeagueTable.Rows.Select(r => r.Team).ToList();

            Assert.That(actualSortOrder, Is.EqualTo(_sortedByPointsThenGoalsForThenGoalDifferenceThenTeamName));
        }                
        [Test]
        public void Sort_OrdersSecondTierLeagueTable_ByPointsThenGoalDifferenceThenGoalsForThenTeamName_Between1999And2017([Range(1999, 2017, 1)] int seasonStartYear)
        {
            var season = $"{seasonStartYear} - {seasonStartYear + 1}";
            var competition = seasonStartYear < 2004 ? "First Division" : "Championship";
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 5, Competition = competition, Season = season};
            
            var sortedLeagueTable = _leagueTableSorter.Sort(_leagueTable, leagueDetailModel);
            var actualSortOrder = sortedLeagueTable.Rows.Select(r => r.Team).ToList();

            Assert.That(actualSortOrder, Is.EqualTo(_sortedByPointsThenGoalDifferenceThenGoalsForThenTeamName));
        }
                        
        [Test]
        public void Sort_OrdersThirdTierLeagueTable_ByPointsThenGoalDifferenceThenGoalsForThenTeamName_Between1992And2017([Range(1999, 2017, 1)] int seasonStartYear)
        {
            var season = $"{seasonStartYear} - {seasonStartYear + 1}";
            var competition = seasonStartYear < 2004 ? "Second Division" : "League One";
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 5, Competition = competition, Season = season};

            var sortedLeagueTable = _leagueTableSorter.Sort(_leagueTable, leagueDetailModel);
            var actualSortOrder = sortedLeagueTable.Rows.Select(r => r.Team).ToList();

            Assert.That(actualSortOrder, Is.EqualTo(_sortedByPointsThenGoalDifferenceThenGoalsForThenTeamName));
        }
                                
        [Test]
        public void Sort_OrdersFourthTierLeagueTable_ByPointsThenGoalDifferenceThenGoalsForThenTeamName_Between1992And2017([Range(1999, 2017, 1)] int seasonStartYear)
        {
            var season = $"{seasonStartYear} - {seasonStartYear + 1}";
            var competition = seasonStartYear < 2004 ? "Third Division" : "League Two";
            var leagueDetailModel = new LeagueDetailModel { TotalPlaces = 5, Competition = competition, Season = season};
            
            var sortedLeagueTable = _leagueTableSorter.Sort(_leagueTable, leagueDetailModel);
            var actualSortOrder = sortedLeagueTable.Rows.Select(r => r.Team).ToList();

            Assert.That(actualSortOrder, Is.EqualTo(_sortedByPointsThenGoalDifferenceThenGoalsForThenTeamName));
        }
    }
}