using System;
using System.Collections.Generic;

public class LeagueTable
{
    public List<LeagueTableRow> Rows { get; set; }
}

public class LeagueTableRow
{
    public int Position { get; set; }
    public string Team { get; set; }
    public int Played { get; set; }
    public int Won { get; set; }
    public int Drawn { get; set; }
    public int Lost { get; set; }
    public int GoalsFor { get; set; }
    public int GoalsAgainst { get; set; }
    public int GoalDifference { get; set; }
    public int Points { get; set; }
    public int PointsDeducted { get; set; }
    public string PointsDeductionReason { get; set; }
    public string Status { get; set; }
    public List<MatchResult> Form { get; set; } 
}

public class MatchResult
{
    public DateTime MatchDate { get; set; }
    public string Result { get; set; } 
}
