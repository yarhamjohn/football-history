using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Builders
{
    public class LeagueTableDrillDownBuilder : ILeagueTableDrillDownBuilder
    {
        private readonly ILeagueMatchesRepository _leagueMatchesRepository;
        private readonly ILeagueFormRepository _leagueFormRepository;
        private readonly IPointDeductionsRepository _pointDeductionsRepository;

        public LeagueTableDrillDownBuilder(ILeagueMatchesRepository leagueMatchesRepository,
            ILeagueFormRepository leagueFormRepository,
            IPointDeductionsRepository pointDeductionsRepository)
        {
            _leagueMatchesRepository = leagueMatchesRepository;
            _leagueFormRepository = leagueFormRepository;
            _pointDeductionsRepository = pointDeductionsRepository;
        }

        public LeagueRowDrillDown GetDrillDown(int tier, string season, string team)
        {
            return new LeagueRowDrillDown
            {
                Form = _leagueFormRepository.GetLeagueForm(tier, season, team),
                Positions = GetIncrementalLeaguePositions(tier, season, team)
            };
        }

        private List<LeaguePosition> GetIncrementalLeaguePositions(int tier, string season, string team)
        {
            var matchDetails = _leagueMatchesRepository.GetLeagueMatches(tier, season);
            var pointDeductions = _pointDeductionsRepository.GetPointDeductions(tier, season);

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
