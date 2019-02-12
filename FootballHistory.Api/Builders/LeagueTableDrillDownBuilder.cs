using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Repositories;
using FootballHistory.Api.Repositories.Models;
using Microsoft.AspNetCore.JsonPatch.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Builders
{
    public class LeagueTableDrillDownBuilder : ILeagueTableDrillDownBuilder
    {
        private readonly ILeagueTable _leagueTable;

        public LeagueTableDrillDownBuilder(ILeagueTable leagueTable)
        {
            _leagueTable = leagueTable;
        }
        
        public LeagueRowDrillDown Build(string team, List<MatchDetailModel> matchDetails, List<PointDeductionModel> pointDeductions)
        {
            return new LeagueRowDrillDown
            {
                Form = GenerateForm(matchDetails, team),
                Positions = GetIncrementalLeaguePositions(matchDetails, pointDeductions, team)
            };
        }

        private static List<Match> GenerateForm(IEnumerable<MatchDetailModel> leagueMatches, string team)
        {
            return leagueMatches.Where(m => m.HomeTeam == team || m.AwayTeam == team)
                .OrderBy(m => m.Date)
                .Select(match => new Match
                {
                    MatchDate = match.Date, 
                    Result = GetResult(match, team)
                })
                .ToList();
        }

        private static string GetResult(MatchDetailModel match, string team)
        {
            if (match.HomeGoals == match.AwayGoals)
            {
                return "D";
            }
            
            if (match.HomeTeam == team && match.HomeGoals > match.AwayGoals)
            {
                return "W";
            }
                        
            if (match.AwayTeam == team && match.HomeGoals < match.AwayGoals)
            {
                return "W";
            }

            return "L";
        }


        private List<LeaguePosition> GetIncrementalLeaguePositions(List<MatchDetailModel> matchDetails, List<PointDeductionModel> pointDeductions, string team)
        {
            var teams = matchDetails.Select(m => m.HomeTeam).Distinct().ToList();

            var positions = new List<LeaguePosition>();

            var dates = matchDetails.Select(m => m.Date).Distinct().OrderBy(m => m.Date).ToList();
            var lastDate = dates.Last().AddDays(1); // what if there are no matches??
            var firstDate = dates.First();

            for (var dt = firstDate; dt <= lastDate; dt = dt.AddDays(1))
            {
                var filteredMatchDetails = matchDetails.Where(m => m.Date < dt).ToList();
                var filteredHomeTeams = filteredMatchDetails.Select(m => m.HomeTeam).ToList();
                var filteredAwayTeams = filteredMatchDetails.Select(m => m.AwayTeam).ToList();
                var filteredTeams = filteredHomeTeams.Union(filteredAwayTeams).ToList();

                var missingTeams = teams.Where(p => filteredTeams.All(p2 => p2 != p)).ToList();

                _leagueTable.RemoveRows();
                _leagueTable.AddMissingTeams(missingTeams);
                _leagueTable.AddLeagueRows(filteredMatchDetails);
                _leagueTable.IncludePointDeductions(pointDeductions);
                _leagueTable.SortLeagueTable();
                _leagueTable.SetLeaguePosition();

                positions.Add(
                    new LeaguePosition
                    {
                        Date = dt,
                        Position = _leagueTable.GetTable().Where(l => l.Team == team).Select(r => r.Position).Single()
                    }
                );
            }

            return positions;
        }
    }
}
