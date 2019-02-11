using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using FootballHistory.Api.Domain;
using FootballHistory.Api.Models.Controller;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Repositories
{
    public class LeagueFormRepository : ILeagueFormRepository
    {
        private LeagueRepositoryContext Context { get; }

        public LeagueFormRepository(LeagueRepositoryContext context)
        {
            Context = context;
        }

        public List<MatchResultOld> GetLeagueForm(int tier, string season, string team)
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
	,CASE WHEN lm.HomeGoals > lm.AwayGoals THEN 'W'
		  WHEN lm.AwayGoals > lm.HomeGoals THEN 'L' 
		  ELSE 'D' END AS Result
FROM dbo.LeagueMatches AS lm
INNER JOIN dbo.Divisions d ON d.Id = lm.DivisionId
INNER JOIN dbo.Clubs AS hc ON hc.Id = lm.HomeClubId
WHERE d.Tier = @Tier
    AND (hc.Name = @Team)
    AND lm.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)

UNION ALL

SELECT lm.MatchDate
	,CASE WHEN lm.HomeGoals < lm.AwayGoals THEN 'W'
		  WHEN lm.AwayGoals < lm.HomeGoals THEN 'L' 
		  ELSE 'D' END AS Result
FROM dbo.LeagueMatches AS lm
INNER JOIN dbo.Divisions d ON d.Id = lm.DivisionId
INNER JOIN dbo.Clubs AS ac ON ac.Id = lm.AwayClubId
WHERE d.Tier = @Tier
    AND (ac.Name = @Team)
    AND lm.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)

ORDER BY MatchDate
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
        private static List<MatchResultOld> GetLeagueForm(DbCommand cmd)
        {
            var form = new List<MatchResultOld>();
            
            using(var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    form.Add(
                        new MatchResultOld
                        {
                            MatchDate = reader.GetDateTime(0),
                            Result = reader.GetString(1)
                        }
                    );
                }
            } 

            return form;
        }
    }
}
