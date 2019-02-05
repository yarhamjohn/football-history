using System;

namespace FootballHistory.Api.Models.LeagueSeason
{
    public class ResultMatrixMatch
    {
        public string AwayTeam { get; set; }
        public string AwayTeamAbbreviation { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public DateTime? MatchDate { get; set; }
    }
}
