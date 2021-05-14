namespace football.history.api.Repositories.Season
{
    /// <summary>
    /// Database model representation for the Season query output
    /// </summary>
    public record SeasonModel (long Id, int StartYear, int EndYear);
}
