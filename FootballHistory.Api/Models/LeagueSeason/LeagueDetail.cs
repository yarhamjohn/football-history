namespace FootballHistory.Api.Models.LeagueSeason
{
    public class LeagueDetail
    {
        public string Competition { get; set; }
        public int TotalPlaces { get; set; }
        public int PromotionPlaces { get; set; }
        public int PlayOffPlaces { get; set; }
        public int RelegationPlaces { get; set; }
    }
}
