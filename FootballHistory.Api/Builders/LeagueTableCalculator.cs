using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders
{
    public class LeagueTableCalculator : ILeagueTableCalculator
    {
        public static int CountGamesPlayed(List<MatchDetailModel> homeGames, List<MatchDetailModel> awayGames)
        {
            return homeGames.Count + awayGames.Count;
        }
            
        public static int CalculateGoalDifference(List<MatchDetailModel> homeGames, List<MatchDetailModel> awayGames)
        {
            return CountGoalsFor(homeGames, awayGames) - CountGoalsAgainst(homeGames, awayGames);
        }

        public static int CountGoalsFor(List<MatchDetailModel> homeGames, List<MatchDetailModel> awayGames)
        {
            var homeGoalsFor = homeGames.Sum(g => g.HomeGoals);
            var awayGoalsFor = awayGames.Sum(g => g.AwayGoals);
            return homeGoalsFor + awayGoalsFor;
        }
        
        public static int CountGoalsAgainst(List<MatchDetailModel> homeGames, List<MatchDetailModel> awayGames)
        {
            var homeGoalsAgainst = homeGames.Sum(g => g.AwayGoals);
            var awayGoalsAgainst = awayGames.Sum(g => g.HomeGoals);
            return homeGoalsAgainst + awayGoalsAgainst;
        }
        
        public static int CountWins(List<MatchDetailModel> homeGames, List<MatchDetailModel> awayGames)
        {
            var homeWins = homeGames.Count(g => g.HomeGoals > g.AwayGoals);
            var awayWins = awayGames.Count(g => g.HomeGoals < g.AwayGoals);
            return homeWins + awayWins;
        }
        
        public static int CountDraws(List<MatchDetailModel> homeGames, List<MatchDetailModel> awayGames)
        {
            var homeDraws = homeGames.Count(g => g.HomeGoals == g.AwayGoals);
            var awayDraws = awayGames.Count(g => g.HomeGoals == g.AwayGoals);
            return homeDraws + awayDraws;
        }
        
        public static int CountDefeats(List<MatchDetailModel> homeGames, List<MatchDetailModel> awayGames)
        {
            var homeDefeats = homeGames.Count(g => g.HomeGoals < g.AwayGoals);
            var awayDefeats = awayGames.Count(g => g.HomeGoals > g.AwayGoals);
            return homeDefeats + awayDefeats;
        }

        public static int CalculatePoints(List<MatchDetailModel> homeGames, List<MatchDetailModel> awayGames)
        {
            return CountWins(homeGames, awayGames) * 3 + CountDraws(homeGames, awayGames);
        }
    }
}
