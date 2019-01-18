using System;
using System.Collections.Generic;

public class LeagueRowDrillDown
{
    public List<MatchResult> Form { get; set; }
    public List<LeaguePosition> Positions {get; set; }
}

public class MatchResult
{
    public DateTime MatchDate { get; set; }
    public string Result { get; set; } 
}

public class LeaguePosition
{
    public DateTime Date { get; set; }
    public int Position { get; set; }
}
