using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Repositories.Competition;
using football.history.api.Repositories.Match;
using football.history.api.Repositories.PointDeduction;

namespace football.history.api.Builders
{
    public interface ILeaguePositionBuilder
    {
        List<LeaguePositionDto> GetPositions(long teamId, CompetitionModel competition);
    }

    public class LeaguePositionBuilder : ILeaguePositionBuilder
    {
        private readonly ILeagueTableBuilder _leagueTableBuilder;
        private readonly IMatchRepository _matchRepository;
        private readonly IPointDeductionRepository _pointDeductionRepository;

        public LeaguePositionBuilder(
            ILeagueTableBuilder leagueTableBuilder,
            IMatchRepository matchRepository,
            IPointDeductionRepository pointDeductionRepository)
        {
            _leagueTableBuilder       = leagueTableBuilder;
            _matchRepository          = matchRepository;
            _pointDeductionRepository = pointDeductionRepository;
        }

        public List<LeaguePositionDto> GetPositions(long teamId, CompetitionModel competition)
        {
            var leagueMatches = _matchRepository.GetLeagueMatches(competition.Id);
            if (!leagueMatches.Any())
            {
                return new List<LeaguePositionDto>();
            }

            var pointDeductions = _pointDeductionRepository.GetPointDeductions(competition.Id);

            return GetDates(leagueMatches)
                .Select(d => GetLeaguePositionDto(teamId, competition, leagueMatches, d, pointDeductions))
                .ToList();
        }

        private LeaguePositionDto GetLeaguePositionDto(
            long teamId, CompetitionModel competition,
            List<MatchModel> leagueMatches,
            DateTime targetDate,
            List<PointDeductionModel> pointDeductions)
        {
            var partialLeagueTable =
                _leagueTableBuilder.BuildPartialLeagueTable(competition, leagueMatches, targetDate, pointDeductions);
            var position = partialLeagueTable.GetPosition(teamId);
            return new(Date: targetDate, Position: position);
        }

        private static IEnumerable<DateTime> GetDates(IReadOnlyCollection<MatchModel> leagueMatches)
        {
            var startDate = leagueMatches.Min(x => x.MatchDate).AddDays(-1);
            var endDate = leagueMatches.Max(x => x.MatchDate).AddDays(1);

            return Enumerable
                .Range(0, 1 + endDate.Subtract(startDate).Days)
                .Select(offset => startDate.AddDays(offset)).ToList();
        }
    }
}