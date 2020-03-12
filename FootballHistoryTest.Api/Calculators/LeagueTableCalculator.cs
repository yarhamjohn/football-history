using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Controllers;
using FootballHistoryTest.Api.Repositories.Match;

namespace FootballHistoryTest.Api.Calculators
{
    public static class LeagueTableCalculator
    {
        public static List<LeagueTableRow> GetLeagueTable(List<MatchModel> leagueMatches, List<MatchModel> playOffMatches)
        {
            var leagueTable = new List<LeagueTableRow>();
            
            var playOffWinner = MatchCalculator.GetMatchWinner(playOffMatches.Single(m => m.Round == "Final"));

            var teams = leagueMatches.Select(m => m.HomeTeam).Distinct();

            foreach (var team in teams)
            {
                var teamMatches = leagueMatches.Where(m => MatchCalculator.MatchInvolvesTeam(m, team)).ToList();
                var homeTeamMatches = leagueMatches.Where(m => m.HomeTeam == team).ToList();
                var awayTeamMatches = leagueMatches.Where(m => m.AwayTeam == team).ToList();
                leagueTable.Add(new LeagueTableRow
                {
                    Team = team, 
                    Played = teamMatches.Count,
                    Won = teamMatches.Count(m => MatchCalculator.TeamWonMatch(m, team)),
                    Lost = teamMatches.Count(m => MatchCalculator.TeamLostMatch(m, team)),
                    Drawn = teamMatches.Count(m => MatchCalculator.TeamDrewMatch(m, team)),
                    GoalsFor = homeTeamMatches.Sum(m => m.HomeGoals) + awayTeamMatches.Sum(m => m.AwayGoals),
                    GoalsAgainst = homeTeamMatches.Sum(m => m.AwayGoals) + awayTeamMatches.Sum(m => m.HomeGoals),
                });
            }
            
            leagueTable.ForEach(r => r.GoalDifference = r.GoalsFor - r.GoalsAgainst);
            
            return leagueTable;
        }
    }
}