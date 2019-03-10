namespace FootballHistory.Api.Repositories.TierRepository
{
    public class SeasonTierFilter
    {
        public int Tier { get; set; }
        public int SeasonStartYear { get; set; }
        public int SeasonEndYear => SeasonStartYear + 1;
        public string Season => $"{SeasonStartYear} - {SeasonEndYear}";
    }
}