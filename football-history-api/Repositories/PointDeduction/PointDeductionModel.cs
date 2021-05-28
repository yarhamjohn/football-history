namespace football.history.api.Repositories.PointDeduction
{
    public record PointDeductionModel (
        long Id,
        long CompetitionId,
        int PointsDeducted,
        long TeamId,
        string TeamName,
        string Reason);
}
