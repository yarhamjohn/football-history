using System.Collections.Generic;

public class Results
{
    public string HomeTeam { get; set; }
    public List<(string AwayTeam, string Result)> Scores { get; set; }
}