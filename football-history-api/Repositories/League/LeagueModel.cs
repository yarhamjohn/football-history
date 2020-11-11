namespace football.history.api.Repositories.League
{
    public class LeagueModel
    {
        public string Name { get; set; }
        public int Tier { get; set; }
        public int TotalPlaces { get; set; }
        public int PromotionPlaces { get; set; }
        public int PlayOffPlaces { get; set; }
        public int RelegationPlaces { get; set; }
        public int RelegationPlayOffPlaces { get; set; }
        public int ReElectionPlaces { get; set; }
        public int? FailedReElectionPosition { get; set; }
        public int PointsForWin { get; set; }
        public int StartYear { get; set; }
    }
}
