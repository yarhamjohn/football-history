using System;

public class MatchDetail
{
    public string Competition { get; set; }
    public string Round { get; set; }
    public DateTime Date { get; set; }
    public string HomeTeam { get; set; }
    public string HomeTeamAbbreviation { get; set; }
    public string AwayTeam { get; set; }
    public string AwayTeamAbbreviation { get; set; }
    public int HomeGoals { get; set; }
    public int AwayGoals { get; set; }
    public bool ExtraTime { get; set;  }
    public int? HomeGoalsET { get; set; }
    public int? AwayGoalsET { get; set; }
    public bool PenaltyShootout { get; set; }
    public int? HomePenaltiesTaken { get; set; }
    public int? HomePenaltiesScored { get; set; }
    public int? AwayPenaltiesTaken { get; set; }
    public int? AwayPenaltiesScored { get; set; }
}