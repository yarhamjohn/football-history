using System.Collections.Generic;

public class ResultMatrix
{
    public List<ResultMatrixRow> Rows { get; set; }
}

public class ResultMatrixRow
{
    public string HomeTeam { get; set; }
    public string HomeTeamAbbreviation { get; set; }
    public List<ResultScore> Scores { get; set; }
}

public class ResultScore
{
    public string AwayTeam { get; set; }
    public string Score { get; set; }
    public string Result { get; set; }
}