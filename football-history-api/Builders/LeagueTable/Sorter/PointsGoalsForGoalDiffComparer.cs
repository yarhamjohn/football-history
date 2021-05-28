using System;
using System.Collections.Generic;

namespace football.history.api.Builders
{
    public class PointsGoalsForGoalDiffComparer : IComparer<LeagueTableRowDto>
    {
        private const int Equal = 0;
        private const int XIsGreater = 1;
        private const int YIsGreater = -1;
        public int Compare(LeagueTableRowDto? x, LeagueTableRowDto? y)
        {
            if (ReferenceEquals(x, y))
            {
                return Equal;
            }

            if (x == null)
            {
                return YIsGreater;
            }

            if (y == null)
            {
                return XIsGreater;
            }
            
            var points = x.Points.CompareTo(y.Points);
            if (points != Equal)
            {
                return points;
            }
            
            var goalsFor = x.GoalsFor.CompareTo(y.GoalsFor);
            if (goalsFor != Equal)
            {
                return goalsFor;
            }
            
            var goalDifference = x.GoalDifference.CompareTo(y.GoalDifference);
            if (goalDifference != Equal)
            {
                return goalDifference;
            }
            
            // TODO: Sort by head-to-head results
            
            var teamName = string.Compare(x.Team, y.Team, StringComparison.Ordinal);
            if (teamName != Equal)
            {
                // unless it affects a promotion/relegation spot at the end of the season
                // in which case a play-off occurs (this has never happened)
                return teamName > 0 ? YIsGreater : XIsGreater;
            }

            return Equal;
        }
    }
}