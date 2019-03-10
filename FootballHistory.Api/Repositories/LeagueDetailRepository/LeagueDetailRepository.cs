using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using FootballHistory.Api.Domain;
using FootballHistory.Api.Repositories.TierRepository;
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

        public LeagueDetailModel GetLeagueInfo(SeasonTierFilter filter)
        {
            var leagueDetails = GetLeagueInfos(filter);
            return leagueDetails.FirstOrDefault();
        }
        
        public List<LeagueDetailModel> GetLeagueInfos(params SeasonTierFilter[] filters)
        {
            using (var conn = Context.Database.GetDbConnection())
            {
                var cmd = GetDbCommand(conn, filters.ToList());
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

                cmd.Parameters.Add(new SqlParameter($"@Tier{i}", (int) filters[i].Tier));
                cmd.Parameters.Add(new SqlParameter($"@Season{i}", filters[i].Season));
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
