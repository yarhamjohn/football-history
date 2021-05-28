namespace football.history.api.Dtos
{
    /// <summary>
    /// DTO for the Competition object returned by the API
    /// </summary>
    public record CompetitionDto (
        long Id,
        string Name,
        CompetitionSeasonDto Season,
        string Level,
        string? Comment,
        CompetitionRulesDto Rules);

    public record CompetitionRulesDto(
        int PointsForWin,
        int TotalPlaces,
        int PromotionPlaces,
        int RelegationPlaces,
        int PlayOffPlaces,
        int RelegationPlayOffPlaces,
        int ReElectionPlaces,
        int? FailedReElectionPosition);

    public record CompetitionSeasonDto
    (
        long Id,
        int StartYear,
        int EndYear 
    );
}
