namespace football.history.api.Builders
{
    public class LeagueTableRowDto
    {
        public int Position { get; set; }
        public string Team { get; set; }
        public int Played { get; set; }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference { get; set; }
        public double GoalAverage { get; set; }
        public int Points { get; set; }
        public double PointsPerGame { get; set; }
        public int PointsDeducted { get; set; }
        public string? PointsDeductionReason { get; set; }
        public string? Status { get; set; }
    }
}
