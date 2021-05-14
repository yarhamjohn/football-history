using System;

namespace football.history.api.Builders.Match
{
    public record MatchDto (
        long Id,
        DateTime MatchDate,
        MatchCompetitionDto Competition,
        MatchRulesDto Rules,
        MatchTeamDto HomeTeam,
        MatchTeamDto AwayTeam);

    public record MatchRulesDto(
        string Type,
        string? Stage,
        bool ExtraTime,
        bool Penalties,
        int? NumLegs,
        bool AwayGoals,
        bool Replays);

    public record MatchTeamDto(
        long Id,
        string Name,
        string Abbreviation,
        int Goals,
        int GoalsExtraTime,
        int PenaltiesTaken,
        int PenaltiesScored);
        
    public record MatchCompetitionDto(
        long Id, 
        string Name,
        int StartYear,
        int EndYear,
        string Level);
}