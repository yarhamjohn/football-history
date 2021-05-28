using System.Linq;
using football.history.api.Exceptions;
using football.history.api.Repositories.Competition;

namespace football.history.api.Builders
{
    public interface IHistoricalPositionBuilder
    {
        IHistoricalPosition Build(long teamId, long seasonId);
    }

    public class HistoricalPositionBuilder : IHistoricalPositionBuilder
    {
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ILeagueTableBuilder _leagueTableBuilder;

        public HistoricalPositionBuilder(
            ICompetitionRepository competitionRepository,
            ILeagueTableBuilder leagueTableBuilder)
        {
            _competitionRepository   = competitionRepository;
            _leagueTableBuilder      = leagueTableBuilder;
        }
        
        public IHistoricalPosition Build(long teamId, long seasonId)
        {
            var competitionsInSeason = _competitionRepository.GetCompetitionsInSeason(seasonId).ToArray();
            if (!competitionsInSeason.Any())
            {
                throw new DataNotFoundException($"No competitions were found for the requested season ({seasonId})");
            }

            var competition = _competitionRepository.GetCompetitionForSeasonAndTeam(seasonId, teamId);
            if (competition is null)
            {
                // The requested team was not in a competition in this season so just use the first for common data
                var comp = competitionsInSeason.First();
                return new EmptyHistoricalPosition(competitionsInSeason, comp);
            }

            var row = _leagueTableBuilder.BuildFullLeagueTable(competition).GetRow(teamId);
            return new PopulatedHistoricalPosition(competitionsInSeason, competition, row);
        }
    }
}