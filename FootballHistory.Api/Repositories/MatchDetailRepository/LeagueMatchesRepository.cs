using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using FootballHistory.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Repositories.MatchDetailRepository
{
    public class LeagueMatchesRepository : ILeagueMatchesRepository
    {
        private LeagueMatchesRepositoryContext Context { get; }

        public LeagueMatchesRepository(LeagueMatchesRepositoryContext context)
        {
            Context = context;
        }

        public List<MatchDetailModel> GetLeagueMatches(int tier, string season)
        {
            var seasonTier = new List<(int, string)> {(tier, season)};
            return GetLeagueMatches(seasonTier);
        }
        
        public List<MatchDetailModel> GetLeagueMatches(List<(int, string)> seasonTier)
        {
            using(var conn = Context.Database.GetDbConnection())
            {
                var cmd = GetDbCommand(conn, seasonTier);
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
        private static DbCommand GetDbCommand(DbConnection conn, List<(int, string)> seasonTier)
        {
            conn.Open();
            
            var cmd = conn.CreateCommand();
            var fullSql = new StringBuilder();

            for (var i = 0; i < seasonTier.Count; i++)
            {
                fullSql.Append(BuildSql(i));

                if (i < seasonTier.Count - 1)
                {
                    fullSql.Append("\n UNION ALL \n");
                }

                cmd.Parameters.Add(new SqlParameter($"@Tier{i}", seasonTier.Single().Item1));
                cmd.Parameters.Add(new SqlParameter($"@StartYear{i}", seasonTier.Single().Item2.Substring(0, 4)));
                cmd.Parameters.Add(new SqlParameter($"@EndYear{i}", seasonTier.Single().Item2.Substring(7, 4)));
            }

            cmd.CommandText = fullSql.ToString();
            return cmd;
        }

        private static string BuildSql(int num)
        {
            return $@"
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
WHERE d.Tier = @Tier{num}
    AND lm.MatchDate BETWEEN DATEFROMPARTS(@StartYear{num}, 7, 1) AND DATEFROMPARTS(@EndYear{num}, 6, 30)
";
        }
    }
}
