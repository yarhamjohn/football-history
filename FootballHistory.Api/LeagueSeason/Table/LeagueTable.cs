using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballHistory.Api.LeagueSeason.Table
{
    public class LeagueTable : ILeagueTable
    {
        public List<LeagueTableRow> Rows { get; set; }

        public LeagueTable()
        {
            Rows = new List<LeagueTableRow>();
        }
        
        public int GetPosition(string team)
        {
            var positions = Rows.Where(r => r.Team == team).Select(r => r.Position).ToList();
            if (positions.Count == 0)
            {
                throw new Exception($"The requested team ({team}) was not found in the league table.");
            }

            return positions.Single();
        }
    }
}