namespace Backend.Models.LeagueSeason
{
    public class PointDeduction
    {
        public string Competition { get; set; }
        public string Team { get; set; }
        public int PointsDeducted { get; set; }
        public string Reason { get; set; }
    }
}
