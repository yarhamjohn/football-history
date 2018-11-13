using System.Collections.Generic;

public class Results
{
    public string HomeTeam { get; set; }
    public string HomeTeamAbbreviation { get; set; }
    public List<(string AwayTeam, string Score, string Result)> Scores { get; set; }
}