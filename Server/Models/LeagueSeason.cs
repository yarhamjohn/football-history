using System;
using System.Collections.Generic;

public class LeagueSeason
{
    public string Season { get; set; }
    public int Tier { get; set; }
    public string CompetitionName { get; set; }

    public LeagueTable LeagueTable { get; set; }

    public PlayOffs PlayOffs { get; set; }
    public ResultMatrix ResultMatrix { get; set; }
    public LeagueFilterOptions LeagueFilterOptions { get; set; }
}
