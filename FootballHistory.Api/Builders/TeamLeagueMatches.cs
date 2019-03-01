using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders
{
    public class TeamLeagueMatches
    {
        private readonly List<MatchDetailModel> _homeMatches;
        private readonly List<MatchDetailModel> _awayMatches;

        public TeamLeagueMatches(List<MatchDetailModel> leagueMatches, string team)
        {
            _homeMatches = leagueMatches.Where(m => m.HomeTeam == team).ToList();
            _awayMatches = leagueMatches.Where(m => m.AwayTeam == team).ToList();
        }

        public int CountGamesPlayed()
        {
            return _homeMatches.Count + _awayMatches.Count;
        }
            
        public int CalculateGoalDifference()
        {
            return CountGoalsFor() - CountGoalsAgainst();
        }

        public int CountGoalsFor()
        {
            var homeGoalsFor = _homeMatches.Sum(g => g.HomeGoals);
            var awayGoalsFor = _awayMatches.Sum(g => g.AwayGoals);
            return homeGoalsFor + awayGoalsFor;
        }
        
        public int CountGoalsAgainst()
        {
            var homeGoalsAgainst = _homeMatches.Sum(g => g.AwayGoals);
            var awayGoalsAgainst = _awayMatches.Sum(g => g.HomeGoals);
            return homeGoalsAgainst + awayGoalsAgainst;
        }
        
        public int CountWins()
        {
            var homeWins = _homeMatches.Count(g => g.HomeGoals > g.AwayGoals);
            var awayWins = _awayMatches.Count(g => g.HomeGoals < g.AwayGoals);
            return homeWins + awayWins;
        }
        
        public int CountDraws()
        {
            var homeDraws = _homeMatches.Count(g => g.HomeGoals == g.AwayGoals);
            var awayDraws = _awayMatches.Count(g => g.HomeGoals == g.AwayGoals);
            return homeDraws + awayDraws;
        }
        
        public int CountDefeats()
        {
            var homeDefeats = _homeMatches.Count(g => g.HomeGoals < g.AwayGoals);
            var awayDefeats = _awayMatches.Count(g => g.HomeGoals > g.AwayGoals);
            return homeDefeats + awayDefeats;
        }

        public bool AreInvalid()
        {
            var opponentPairsOne = _homeMatches.Select(g => (g.HomeTeam, g.AwayTeam)).ToList();
            var opponentPairsTwo = _awayMatches.Select(g => (g.HomeTeam, g.AwayTeam)).ToList();
                
            var sameTeamsOne = opponentPairsOne.Where(p => p.Item1 == p.Item2).ToList();
            var sameTeamsTwo = opponentPairsTwo.Where(p => p.Item1 == p.Item2).ToList();
                
            return opponentPairsOne.Distinct().Count() != _homeMatches.Count
                   || opponentPairsTwo.Distinct().Count() != _awayMatches.Count
                   || sameTeamsOne.Count > 0
                   || sameTeamsTwo.Count > 0;
        }
    }
}
