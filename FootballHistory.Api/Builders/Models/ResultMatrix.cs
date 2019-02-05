using System.Collections.Generic;

namespace FootballHistory.Api.Models.LeagueSeason
{
    public class ResultMatrix
    {
        public List<ResultMatrixRow> Rows { get; }

        public ResultMatrix()
        {
            Rows = new List<ResultMatrixRow>();
        }
    }
}
