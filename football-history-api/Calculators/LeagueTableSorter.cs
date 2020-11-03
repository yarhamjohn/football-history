using System.Collections.Generic;
using System.Linq;
using football.history.api.Builders;
using football.history.api.Repositories.League;

namespace football.history.api.Calculators
{
    public static class LeagueTableSorter
    {
        public static List<LeagueTableRow> SortTable(
            List<LeagueTableRow> leagueTable,
            LeagueModel leagueModel)
        {
            List<LeagueTableRow> sortedLeagueTable;
            if (FootballLeagueBetween1992And1998(leagueModel))
            {
                sortedLeagueTable = leagueTable.OrderByDescending(t => t.Points)
                    .ThenByDescending(t => t.GoalsFor)
                    .ThenByDescending(t => t.GoalDifference)
                    // head to head
                    .ThenBy(t => t.Team)
                    .ToList();
            } else
            {
                sortedLeagueTable = leagueTable
                    .OrderByDescending(
                        t => IsCovidAffectedLeague(leagueModel) ? t.PointsPerGame : t.Points)
                    .ThenByDescending(t => t.GoalDifference)
                    .ThenByDescending(t => t.GoalsFor)
                    // head to head
                    .ThenBy(t => t.Team) // unless it affects a promotion/relegation spot at the end of the season in which case a play-off occurs (this has never happened)
                    .ToList();
            }

            for (var i = 0; i < sortedLeagueTable.Count; i++)
            {
                sortedLeagueTable[i].Position = i + 1;
            }

            return sortedLeagueTable;
        }

        private static bool IsCovidAffectedLeague(LeagueModel leagueModel) =>
            leagueModel.StartYear == 2019 && (leagueModel.Tier == 3 || leagueModel.Tier == 4);

        private static bool FootballLeagueBetween1992And1998(LeagueModel leagueModel) =>
            leagueModel.StartYear >= 1992 && leagueModel.StartYear <= 1998 && leagueModel.Name != "Premier League";
    }
}
