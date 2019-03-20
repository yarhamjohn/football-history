using FootballHistory.Api.Repositories.LeagueDetailRepository;

namespace FootballHistory.Api.LeagueSeason.LeagueTable
{
    public class LeagueTablePositionCalculator : ILeagueTablePositionCalculator
    {
        private readonly ILeagueTableSorter _leagueTableSorter;

        public LeagueTablePositionCalculator(ILeagueTableSorter leagueTableSorter)
        {
            _leagueTableSorter = leagueTableSorter;
        }
        
        public LeagueTable AddPositions(LeagueTable leagueTable, LeagueDetailModel leagueDetailModel)
        {
            var sortedLeagueTable = _leagueTableSorter.Sort(leagueTable, leagueDetailModel);

            for (var i = 0; i < sortedLeagueTable.Rows.Count; i++)
            {
                sortedLeagueTable.Rows[i].Position = i + 1;
            }

            return sortedLeagueTable;
        }
    }
}