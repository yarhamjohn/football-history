using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders
{
    public class LeagueTableBuilder : ILeagueTableBuilder
    {
        public LeagueTab Build(List<MatchDetailModel> leagueMatches)
        {
            var leagueTable = new LeagueTab();

            var teams = GetTeams(leagueMatches);
            foreach (var team in teams)
            {
                var matches = GetGames(leagueMatches, team);
                if (LeagueMatchesAreInvalid(matches))
                {
                    throw new Exception("An invalid set of league matches were provided.");
                }
                
                leagueTable.Rows.Add(
                    new LeagueTableRow
                    {
                        Team = team,
                        Played = matches.Count,
                        Won = CountWins(matches, team),
                        Lost = CountDefeats(matches, team),
                        Drawn = CountDraws(matches, team),
                        GoalsFor = CountGoalsFor(matches, team),
                        GoalsAgainst = CountGoalsAgainst(matches, team),
                        GoalDifference = CalculateGoalDifference(matches, team)
                    }
                );
            }
            
            return leagueTable;
        }

        private int CalculateGoalDifference(List<MatchDetailModel> matches, string team)
        {
            return CountGoalsFor(matches, team) - CountGoalsAgainst(matches, team);
        }

        private int CountGoalsFor(List<MatchDetailModel> matches, string team)
        {
            var homeGoalsFor = GetHomeGames(matches, team).Sum(g => g.HomeGoals);
            var awayGoalsFor = GetAwayGames(matches, team).Sum(g => g.AwayGoals);
            return homeGoalsFor + awayGoalsFor;
        }
        
        private int CountGoalsAgainst(List<MatchDetailModel> matches, string team)
        {
            var homeGoalsAgainst = GetHomeGames(matches, team).Sum(g => g.AwayGoals);
            var awayGoalsAgainst = GetAwayGames(matches, team).Sum(g => g.HomeGoals);
            return homeGoalsAgainst + awayGoalsAgainst;
        }
        
        private int CountWins(List<MatchDetailModel> matches, string team)
        {
            var homeWins = GetHomeGames(matches, team).Count(g => g.HomeGoals > g.AwayGoals);
            var awayWins = GetAwayGames(matches, team).Count(g => g.HomeGoals < g.AwayGoals);
            return homeWins + awayWins;
        }
        
        private int CountDraws(List<MatchDetailModel> matches, string team)
        {
            var homeDraws = GetHomeGames(matches, team).Count(g => g.HomeGoals == g.AwayGoals);
            var awayDraws = GetAwayGames(matches, team).Count(g => g.HomeGoals == g.AwayGoals);
            return homeDraws + awayDraws;
        }
        
        private int CountDefeats(List<MatchDetailModel> matches, string team)
        {
            var homeDefeats = GetHomeGames(matches, team).Count(g => g.HomeGoals < g.AwayGoals);
            var awayDefeats = GetAwayGames(matches, team).Count(g => g.HomeGoals > g.AwayGoals);
            return homeDefeats + awayDefeats;
        }

        private static bool LeagueMatchesAreInvalid(List<MatchDetailModel> games)
        {
            return games.Select(g => (g.HomeTeam, g.AwayTeam)).Distinct().Count() != games.Count;
        }

        private static List<MatchDetailModel> GetGames(List<MatchDetailModel> leagueMatches, string team)
        {
            var games = new List<MatchDetailModel>();
            games.AddRange(GetHomeGames(leagueMatches, team));
            games.AddRange(GetAwayGames(leagueMatches, team));

            return games;
        }

        private static List<MatchDetailModel> GetHomeGames(List<MatchDetailModel> matches, string team)
        {
            return matches.Where(m => m.HomeTeam == team).ToList();
        }
        
        private static List<MatchDetailModel> GetAwayGames(List<MatchDetailModel> matches, string team)
        {
            return matches.Where(m => m.AwayTeam == team).ToList();
        }

        private List<string> GetTeams(List<MatchDetailModel> leagueMatches)
        {
            var homeTeams = leagueMatches.Select(m => m.HomeTeam).ToList();
            var awayTeams = leagueMatches.Select(m => m.AwayTeam).ToList();
            
            return homeTeams.Union(awayTeams).ToList();
        }
        
    }
}
