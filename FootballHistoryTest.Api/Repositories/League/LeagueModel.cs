using System.Collections.Generic;
using FootballHistoryTest.Api.Repositories.Team;

namespace FootballHistoryTest.Api.Repositories.League
{
    public class LeagueModel
    {
        public string Name { get; set; }
        public int Tier { get; set; }
        public int TotalPlaces { get; set; }
        public int PromotionPlaces { get; set; }
        public int PlayOffPlaces { get; set; }
        public int RelegationPlaces { get; set; }
        public int PointsForWin { get; set; }
    }
}