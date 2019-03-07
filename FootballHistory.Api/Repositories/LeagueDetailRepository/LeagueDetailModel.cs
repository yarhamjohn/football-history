namespace FootballHistory.Api.Repositories.LeagueDetailRepository
{
    public class LeagueDetailModel
    {
        public string Competition { get; set; }
        public int TotalPlaces { get; set; }
        public int PromotionPlaces { get; set; }
        public int PlayOffPlaces { get; set; }
        public int RelegationPlaces { get; set; }
        public string Season { get; set; }
    }
}
