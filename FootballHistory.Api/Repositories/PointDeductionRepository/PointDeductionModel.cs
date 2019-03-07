namespace FootballHistory.Api.Repositories.PointDeductionRepository
{
    public class PointDeductionModel
    {
        public string Competition { get; set; }
        public string Team { get; set; }
        public int PointsDeducted { get; set; }
        public string Reason { get; set; }
        public string Season { get; set; }
    }
}
