using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.LeagueSeason.LeagueTable;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using FootballHistory.Api.Repositories.PointDeductionRepository;

namespace FootballHistory.Api.LeagueSeason.LeagueTableDrillDown
{
    public class LeagueTableDrillDownBuilder : ILeagueTableDrillDownBuilder
    {
        public LeagueTableDrillDown Build(string team, List<MatchDetailModel> matchDetails, List<PointDeductionModel> pointDeductions)
        {
            return new LeagueTableDrillDown
            {
                Form = GenerateForm(matchDetails, team),
                Positions = new List<LeaguePosition>()//GetIncrementalLeaguePositions(matchDetails, pointDeductions, team)
            };
        }

        private static List<Match> GenerateForm(List<MatchDetailModel> leagueMatches, string team)
        {
            var matches = leagueMatches.Where(m => m.HomeTeam == team || m.AwayTeam == team).ToList();
            
            var numMatchDates = matches.Select(m => m.Date).Distinct().Count();
            if (numMatchDates < matches.Count)
            {
                throw new Exception($"Multiple matches involving {team} were found with the same match date.");
            }
            
            return matches
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
            var homeWin = match.HomeTeam == team && match.HomeGoals > match.AwayGoals;
            var awayWin = match.AwayTeam == team && match.HomeGoals < match.AwayGoals;
            if (homeWin || awayWin)
            {
                return "W";
            }

            var draw = match.HomeGoals == match.AwayGoals;
            if (draw)
            {
                return "D";
            }

            return "L";
        }

        private static List<LeaguePosition> GetIncrementalLeaguePositions(List<MatchDetailModel> matchDetails, List<PointDeductionModel> pointDeductions, string team)
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

//                _leagueTable.RemoveRows();
//                _leagueTable.AddMissingTeams(missingTeams);
//                _leagueTable.AddLeagueRows(filteredMatchDetails);
//                _leagueTable.IncludePointDeductions(pointDeductions);
//                _leagueTable.SortLeagueTable();
//                _leagueTable.SetLeaguePosition();
//
//                positions.Add(
//                    new LeaguePosition
//                    {
//                        Date = dt,
//                        Position = _leagueTable.GetTable().Where(l => l.Team == team).Select(r => r.Position).Single()
//                    }
//                );
            }

            return positions;
        }
    }
}
