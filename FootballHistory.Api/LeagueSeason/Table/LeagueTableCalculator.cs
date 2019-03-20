using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using FootballHistory.Api.Repositories.PointDeductionRepository;

namespace FootballHistory.Api.LeagueSeason.Table
{
    public class LeagueTableCalculator : ILeagueTableCalculator
    {
        private readonly List<PointDeductionModel> _pointDeductions;
        private readonly List<MatchDetailModel> _homeGames;
        private readonly List<MatchDetailModel> _awayGames;

        public LeagueTableCalculator(List<MatchDetailModel> leagueMatches, List<PointDeductionModel> pointDeductions, string team)
        {
            _pointDeductions = pointDeductions.Where(d => d.Team == team).ToList();
            _homeGames = leagueMatches.Where(m => m.HomeTeam == team).ToList();
            _awayGames = leagueMatches.Where(m => m.AwayTeam == team).ToList();
        }
        
        public int CountGamesPlayed()
        {
            return _homeGames.Count + _awayGames.Count;
        }
            
        public int CalculateGoalDifference()
        {
            return CountGoalsFor() - CountGoalsAgainst();
        }

        public int CountGoalsFor()
        {
            var homeGoalsFor = _homeGames.Sum(g => g.HomeGoals);
            var awayGoalsFor = _awayGames.Sum(g => g.AwayGoals);
            return homeGoalsFor + awayGoalsFor;
        }
        
        public int CountGoalsAgainst()
        {
            var homeGoalsAgainst = _homeGames.Sum(g => g.AwayGoals);
            var awayGoalsAgainst = _awayGames.Sum(g => g.HomeGoals);
            return homeGoalsAgainst + awayGoalsAgainst;
        }
        
        public int CountWins()
        {
            var homeWins = _homeGames.Count(g => g.HomeGoals > g.AwayGoals);
            var awayWins = _awayGames.Count(g => g.HomeGoals < g.AwayGoals);
            return homeWins + awayWins;
        }
        
        public int CountDraws()
        {
            var homeDraws = _homeGames.Count(g => g.HomeGoals == g.AwayGoals);
            var awayDraws = _awayGames.Count(g => g.HomeGoals == g.AwayGoals);
            return homeDraws + awayDraws;
        }
        
        public int CountDefeats()
        {
            var homeDefeats = _homeGames.Count(g => g.HomeGoals < g.AwayGoals);
            var awayDefeats = _awayGames.Count(g => g.HomeGoals > g.AwayGoals);
            return homeDefeats + awayDefeats;
        }

        public int CalculatePoints()
        {
            return CountWins() * 3 + CountDraws() - CalculatePointsDeducted();
        }

        public int CalculatePointsDeducted()
        {
           return _pointDeductions.Sum(d => d.PointsDeducted);

        }
        
        public string GetPointDeductionReasons()
        {
            return string.Join(", ", _pointDeductions.Select(d => d.Reason));
        }
    }
}
