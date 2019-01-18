using System.Collections.Generic;

public class PlayOffs
{
    public List<SemiFinal> SemiFinals { get; set; }
    public MatchDetail Final { get; set; }
}

public class SemiFinal
{
    public MatchDetail FirstLeg { get; set; }
    public MatchDetail SecondLeg { get; set; }
}
