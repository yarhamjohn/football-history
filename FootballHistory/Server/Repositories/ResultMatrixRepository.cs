using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using FootballHistory.Server.Domain;
using FootballHistory.Server.Models.LeagueSeason;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Server.Repositories
{
    public class ResultMatrixRepository : IResultMatrixRepository
    {
        private LeagueSeasonContext Context { get; }

        public ResultMatrixRepository(LeagueSeasonContext context)
        {
            Context = context;
        }

        public ResultMatrix GetResultMatrix(int tier, string season)
        {
            var seasonStartYear = season.Substring(0, 4);
            var seasonEndYear = season.Substring(7, 4);

            using(var conn = Context.Database.GetDbConnection())
            {
                var matchDetails = GetLeagueMatchDetails(conn, tier, seasonStartYear, seasonEndYear);
                return CreateResultMatrix(matchDetails);
            }
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

        private List<MatchDetail> GetLeagueMatchDetails(DbConnection conn, int tier, string seasonStartYear, string seasonEndYear)
        {
            var sql = @"
SELECT d.Name AS CompetitionName
    ,lm.matchDate
    ,hc.Name AS HomeTeam
    ,hc.Abbreviation AS HomeAbbreviation
    ,ac.Name as AwayTeam
    ,ac.Abbreviation as AwayAbbreviation
    ,lm.HomeGoals
    ,lm.AwayGoals
FROM dbo.LeagueMatches AS lm
INNER JOIN dbo.Divisions d ON d.Id = lm.DivisionId
INNER JOIN dbo.Clubs AS hc ON hc.Id = lm.HomeClubId
INNER JOIN dbo.Clubs AS ac ON ac.Id = lm.AwayClubId
WHERE d.Tier = @Tier
    AND lm.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)
";

            var matchDetails = new List<MatchDetail>();

            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@Tier", tier));
            cmd.Parameters.Add(new SqlParameter("@SeasonStartYear", seasonStartYear));
            cmd.Parameters.Add(new SqlParameter("@SeasonEndYear", seasonEndYear));

            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    matchDetails.Add(
                        new MatchDetail
                        {
                            Competition = reader.GetString(0),
                            Date = reader.GetDateTime(1),
                            HomeTeam = reader.GetString(2),
                            HomeTeamAbbreviation = reader.GetString(3),
                            AwayTeam = reader.GetString(4),
                            AwayTeamAbbreviation = reader.GetString(5),
                            HomeGoals = reader.GetByte(6),
                            AwayGoals = reader.GetByte(7),
                            ExtraTime = false,
                            PenaltyShootout = false,
                            Round = "League"
                        }
                    );
                }
            }
            else 
            {
                System.Console.WriteLine("No rows found");
            }
            reader.Close();
            conn.Close();

            return matchDetails;
        }
    }
}
