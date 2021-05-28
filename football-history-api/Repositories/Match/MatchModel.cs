using System;

namespace football.history.api.Repositories.Match
{
    public record MatchModel (
        long Id,
        DateTime MatchDate,
        long CompetitionId,
        string CompetitionName,
        int CompetitionStartYear,
        int CompetitionEndYear,
        int CompetitionTier,
        string? CompetitionRegion,
        string RulesType,
        string? RulesStage,
        bool RulesExtraTime,
        bool RulesPenalties,
        int? RulesNumLegs,
        bool RulesAwayGoals,
        bool RulesReplays,
        long HomeTeamId,
        string HomeTeamName,
        string HomeTeamAbbreviation,
        long AwayTeamId,
        string AwayTeamName,
        string AwayTeamAbbreviation,
        int HomeGoals,
        int AwayGoals,
        int HomeGoalsExtraTime,
        int AwayGoalsExtraTime,
        int HomePenaltiesTaken,
        int HomePenaltiesScored,
        int AwayPenaltiesTaken,
        int AwayPenaltiesScored)
    {
        public readonly string CompetitionLevel = $@"{CompetitionTier}{CompetitionRegion}";
    };
}
