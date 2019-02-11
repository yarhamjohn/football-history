namespace FootballHistory.Api.Repositories.Models
{
    public class PointDeductionModel
    {
        public string Competition { get; set; }
        public string Team { get; set; }
        public int PointsDeducted { get; set; }
        public string Reason { get; set; }
    }
}
