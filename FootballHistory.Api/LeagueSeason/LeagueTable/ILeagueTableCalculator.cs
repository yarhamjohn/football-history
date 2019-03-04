namespace FootballHistory.Api.LeagueSeason.LeagueTable
{
    public interface ILeagueTableCalculator
    {
        int CountGamesPlayed();
        int CalculateGoalDifference();
        int CountGoalsFor();
        int CountGoalsAgainst();
        int CountWins();
        int CountDraws();
        int CountDefeats();
        int CalculatePoints();
        int CalculatePointsDeducted();
        string GetPointDeductionReasons();
    }
}