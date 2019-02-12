using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using FootballHistory.Api.Domain;
using FootballHistory.Api.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Repositories
{
    public class LeagueFormRepository : ILeagueFormRepository
    {
        private LeagueFormRepositoryContext Context { get; }

        public LeagueFormRepository(LeagueFormRepositoryContext context)
        {
            Context = context;
        }

        public List<MatchDetailModel> GetLeagueMatches(int tier, string season, string team)
        {
            using(var conn = Context.Database.GetDbConnection())
            {
                var cmd = GetDbCommand(conn, tier, season, team);
                return GetLeagueForm(cmd);
            }
        }

        private static DbCommand GetDbCommand(DbConnection conn, int tier, string season, string team)
        {
            const string sql = @"
SELECT lm.MatchDate
    ,hc.Name AS HomeTeam
    ,ac.Name AS AwayTeam
	,lm.HomeGoals
	,lm.AwayGoals
FROM dbo.LeagueMatches AS lm
INNER JOIN dbo.Divisions d ON d.Id = lm.DivisionId
INNER JOIN dbo.Clubs AS hc ON hc.Id = lm.HomeClubId
INNER JOIN dbo.Clubs AS ac ON ac.Id = lm.AwayClubId
WHERE d.Tier = @Tier
    AND (hc.Name = @Team OR ac.Name = @Team)
    AND lm.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)
";
            
            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@Tier", tier));
            cmd.Parameters.Add(new SqlParameter("@SeasonStartYear", season.Substring(0, 4)));
            cmd.Parameters.Add(new SqlParameter("@SeasonEndYear", season.Substring(7, 4)));
            cmd.Parameters.Add(new SqlParameter("@Team", team));

            return cmd;
        }
        private static List<MatchDetailModel> GetLeagueForm(DbCommand cmd)
        {
            var form = new List<MatchDetailModel>();
            
            using(var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    form.Add(
                        new MatchDetailModel
                        {
                            Date = reader.GetDateTime(0),
                            HomeTeam = reader.GetString(1),
                            AwayTeam = reader.GetString(2),
                            HomeGoals = reader.GetByte(3),
                            AwayGoals = reader.GetByte(4),
                        }
                    );
                }
            } 

            return form;
        }
    }
}
