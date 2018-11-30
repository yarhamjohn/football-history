using System.Collections.Generic;

public class FilterOptions
{
    public List<string> AllSeasons { get; set; }
    public List<Tier> AllTiers { get; set; }
}

public class Tier
{
    public List<Division> Divisions { get; set; }
    public int Level { get; set; }
}

public class Division
{
    public string Name { get; set; }
    public int SeasonStartYear { get; set; }
    public int SeasonEndYear { get; set; }
}