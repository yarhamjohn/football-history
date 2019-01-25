using System.Collections.Generic;
using System.Linq;
using FootballHistory.Server.Models.LeagueSeason;

namespace FootballHistory.Server.Builders
{
    public class ResultMatrixBuilder : IResultMatrixBuilder
    {
        public ResultMatrix Build(List<MatchDetail> matchDetails)
        {
            return CreateResultMatrix(matchDetails);
        }

        private ResultMatrix CreateResultMatrix(List<MatchDetail> matchDetails)
        {
            var teams = matchDetails.Select(m => (m.HomeTeam, m.HomeTeamAbbreviation)).Distinct().ToList();

            var resultMatrix = new ResultMatrix();
            foreach (var team in teams)
            {
                resultMatrix.Rows.Add(
                    new ResultMatrixRow
                    {
                        HomeTeam = team.Item1,
                        HomeTeamAbbreviation = team.Item2,
                        Results = GetScores(matchDetails, team.Item1, team.Item1)
                    }
                );
            }

            return resultMatrix;
        }

        private List<MatchResult> GetScores(List<MatchDetail> matchDetails, string awayTeam, string homeTeam)
        {
            var homeGames = matchDetails.Where(m => m.HomeTeam == awayTeam).ToList();

            var resultScores = new List<MatchResult> { new MatchResult { AwayTeam = homeTeam, AwayTeamAbbreviation = null, HomeScore = null, AwayScore = null, MatchDate = null } };
            foreach(var game in homeGames)
            {
                resultScores.Add(
                    new MatchResult
                    {
                        AwayTeam = game.AwayTeam,
                        HomeScore = game.HomeGoals,
                        AwayScore = game.AwayGoals,
                        MatchDate = game.Date
                    }
                );
            }

            return resultScores;
        }
    }
}
