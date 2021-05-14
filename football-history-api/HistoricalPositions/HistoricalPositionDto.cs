using football.history.api.Dtos;

namespace football.history.api.Builders
{
    public record HistoricalPositionTeamDto
    (
        long CompetitionId,
        int Position,
        int AbsolutePosition,
        string? Status
    );
    
    public record HistoricalPositionDto
    (
        long SeasonId,
        int SeasonStartYear,
        CompetitionDto[] Competitions,
        HistoricalPositionTeamDto? Team
    );
}