using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using football.history.api.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace football.history.api.Repositories.Team
{
    public class TeamRepository : ITeamRepository
    {
        private readonly DatabaseContext _context;

        public TeamRepository(DatabaseContext context)
        {
            _context = context;
        }

        public List<TeamModel> GetTeamModels()
        {
            var conn = _context.Database.GetDbConnection();
            var cmd = GetDbCommand(conn);
            var result = SelectAllTeams(cmd);
            conn.Close();
            return result;
        }

        public List<TeamModel> GetTeamModels(int seasonStartYear, int tier)
        {
            var conn = _context.Database.GetDbConnection();

            var cmd = GetDbCommand(conn, seasonStartYear, tier);
            var result = SelectAllTeams(cmd);
            conn.Close();
            return result;
        }

        private static List<TeamModel> SelectAllTeams(DbCommand cmd)
        {
            var teams = new List<TeamModel>();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    teams.Add(
                        new TeamModel
                        {
                            Name = reader.GetString(0),
                            Abbreviation = reader.GetString(1)
                        });
                }
            }

            return teams;
        }

        private static DbCommand GetDbCommand(DbConnection conn)
        {
            const string sql = @"SELECT Name, Abbreviation FROM dbo.Clubs";

            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;

            return cmd;
        }

        private static DbCommand GetDbCommand(DbConnection conn, int seasonStartYear, int tier)
        {
            const string sql = @"
SELECT DISTINCT hc.Name, hc.Abbreviation
  FROM dbo.LeagueMatches AS m
LEFT JOIN dbo.Clubs AS hc
  ON hc.Id = m.HomeClubId
LEFT JOIN dbo.Clubs AS ac
  ON ac.Id = m.AwayClubId
LEFT JOIN dbo.Divisions AS d
  ON d.Id = m.DivisionId
WHERE d.Tier = @Tier AND m.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)
";

            const string sqlFor20192020 = @"
SELECT DISTINCT hc.Name, hc.Abbreviation
  FROM dbo.LeagueMatches AS m
LEFT JOIN dbo.Clubs AS hc
  ON hc.Id = m.HomeClubId
LEFT JOIN dbo.Clubs AS ac
  ON ac.Id = m.AwayClubId
LEFT JOIN dbo.Divisions AS d
  ON d.Id = m.DivisionId
WHERE d.Tier = @Tier AND m.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 8, 20)
";

            const string sqlFor20202021 = @"
SELECT DISTINCT hc.Name, hc.Abbreviation
  FROM dbo.LeagueMatches AS m
LEFT JOIN dbo.Clubs AS hc
  ON hc.Id = m.HomeClubId
LEFT JOIN dbo.Clubs AS ac
  ON ac.Id = m.AwayClubId
LEFT JOIN dbo.Divisions AS d
  ON d.Id = m.DivisionId
WHERE d.Tier = @Tier AND m.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 8, 21) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)
";

            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = seasonStartYear switch
            {
                2019 => sqlFor20192020,
                2020 => sqlFor20202021,
                _ => sql
            };

            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@Tier",
                    Value = tier
                });
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@SeasonStartYear",
                    Value = seasonStartYear
                });
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@SeasonEndYear",
                    Value = seasonStartYear + 1
                });

            return cmd;
        }
    }
}
