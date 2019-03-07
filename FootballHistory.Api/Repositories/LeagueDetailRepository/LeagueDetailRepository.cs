using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using FootballHistory.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Repositories.LeagueDetailRepository
{
    public class LeagueDetailRepository : ILeagueDetailRepository
    {
        private LeagueDetailRepositoryContext Context { get; }

        public LeagueDetailRepository(LeagueDetailRepositoryContext context)
        {
            Context = context;
        }

        public LeagueDetailModel GetLeagueInfo(int tier, string season)
        {
            var seasonTier = new List<(int, string)> {(tier, season)};
            var leagueDetails = GetLeagueInfos(seasonTier);
            return leagueDetails.FirstOrDefault();
        }
        
        public List<LeagueDetailModel> GetLeagueInfos(List<(int, string)> seasonTier)
        {
            using (var conn = Context.Database.GetDbConnection())
            {
                var cmd = GetDbCommand(conn, seasonTier);
                return CreateLeagueDetails(cmd);
            }
        }

        private static List<LeagueDetailModel> CreateLeagueDetails(DbCommand cmd)
        {
            var leagueDetails = new List<LeagueDetailModel>();
            
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    leagueDetails.Add(new LeagueDetailModel
                    {
                        Competition = reader.GetString(0),
                        TotalPlaces = reader.GetByte(1),
                        PromotionPlaces = reader.GetByte(2),
                        PlayOffPlaces = reader.GetByte(3),
                        RelegationPlaces = reader.GetByte(4),
                        Season = reader.GetString(5)
                    });
                }
            }

            return leagueDetails;
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

                cmd.Parameters.Add(new SqlParameter($"@Tier{i}", seasonTier[i].Item1));
                cmd.Parameters.Add(new SqlParameter($"@Season{i}", seasonTier[i].Item2));
            }

            cmd.CommandText = fullSql.ToString();
            return cmd;
        }

        private static string BuildSql(int num)
        {
            return $@"
SELECT d.Name AS CompetitionName
    ,ls.TotalPlaces
    ,ls.PromotionPlaces
    ,ls.PlayOffPlaces
    ,ls.RelegationPlaces
    ,ls.Season
FROM dbo.LeagueStatuses AS ls
INNER JOIN dbo.Divisions d ON d.Id = ls.DivisionId
WHERE d.Tier = @Tier{num} AND ls.Season = @Season{num}
";
        }
    }
}
