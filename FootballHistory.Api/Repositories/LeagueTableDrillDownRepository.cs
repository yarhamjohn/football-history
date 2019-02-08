using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using FootballHistory.Api.Domain;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Repositories
{
    public class LeagueTableDrillDownRepository : ILeagueTableDrillDownRepository
    {
        private readonly ILeagueMatchesRepository _leagueMatchesRepository;
        private LeagueSeasonContext Context { get; }

        public LeagueTableDrillDownRepository(LeagueSeasonContext context, ILeagueMatchesRepository leagueMatchesRepository)
        {
            _leagueMatchesRepository = leagueMatchesRepository;
            Context = context;
        }

        public LeagueRowDrillDown GetDrillDown(int tier, string season, string team)
        {
            var result = new LeagueRowDrillDown();

            using(var conn = Context.Database.GetDbConnection())
            {
                result.Form = GetLeagueForm(conn, tier, season, team);
                result.Positions = GetIncrementalLeaguePositions(conn, tier, season, team);
            }

            return result;
        }

        private List<MatchResultOld> GetLeagueForm(DbConnection conn, int tier, string season, string team)
        {
            var seasonStartYear = season.Substring(0, 4);
            var seasonEndYear = season.Substring(7, 4);

            var sql = @"
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
            
            var form = new List<MatchResultOld>();

            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@Tier", tier));
            cmd.Parameters.Add(new SqlParameter("@SeasonStartYear", seasonStartYear));
            cmd.Parameters.Add(new SqlParameter("@SeasonEndYear", seasonEndYear));
            cmd.Parameters.Add(new SqlParameter("@Team", team));

            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
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
            else 
            {
                System.Console.WriteLine("No rows found");
            }
            reader.Close();
            conn.Close();

            return form;
        }

        private List<LeaguePosition> GetIncrementalLeaguePositions(DbConnection conn, int tier, string season, string team)
        {
            var seasonStartYear = season.Substring(0, 4);
            var seasonEndYear = season.Substring(7, 4);

            var matchDetails = _leagueMatchesRepository.GetLeagueMatches(tier, season);
            var pointDeductions = CommonStuff.GetPointDeductions(conn, tier, season);

            var teams = matchDetails.Select(m => m.HomeTeam).Distinct().ToList();

            var positions = new List<LeaguePosition>();

            var dates = matchDetails.Select(m => m.Date).Distinct().OrderBy(m => m.Date).ToList();
            var lastDate = dates.Last().AddDays(1);
            var firstDate = dates.First();

            for (var dt = firstDate; dt <= lastDate; dt = dt.AddDays(1))
            {
                var leagueTable = new List<LeagueTableRow>();

                var filteredMatchDetails = matchDetails.Where(m => m.Date < dt).ToList();
                var filteredHomeTeams = filteredMatchDetails.Select(m => m.HomeTeam).ToList();
                var filteredAwayTeams = filteredMatchDetails.Select(m => m.AwayTeam).ToList();
                var filteredTeams = filteredHomeTeams.Union(filteredAwayTeams).ToList();

                var missingTeams = teams.Where(p => filteredTeams.All(p2 => p2 != p)).ToList();

                foreach (var t in missingTeams)
                {
                    leagueTable.Add(new LeagueTableRow
                    {
                        Team = t,
                        Won = 0,
                        Drawn = 0,
                        Lost = 0,
                        GoalsFor = 0,
                        GoalsAgainst = 0
                    });
                }
                
                CommonStuff.AddLeagueRows(leagueTable, filteredMatchDetails);
                CommonStuff.IncludePointDeductions(leagueTable, pointDeductions);

                leagueTable = CommonStuff.SortLeagueTable(leagueTable);

                CommonStuff.SetLeaguePosition(leagueTable);

                positions.Add(
                    new LeaguePosition
                    {
                        Date = dt,
                        Position = leagueTable.Where(l => l.Team == team).Select(r => r.Position).Single()
                    }
                );
            }

            return positions;
        }
    }
}
