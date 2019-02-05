using System.Collections.Generic;

namespace FootballHistory.Api.Models.LeagueSeason
{
    public class ResultMatrixRow
    {
        public string HomeTeam { get; set; }
        public string HomeTeamAbbreviation { get; set; }
        public List<ResultMatrixMatch> Results { get; set; }

        public ResultMatrixRow()
        {
            Results = new List<ResultMatrixMatch>();
        }
    }
}
