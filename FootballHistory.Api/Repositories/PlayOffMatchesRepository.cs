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
        private LeagueSeasonContext Context { get; }

        public PlayOffMatchesRepository(LeagueSeasonContext context)
        {
            Context = context;
        }

        public List<MatchDetailModel> GetPlayOffMatches(int tier, string season)
        {
            var seasonStartYear = season.Substring(0, 4);
            var seasonEndYear = season.Substring(7, 4);

            using(var conn = Context.Database.GetDbConnection())
            {
                return GetPlayOffMatchDetails(conn, tier, seasonStartYear, seasonEndYear);
            }
        }

        private List<MatchDetailModel> GetPlayOffMatchDetails(DbConnection conn, int tier, string seasonStartYear, string seasonEndYear)
        {
            var sql = @"
SELECT d.Name AS CompetitionName
    ,pom.Round
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

            var matchDetails = new List<MatchDetailModel>();

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
                    var extraTime = reader.GetBoolean(9); // == 1 ? true : false;
                    var penaltyShootout = reader.GetBoolean(12); // == 1 ? true : false;

                    matchDetails.Add(
                        new MatchDetailModel
                        {
                            Competition = reader.GetString(0),
                            Round = reader.GetString(1),
                            Date = reader.GetDateTime(2),
                            HomeTeam = reader.GetString(3),
                            HomeTeamAbbreviation = reader.GetString(4),
                            AwayTeam = reader.GetString(5),
                            AwayTeamAbbreviation = reader.GetString(6),
                            HomeGoals = reader.GetByte(7),
                            AwayGoals = reader.GetByte(8),
                            ExtraTime = extraTime,
                            HomeGoalsET = extraTime ? reader.GetByte(10) : (int?) null,
                            AwayGoalsET = extraTime ? reader.GetByte(11) : (int?) null,
                            PenaltyShootout = penaltyShootout,
                            HomePenaltiesTaken = penaltyShootout ? reader.GetByte(13) : (int?) null,
                            HomePenaltiesScored = penaltyShootout ? reader.GetByte(14) : (int?) null,
                            AwayPenaltiesTaken = penaltyShootout ? reader.GetByte(15) : (int?) null,
                            AwayPenaltiesScored = penaltyShootout ? reader.GetByte(16) : (int?) null
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
