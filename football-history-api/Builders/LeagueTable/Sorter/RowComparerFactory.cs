using System.Collections.Generic;
using football.history.api.Repositories.Competition;

namespace football.history.api.Builders
{
    public interface IRowComparerFactory
    {
        IComparer<LeagueTableRowDto> GetLeagueTableComparer(CompetitionModel competition);
    }
    
    public class RowComparerFactory : IRowComparerFactory
    {
        public IComparer<LeagueTableRowDto> GetLeagueTableComparer(CompetitionModel competition)
        {
            if (FootballLeagueBetween1992And1998(competition))
            {
                return new PointsGoalsForGoalDiffComparer();
            }

            if (FootballLeagueBefore1976(competition))
            {
                return new PointsGoalAvgComparer();
            }

            if (IsCovidAffectedLeague(competition))
            {
                return new PointsPerGameGoalDiffGoalsForComparer();
            }

            return new PointsGoalDiffGoalsForComparer();
        }

        private static bool IsCovidAffectedLeague(CompetitionModel competition) =>
            competition.StartYear == 2019 && competition.Tier is 3 or 4;

        private static bool FootballLeagueBetween1992And1998(CompetitionModel competition) =>
            competition.StartYear is >= 1992 and <= 1998 && competition.Tier != 1;

        private static bool FootballLeagueBefore1976(CompetitionModel competition) =>
            competition.StartYear < 1976;
    }
}