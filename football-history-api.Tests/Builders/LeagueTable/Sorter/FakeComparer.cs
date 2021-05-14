using System;
using System.Collections.Generic;
using football.history.api.Builders;

namespace football.history.api.Tests.Builders.LeagueTable.Sorter
{
    public class FakeComparer : IComparer<LeagueTableRowDto?>
    {
        public int Compare(LeagueTableRowDto? x, LeagueTableRowDto? y)
        {
            if (x is null || y is null)
            {
                throw new NotImplementedException();
            }
            
            var points = x.Points.CompareTo(y.Points);
            if (points != 0)
            {
                return points;
            }

            return string.Compare(y.Team, x.Team, StringComparison.Ordinal);
        }
    }
}