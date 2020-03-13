namespace FootballHistoryTest.Api.Repositories.PointDeductions
{
    public class PointsDeductionModel
    {
        public string Team { get; set; }
        public int SeasonStartYear { get; set; }
        public int PointsDeducted {get; set; }
        public string Reason { get; set; }
    }
}
