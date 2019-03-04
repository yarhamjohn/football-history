using System.Data.Common;
using System.Data.SqlClient;
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
            using (var conn = Context.Database.GetDbConnection())
            {
                var cmd = GetDbCommand(conn, tier, season);
                return CreateLeagueDetail(cmd);
            }
        }

        private static LeagueDetailModel CreateLeagueDetail(DbCommand cmd)
        {
            var leagueDetails = new LeagueDetailModel();
            
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    leagueDetails = new LeagueDetailModel
                    {
                        Competition = reader.GetString(0),
                        TotalPlaces = reader.GetByte(1),
                        PromotionPlaces = reader.GetByte(2),
                        PlayOffPlaces = reader.GetByte(3),
                        RelegationPlaces = reader.GetByte(4)
                    };
                }
            }

            return leagueDetails;
        }

        private static DbCommand GetDbCommand(DbConnection conn, int tier, string season)
        {
            const string sql = @"
SELECT d.Name AS CompetitionName
    ,ls.TotalPlaces
    ,ls.PromotionPlaces
    ,ls.PlayOffPlaces
    ,ls.RelegationPlaces
FROM dbo.LeagueStatuses AS ls
INNER JOIN dbo.Divisions d ON d.Id = ls.DivisionId
WHERE d.Tier = @Tier AND ls.Season = @Season
";

            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@Tier", tier));
            cmd.Parameters.Add(new SqlParameter("@Season", season));
            
            return cmd;
        }
    }
}
