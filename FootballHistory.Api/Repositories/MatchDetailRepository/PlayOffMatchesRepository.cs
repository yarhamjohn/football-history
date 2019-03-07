using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using FootballHistory.Api.Controllers;
using FootballHistory.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Repositories.MatchDetailRepository
{
    public class PlayOffMatchesRepository : IPlayOffMatchesRepository
    {
        private PlayOffMatchesRepositoryContext RepositoryContext { get; }

        public PlayOffMatchesRepository(PlayOffMatchesRepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }
        public List<MatchDetailModel> GetPlayOffMatches(params SeasonTierFilter[] filters)
        {
            using(var conn = RepositoryContext.Database.GetDbConnection())
            {
                var cmd = GetDbCommand(conn, filters.ToList());
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

        private static DbCommand GetDbCommand(DbConnection conn, List<SeasonTierFilter> filters)
        {
            conn.Open();
            
            var cmd = conn.CreateCommand();
            var fullSql = new StringBuilder();

            for (var i = 0; i < filters.Count; i++)
            {
                fullSql.Append(BuildSql(i));

                if (i < filters.Count - 1)
                {
                    fullSql.Append("\n UNION ALL \n");
                }

                cmd.Parameters.Add(new SqlParameter($"@Tier{i}", filters[i].Tier));
                cmd.Parameters.Add(new SqlParameter($"@SeasonStartYear{i}", filters[i].Season.Substring(0, 4)));
                cmd.Parameters.Add(new SqlParameter($"@SeasonEndYear{i}", filters[i].Season.Substring(7, 4)));

            }

            cmd.CommandText = fullSql.ToString();
            return cmd;
        }

        private static string BuildSql(int num)
        {
            return $@"
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
WHERE d.Tier = @Tier{num}
    AND pom.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear{num}, 7, 1) AND DATEFROMPARTS(@SeasonEndYear{num}, 6, 30)
";
        }
    }
}
