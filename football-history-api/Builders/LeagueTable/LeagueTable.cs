using System.Collections.Generic;
using System.Linq;
using football.history.api.Dtos;
using football.history.api.Exceptions;
using football.history.api.Repositories.Competition;

namespace football.history.api.Builders
{
    public interface ILeagueTable
    {
        List<LeagueTableRowDto> GetRows();
        LeagueTableRowDto GetRow(long teamId);
        int GetPosition(long teamId);
    }
    
    public class LeagueTable : ILeagueTable
    {
        private readonly List<LeagueTableRowDto> _rows;

        public LeagueTable(List<LeagueTableRowDto> rows)
        {
            _rows = rows;
        }
        
        public List<LeagueTableRowDto> GetRows() => _rows;

        public LeagueTableRowDto GetRow(long teamId)
        {
            try
            {
                return _rows.Single(r => r.TeamId == teamId);
            }
            catch
            {
                throw new DataInvalidException($"The requested team ({teamId}) appeared {_rows.Count} times in the league table. Expected it to appear once.");
            }
        }
        
        public int GetPosition(long teamId)
        {
            return GetRow(teamId).Position;
        }
    }
}