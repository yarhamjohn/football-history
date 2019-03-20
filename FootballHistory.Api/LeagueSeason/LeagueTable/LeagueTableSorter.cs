using System;
using System.Linq;
using FootballHistory.Api.Repositories.LeagueDetailRepository;

namespace FootballHistory.Api.LeagueSeason.LeagueTable
{
    public class LeagueTableSorter : ILeagueTableSorter
    {
        public LeagueTable Sort(LeagueTable leagueTable, LeagueDetailModel leagueDetailModel)
        {
            var sortedLeagueTable = new LeagueTable();
            
            var seasonStartYear = Convert.ToInt32(leagueDetailModel.Season.Substring(0, 4));
            if (seasonStartYear >= 1999 || leagueDetailModel.Competition == "Premier League")
            {
                sortedLeagueTable.Rows = leagueTable.Rows
                    .OrderByDescending(t => t.Points)
                    .ThenByDescending(t => t.GoalDifference) // Goal ratio was used prior to 1976-77
                    .ThenByDescending(t => t.GoalsFor)
                    // head to head
                    .ThenBy(t => t.Team) // unless it affects a promotion/relegation spot at the end of the season in which case a play-off occurs (this has never happened)
                    .ToList();
            }
            else
            {
                sortedLeagueTable.Rows = leagueTable.Rows
                    .OrderByDescending(t => t.Points)
                    .ThenByDescending(t => t.GoalsFor)
                    .ThenByDescending(t => t.GoalDifference) // Goal ratio was used prior to 1976-77
                    // head to head
                    .ThenBy(t =>
                        t.Team) // unless it affects a promotion/relegation spot at the end of the season in which case a play-off occurs (this has never happened)
                    .ToList();
            }

            return sortedLeagueTable;
        }
    }
}