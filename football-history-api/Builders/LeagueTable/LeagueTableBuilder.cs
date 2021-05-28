using System;
using System.Collections.Generic;
using System.Linq;
using football.history.api.Repositories.Competition;
using football.history.api.Repositories.Match;
using football.history.api.Repositories.PointDeduction;
using football.history.api.Repositories.Team;

namespace football.history.api.Builders
{
    public interface ILeagueTableBuilder
    {
        ILeagueTable BuildFullLeagueTable(CompetitionModel competition);
        ILeagueTable BuildPartialLeagueTable(CompetitionModel competition, List<MatchModel> leagueMatches, DateTime targetDate, List<PointDeductionModel> pointDeductions);
    }
    
    public class LeagueTableBuilder : ILeagueTableBuilder
    {
        private readonly IRowComparerFactory _rowComparerFactory;
        private readonly IStatusCalculator _statusCalculator;
        private readonly IMatchRepository _matchRepository;
        private readonly IPointDeductionRepository _pointDeductionRepository;
        private readonly IRowBuilder _rowBuilder;

        public LeagueTableBuilder(
            IMatchRepository matchRepository,
            IPointDeductionRepository pointDeductionRepository,
            IRowBuilder rowBuilder,
            IRowComparerFactory rowComparerFactory,
            IStatusCalculator statusCalculator)
        {
            _matchRepository = matchRepository;
            _pointDeductionRepository = pointDeductionRepository;
            _rowBuilder = rowBuilder;
            _rowComparerFactory = rowComparerFactory;
            _statusCalculator   = statusCalculator;
        }
        
        public ILeagueTable BuildFullLeagueTable(CompetitionModel competition)
        {
            var leagueMatches = _matchRepository.GetLeagueMatches(competition.Id);
            var teamsInLeague = GetTeamsInLeague(leagueMatches);
            var pointDeductions = _pointDeductionRepository.GetPointDeductions(competition.Id);

            var rows = GetRows(competition, teamsInLeague, leagueMatches, pointDeductions);
            
            SetPositions(competition, rows);
            SetStatuses(competition, rows);
            
            return new LeagueTable(rows);
        }
        
        public ILeagueTable BuildPartialLeagueTable(CompetitionModel competition, List<MatchModel> leagueMatches, DateTime targetDate, List<PointDeductionModel> pointDeductions)
        {
            var matchesToDate = leagueMatches.Where(x => x.MatchDate < targetDate).ToList();
            var teamsInLeague = GetTeamsInLeague(leagueMatches);

            var rows = GetRows(competition, teamsInLeague, matchesToDate, pointDeductions);

            SetPositions(competition, rows);
            
            return new LeagueTable(rows);
        }

        private static IEnumerable<TeamModel> GetTeamsInLeague(List<MatchModel> leagueMatches)
        {
            return leagueMatches.SelectMany(
                    m => new[]
                    {
                        new TeamModel(m.HomeTeamId, m.HomeTeamName, m.HomeTeamAbbreviation, Notes: null),
                        new TeamModel(m.AwayTeamId, m.AwayTeamName, m.AwayTeamAbbreviation, Notes: null)
                    })
                .Distinct();
        }
        
        private List<LeagueTableRowDto> GetRows(
            CompetitionModel competition,
            IEnumerable<TeamModel> teamsInLeague,
            List<MatchModel> leagueMatches,
            List<PointDeductionModel> pointDeductions)
        {
            return teamsInLeague
                .Select(team => _rowBuilder.Build(competition, team, leagueMatches, pointDeductions))
                .ToList();
        }

        private void SetPositions(CompetitionModel competition, List<LeagueTableRowDto> rows)
        {
            var leagueTableComparer = _rowComparerFactory.GetLeagueTableComparer(competition);
            rows.Sort(leagueTableComparer);

            for (var i = 0; i < rows.Count; i++)
            {
                rows[i].Position = rows.Count - i;
            }
        }

        private void SetStatuses(CompetitionModel competition, IEnumerable<LeagueTableRowDto> rows)
        {
            foreach (var row in rows)
            {
                row.Status = _statusCalculator.GetStatus(row.Team, row.Position, competition);
            }
        }
    }
}