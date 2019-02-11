using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using FootballHistory.Api.Domain;
using FootballHistory.Api.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Repositories
{
    public class PlayOffMatchesRepository : IPlayOffMatchesRepository
    {
        private PlayOffMatchesRepositoryContext RepositoryContext { get; }

        public PlayOffMatchesRepository(PlayOffMatchesRepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public List<MatchDetailModel> GetPlayOffMatches(int tier, string season)
        {
            using(var conn = RepositoryContext.Database.GetDbConnection())
            {
                var cmd = GetDbCommand(conn, tier, season);
                return GetPlayOffMatchDetails(cmd);
            }
        }

        private static List<MatchDetailModel> GetPlayOffMatchDetails(DbCommand cmd)
        {
            var matchDetails = new List<MatchDetailModel>();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var extraTime = reader.GetBoolean(8);
                    var penaltyShootout = reader.GetBoolean(11);

                    matchDetails.Add(
                        new MatchDetailModel
                        {
                            Round = reader.GetString(0),
                            Date = reader.GetDateTime(1),
                            HomeTeam = reader.GetString(2),
                            HomeTeamAbbreviation = reader.GetString(3),
                            AwayTeam = reader.GetString(4),
                            AwayTeamAbbreviation = reader.GetString(5),
                            HomeGoals = reader.GetByte(6),
                            AwayGoals = reader.GetByte(7),
                            ExtraTime = extraTime,
                            HomeGoalsET = extraTime ? reader.GetByte(9) : (int?) null,
                            AwayGoalsET = extraTime ? reader.GetByte(10) : (int?) null,
                            PenaltyShootout = penaltyShootout,
                            HomePenaltiesTaken = penaltyShootout ? reader.GetByte(12) : (int?) null,
                            HomePenaltiesScored = penaltyShootout ? reader.GetByte(13) : (int?) null,
                            AwayPenaltiesTaken = penaltyShootout ? reader.GetByte(14) : (int?) null,
                            AwayPenaltiesScored = penaltyShootout ? reader.GetByte(15) : (int?) null
                        }
                    );
                }
            }

            return matchDetails;
        }

        private static DbCommand GetDbCommand(DbConnection conn, int tier, string season)
        {
            var sql = @"
SELECT pom.Round
    ,pom.matchDate
    ,hc.Name AS HomeTeam
    ,hc.Abbreviation AS HomeAbbreviation
    ,ac.Name as AwayTeam
    ,ac.Abbreviation as AwayAbbreviation
    ,pom.HomeGoals
    ,pom.AwayGoals
    ,pom.ExtraTime AS ExtraTime
    ,pom.HomeGoalsET
    ,pom.AwayGoalsET
    ,pom.PenaltyShootout
    ,pom.HomePenaltiesTaken
    ,pom.HomePenaltiesScored
    ,pom.AwayPenaltiesTaken
    ,pom.AwayPenaltiesScored
FROM dbo.PlayOffMatches AS pom
INNER JOIN dbo.Divisions d ON d.Id = pom.DivisionId
INNER JOIN dbo.Clubs AS hc ON hc.Id = pom.HomeClubId
INNER JOIN dbo.Clubs AS ac ON ac.Id = pom.AwayClubId
WHERE d.Tier = @Tier
    AND pom.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)
";

            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@Tier", tier));
            cmd.Parameters.Add(new SqlParameter("@SeasonStartYear", season.Substring(0, 4)));
            cmd.Parameters.Add(new SqlParameter("@SeasonEndYear", season.Substring(7, 4)));
            
            return cmd;
        }
    }
}
