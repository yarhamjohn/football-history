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
            if (PremierLeague_Or_FootballLeagueFrom1999(leagueModel))
            {
                sortedLeagueTable = leagueTable
                    .OrderByDescending(
                        t => IsCovidAffectedLeague(leagueModel) ? t.PointsPerGame : t.Points)
                    .ThenByDescending(t => t.GoalDifference) // Goal ratio was used prior to 1976-77
                    .ThenByDescending(t => t.GoalsFor)
                    // head to head
                    .ThenBy(t => t.Team)
                    .ToList(); // unless it affects a promotion/relegation spot at the end of the season in which case a play-off occurs (this has never happened)
            }
            else
            {
                sortedLeagueTable = leagueTable.OrderByDescending(t => t.Points)
                    .ThenByDescending(t => t.GoalsFor)
                    .ThenByDescending(t => t.GoalDifference) // Goal ratio was used prior to 1976-77
                    // head to head
                    .ThenBy(t => t.Team)
                    .ToList(); // unless it affects a promotion/relegation spot at the end of the season in which case a play-off occurs (this has never happened)
            }

            for (var i = 0; i < sortedLeagueTable.Count; i++)
            {
                sortedLeagueTable[i].Position = i + 1;
            }

            return sortedLeagueTable;
        }

        private static bool IsCovidAffectedLeague(LeagueModel leagueModel) =>
            leagueModel.StartYear == 2019 && (leagueModel.Tier == 3 || leagueModel.Tier == 4);

        private static bool PremierLeague_Or_FootballLeagueFrom1999(LeagueModel leagueModel) =>
            leagueModel.StartYear >= 1999 || leagueModel.Name == "Premier League";
    }
}
