using System.Collections.Generic;

namespace FootballHistory.Api.Builders.Models
{
    public class LeagueTab
    {
        public List<LeagueTableRow> Rows { get; set; }

        public LeagueTab()
        {
            Rows = new List<LeagueTableRow>();
        }
    }
}
