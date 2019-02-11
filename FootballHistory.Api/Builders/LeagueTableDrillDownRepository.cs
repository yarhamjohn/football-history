using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using FootballHistory.Api.Domain;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Builders
{
    public class LeagueTableDrillDownRepository : ILeagueTableDrillDownRepository
    {
        private readonly ILeagueMatchesRepository _leagueMatchesRepository;
        private readonly ILeagueFormRepository _leagueFormRepository;
        private LeagueRepositoryContext Context { get; }

        public LeagueTableDrillDownRepository(
            LeagueRepositoryContext context, 
            ILeagueMatchesRepository leagueMatchesRepository,
            ILeagueFormRepository leagueFormRepository)
        {
            _leagueMatchesRepository = leagueMatchesRepository;
            _leagueFormRepository = leagueFormRepository;
            Context = context;
        }

        public LeagueRowDrillDown GetDrillDown(int tier, string season, string team)
        {
            var result = new LeagueRowDrillDown();

            using(var conn = Context.Database.GetDbConnection())
            {
                result.Form = _leagueFormRepository.GetLeagueForm(tier, season, team);
                result.Positions = GetIncrementalLeaguePositions(conn, tier, season, team);
            }

            return result;
        }

        private List<LeaguePosition> GetIncrementalLeaguePositions(DbConnection conn, int tier, string season, string team)
        {
            var seasonStartYear = season.Substring(0, 4);
            var seasonEndYear = season.Substring(7, 4);

            var matchDetails = _leagueMatchesRepository.GetLeagueMatches(tier, season);
            var pointDeductions = CommonStuff.GetPointDeductions(conn, tier, season);

            var teams = matchDetails.Select(m => m.HomeTeam).Distinct().ToList();

            var positions = new List<LeaguePosition>();

            var dates = matchDetails.Select(m => m.Date).Distinct().OrderBy(m => m.Date).ToList();
            var lastDate = dates.Last().AddDays(1);
            var firstDate = dates.First();

            for (var dt = firstDate; dt <= lastDate; dt = dt.AddDays(1))
            {
                var leagueTable = new List<LeagueTableRow>();

                var filteredMatchDetails = matchDetails.Where(m => m.Date < dt).ToList();
                var filteredHomeTeams = filteredMatchDetails.Select(m => m.HomeTeam).ToList();
                var filteredAwayTeams = filteredMatchDetails.Select(m => m.AwayTeam).ToList();
                var filteredTeams = filteredHomeTeams.Union(filteredAwayTeams).ToList();

                var missingTeams = teams.Where(p => filteredTeams.All(p2 => p2 != p)).ToList();

                foreach (var t in missingTeams)
                {
                    leagueTable.Add(new LeagueTableRow
                    {
                        Team = t,
                        Won = 0,
                        Drawn = 0,
                        Lost = 0,
                        GoalsFor = 0,
                        GoalsAgainst = 0
                    });
                }
                
                CommonStuff.AddLeagueRows(leagueTable, filteredMatchDetails);
                CommonStuff.IncludePointDeductions(leagueTable, pointDeductions);

                leagueTable = CommonStuff.SortLeagueTable(leagueTable);

                CommonStuff.SetLeaguePosition(leagueTable);

                positions.Add(
                    new LeaguePosition
                    {
                        Date = dt,
                        Position = leagueTable.Where(l => l.Team == team).Select(r => r.Position).Single()
                    }
                );
            }

            return positions;
        }
    }
}
