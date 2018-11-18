using System.Collections.Generic;

public class PlayOff
{
    public List<PlayOffSemiFinal> SemiFinals { get; set; }
    public PlayOffResult Final { get; set; }
}

public class PlayOffSemiFinal
{
    public PlayOffResult FirstLeg { get; set; }
    public PlayOffResult SecondLeg { get; set; }
}

public class PlayOffResult
{
    public string HomeTeam { get; set; }
    public string AwayTeam { get; set; }
    public int HomeGoals { get; set; }
    public int AwayGoals { get; set; }
    public int? HomeGoalsET { get; set; }
    public int? AwayGoalsET { get; set; }
    public int? HomePenaltiesTaken { get; set; }
    public int? HomePenaltiesScored { get; set; }
    public int? AwayPenaltiesTaken { get; set; }
    public int? AwayPenaltiesScored { get; set; }
}
