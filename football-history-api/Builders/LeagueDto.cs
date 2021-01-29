using System.Collections.Generic;

namespace football.history.api.Builders
{
    public class LeagueDto
    {
        public string Name { get; set; }
        public int Tier { get; set; }
        public int TotalPlaces { get; set; }
        public int PromotionPlaces { get; set; }
        public int PlayOffPlaces { get; set; }
        public int RelegationPlaces { get; set; }
        public int PointsForWin { get; set; }
        public int StartYear { get; set; }
        public List<LeagueTableRowDto> Table { get; set; }
    }
}
