using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using FootballHistory.Server.Domain;
using FootballHistory.Server.Domain.Models;
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

        public List<MatchDetailModel> GetLeagueMatches(int tier, string season)
        {
            using(var conn = Context.Database.GetDbConnection())
            {
                var cmd = GetDbCommand(conn, tier, season);
                return GetMatchDetails(cmd);
            }
        }

        private static List<MatchDetailModel> GetMatchDetails(DbCommand cmd)
        {
            var matchDetails = new List<MatchDetailModel>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    matchDetails.Add(
                        new MatchDetailModel
                        {
                            Date = reader.GetDateTime(0),
                            HomeTeam = reader.GetString(1),
                            HomeTeamAbbreviation = reader.GetString(2),
                            AwayTeam = reader.GetString(3),
                            AwayTeamAbbreviation = reader.GetString(4),
                            HomeGoals = reader.GetByte(5),
                            AwayGoals = reader.GetByte(6),
                            ExtraTime = false,
                            PenaltyShootout = false
                        }
                    );
                }
            }

            return matchDetails;
        }

        private static DbCommand GetDbCommand(DbConnection conn, int tier, string season)
        {
            const string sql = @"
SELECT lm.matchDate
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
    AND lm.MatchDate BETWEEN DATEFROMPARTS(@StartYear, 7, 1) AND DATEFROMPARTS(@EndYear, 6, 30)
";

            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@Tier", tier));
            cmd.Parameters.Add(new SqlParameter("@StartYear", season.Substring(0, 4)));
            cmd.Parameters.Add(new SqlParameter("@EndYear", season.Substring(7, 4)));
            
            return cmd;
        }
    }
}
