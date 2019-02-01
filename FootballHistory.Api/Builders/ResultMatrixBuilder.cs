using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Domain.Models;
using FootballHistory.Api.Models.LeagueSeason;

namespace FootballHistory.Api.Builders
{
    public class ResultMatrixBuilder : IResultMatrixBuilder
    {
        public ResultMatrix Build(List<MatchDetailModel> matchDetails)
        {
            return CreateResultMatrix(matchDetails);
        }

        private static ResultMatrix CreateResultMatrix(IReadOnlyCollection<MatchDetailModel> matchDetails)
        {
            var teams = matchDetails
                .Select(m => (HomeTeam: m.HomeTeam, HomeTeamAbbreviation: m.HomeTeamAbbreviation))
                .Distinct()
                .ToList();

            var resultMatrix = new ResultMatrix();
            foreach (var (homeTeam, homeTeamAbbreviation) in teams)
            {
                resultMatrix.Rows.Add(
                    new ResultMatrixRow
                    {
                        HomeTeam = homeTeam,
                        HomeTeamAbbreviation = homeTeamAbbreviation,
                        Results = GetScores(matchDetails, homeTeam, homeTeamAbbreviation)
                    }
                );
            }

            return resultMatrix;
        }

        private static List<MatchResult> GetScores(IEnumerable<MatchDetailModel> matchDetails, string homeTeam, string homeTeamAbbreviation)
        {
            var homeGames = matchDetails.Where(m => m.HomeTeam == homeTeam).ToList();

            var resultScores = new List<MatchResult> { GetMatchResultAgainstSelf(homeTeam, homeTeamAbbreviation) };

            foreach(var game in homeGames)
            {
                resultScores.Add(
                    new MatchResult
                    {
                        AwayTeam = game.AwayTeam,
                        AwayTeamAbbreviation = game.AwayTeamAbbreviation,
                        HomeScore = game.HomeGoals,
                        AwayScore = game.AwayGoals,
                        MatchDate = game.Date
                    }
                );
            }

            return resultScores;
        }

        private static MatchResult GetMatchResultAgainstSelf(string homeTeam, string homeTeamAbbreviation)
        {
            return new MatchResult
            {
                AwayTeam = homeTeam, 
                AwayTeamAbbreviation = homeTeamAbbreviation, 
                HomeScore = null, 
                AwayScore = null,
                MatchDate = null
            };
        }
    }
}
