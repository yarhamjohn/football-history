namespace FootballHistory.Api.Builders
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