namespace football.history.api.Builders
{
    /// <summary>
    /// DTO for the Season object returned by the API
    /// </summary>
    public record SeasonDto (long Id, int StartYear, int EndYear);
}
