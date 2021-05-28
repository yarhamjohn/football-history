using football.history.api.Dtos;

namespace football.history.api.Repositories.Competition
{
    /// <summary>
    /// Database model representation for the Competition query output
    /// </summary>
    public record CompetitionModel (
        long Id,
        string Name,
        long SeasonId,
        int StartYear,
        int EndYear,
        int Tier,
        string? Region,
        string? Comment,
        int PointsForWin,
        int TotalPlaces,
        int PromotionPlaces,
        int RelegationPlaces,
        int PlayOffPlaces,
        int RelegationPlayOffPlaces,
        int ReElectionPlaces,
        int? FailedReElectionPosition)
    {
        public readonly string Level = $@"{Tier}{Region}";
    }

    public static class CompetitionModelExtensions
    {
        public static CompetitionDto ToCompetitionDto(this CompetitionModel model)
            =>
                new(model.Id,
                    model.Name,
                    Season: new(
                        model.SeasonId,
                        model.StartYear,
                        model.EndYear),
                    model.Level,
                    model.Comment,
                    Rules: new(
                        model.PointsForWin,
                        model.TotalPlaces,
                        model.PromotionPlaces,
                        model.RelegationPlaces,
                        model.PlayOffPlaces,
                        model.RelegationPlayOffPlaces,
                        model.ReElectionPlaces,
                        model.FailedReElectionPosition));
    }
}
