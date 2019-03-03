using System.Collections.Generic;
using System.Linq;

namespace FootballHistory.Api.Builders.Models
{
    public class LeagueTab
    {
        public List<LeagueTableRow> Rows { get; set; }

        public LeagueTab()
        {
            Rows = new List<LeagueTableRow>();
        }

        public LeagueTab AddPositions()
        {
            var sortedRows = SortTableRows();
            return new LeagueTab
            {
                Rows = sortedRows.Select((t, i) =>
                {
                    t.Position = i + 1;
                    return t;
                }).ToList()
            };

        }
        
        private List<LeagueTableRow> SortTableRows()
        {
            return Rows
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalDifference) // Goal ratio was used prior to 1976-77
                .ThenByDescending(t => t.GoalsFor)
                // head to head
                .ThenBy(t => t.Team) // unless it affects a promotion/relegation spot at the end of the season in which case a play-off occurs (this has never happened)
                .ToList(); 
        }
    }
}