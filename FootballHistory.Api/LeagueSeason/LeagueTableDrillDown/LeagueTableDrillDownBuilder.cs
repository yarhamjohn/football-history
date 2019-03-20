using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.LeagueSeason.LeagueTable;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using FootballHistory.Api.Repositories.PointDeductionRepository;

namespace FootballHistory.Api.LeagueSeason.LeagueTableDrillDown
{
    public class LeagueTableDrillDownBuilder : ILeagueTableDrillDownBuilder
    {
        private readonly ILeagueTableBuilder _leagueTableBuilder;

        public LeagueTableDrillDownBuilder(ILeagueTableBuilder leagueTableBuilder)
        {
            _leagueTableBuilder = leagueTableBuilder;
        }
        
        public LeagueTableDrillDown Build(string team, List<MatchDetailModel> matchDetails, List<PointDeductionModel> pointDeductions, LeagueDetailModel leagueDetailModel)
        {
            if (matchDetails.Count == 0 || !matchDetails.Any(m => m.HomeTeam == team || m.AwayTeam == team))
            {
                return new LeagueTableDrillDown {Form = new List<Match>(), Positions = new List<LeaguePosition>()};
            }
            
            return new LeagueTableDrillDown
            {
                Form = GenerateForm(matchDetails, team),
                Positions = GetDailyLeaguePositions(matchDetails, pointDeductions, team, leagueDetailModel)
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

        private List<LeaguePosition> GetDailyLeaguePositions(List<MatchDetailModel> matches, List<PointDeductionModel> pointDeductions, string team, LeagueDetailModel leagueDetailModel)
        {
            var dates = matches.Select(m => m.Date).Distinct().OrderBy(m => m.Date).ToList();
            var startDate = dates.First();
            var endDate = dates.Last().AddDays(1);

            var positions = new List<LeaguePosition>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var matchesToDate = matches.Where(m => m.Date < date).ToList();
                var missingTeams = GetMissingTeams(matches, matchesToDate, date);
                var leagueTable = _leagueTableBuilder.BuildWithoutStatuses(matchesToDate, pointDeductions, leagueDetailModel, missingTeams);

                var position = leagueTable.Rows.Where(r => r.Team == team).Select(r => r.Position).Single();
                
                positions.Add(new LeaguePosition {Date = date, Position = position});
            }

            return positions;
        }

        private static List<string> GetMissingTeams(List<MatchDetailModel> matches, List<MatchDetailModel> matchesToDate, DateTime date)
        {
            var allTeams = matches
                .Select(m => new List<string> {m.HomeTeam, m.AwayTeam})
                .SelectMany(teams => teams)
                .Distinct()
                .ToList();
            
            var includedTeams = matchesToDate
                .Select(m => new List<string> {m.HomeTeam, m.AwayTeam})
                .SelectMany(teams => teams)
                .Distinct()
                .ToList();
            
            return allTeams.Where(all => includedTeams.All(inc => inc != all)).ToList();
        }
    }
}
