using System.Collections.Generic;
using System.Linq;
using football.history.api.Repositories.Competition;

namespace football.history.api.Builders
{
    public interface IHistoricalPosition
    {
        public HistoricalPositionDto ToDto();
    }

    public class EmptyHistoricalPosition : IHistoricalPosition
    {
        private readonly CompetitionModel[] _competitionsInSeason;
        private readonly CompetitionModel _competition;

        public EmptyHistoricalPosition(
            CompetitionModel[] competitionsInSeason,
            CompetitionModel competition)
        {
            _competitionsInSeason = competitionsInSeason;
            _competition          = competition;
        }

        public HistoricalPositionDto ToDto()
            => new(
                SeasonId: _competition.SeasonId,
                SeasonStartYear: _competition.StartYear,
                Competitions: _competitionsInSeason.Select(CompetitionModelExtensions.ToCompetitionDto).ToArray(),
                Team: null);
    }
    
    
    public class PopulatedHistoricalPosition : IHistoricalPosition
    {
        private readonly CompetitionModel[] _competitionsInSeason;
        private readonly CompetitionModel _competition;
        private readonly LeagueTableRowDto _teamRow;

        public PopulatedHistoricalPosition(
            CompetitionModel[] competitionsInSeason,
            CompetitionModel competition,
            LeagueTableRowDto teamRow)
        {
            _competitionsInSeason = competitionsInSeason;
            _competition          = competition;
            _teamRow              = teamRow;
        }

        public HistoricalPositionDto ToDto()
            => new(
                SeasonId: _competition.SeasonId,
                SeasonStartYear: _competition.StartYear,
                Competitions: _competitionsInSeason.Select(CompetitionModelExtensions.ToCompetitionDto).ToArray(),
                Team: GetTeamHistoricalPositionDto());

        private HistoricalPositionTeamDto GetTeamHistoricalPositionDto()
        {
            return new(
                CompetitionId: _competition.Id,
                Position: _teamRow.Position,
                AbsolutePosition: GetAbsolutePosition(_competitionsInSeason, _competition, _teamRow),
                Status: _teamRow.Status);
        }

        private static int GetAbsolutePosition(
            IEnumerable<CompetitionModel> competitions,
            CompetitionModel competition,
            LeagueTableRowDto teamRowDto)
            => competitions
                   .Where(m => m.Tier < competition.Tier)
                   .Select(m => m.TotalPlaces)
                   .Sum()
               + teamRowDto.Position;
    }
}