using System;
using System.Collections.Generic;

namespace Backend.Models.LeagueSeason
{
    public class ResultMatrix
    {
        public List<ResultMatrixRow> Rows { get; }

        public ResultMatrix()
        {
            Rows = new List<ResultMatrixRow>();
        }
    }

    public class ResultMatrixRow
    {
        public string HomeTeam { get; set; }
        public string HomeTeamAbbreviation { get; set; }
        public List<MatchResult> Results { get; set; }

        public ResultMatrixRow()
        {
            Results = new List<MatchResult>();
        }
    }

    public class MatchResult
    {
        public string AwayTeam { get; set; }
        public string AwayTeamAbbreviation { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public DateTime? MatchDate { get; set; }
    }
}
