using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders
{
    public class ResultMatrixBuilder : IResultMatrixBuilder
    {
        public ResultMatrix Build(List<MatchDetailModel> matchDetails)
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

        private static List<ResultMatrixMatch> GetScores(IEnumerable<MatchDetailModel> matchDetails, string homeTeam, string homeTeamAbbreviation)
        {
            var homeGames = matchDetails.Where(m => m.HomeTeam == homeTeam).ToList();

            var resultScores = new List<ResultMatrixMatch> { GetMatchResultAgainstSelf(homeTeam, homeTeamAbbreviation) };

            foreach(var game in homeGames)
            {
                resultScores.Add(
                    new ResultMatrixMatch
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

        private static ResultMatrixMatch GetMatchResultAgainstSelf(string homeTeam, string homeTeamAbbreviation)
        {
            return new ResultMatrixMatch
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
