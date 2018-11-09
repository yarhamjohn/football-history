using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace football_history.Server.Repositories
{
    public class FootballHistoryRepository : IFootballHistoryRepository
    {
        private FootballHistoryContext m_Context { get; }

        public FootballHistoryRepository(FootballHistoryContext context)
        {
            m_Context = context;
        }

        public LeagueTable GetLeagueTable(int tier, string season)
        {
            var seasonStartYear = season.Substring(0, 4);
            var seasonEndYear = season.Substring(7, 4);
            var leagueTable = new LeagueTable
            {
                Competition = "",
                Season = $"{seasonStartYear} - {seasonEndYear}",
                LeagueTableRow = new List<LeagueTableRow>()
            };

            var sql = @"
WITH Matches AS 
(
    SELECT m.HomeTeam
        ,m.AwayTeam
        ,m.HomeGoals
        ,m.AwayGoals
    FROM dbo.Matches m
    INNER JOIN dbo.Divisions d
        ON m.DivisionId = d.Id
    WHERE d.Tier = @Tier
        AND m.MatchDate BETWEEN DATEFROMPARTS(@SeasonStartYear, 7, 1) AND DATEFROMPARTS(@SeasonEndYear, 6, 30)
)

SELECT ROW_NUMBER() OVER(ORDER BY Points DESC, GoalDifference DESC, GoalsFor DESC) AS Position
	,Team
	,GamesPlayed
	,GamesWon
	,GamesDrawn
	,GamesLost
	,GoalsFor
	,GoalsAgainst
	,GoalDifference
	,Points
FROM (
	SELECT Team
		,HomeWins + HomeDraws + HomeLosses + AwayWins + AwayDraws + AwayLosses AS GamesPlayed
		,HomeWins + AwayWins AS GamesWon
		,HomeDraws + AwayDraws AS GamesDrawn
		,HomeLosses + AwayLosses AS GamesLost
		,HomeGoalsFor + AwayGoalsFor AS GoalsFor
		,HomeGoalsAgainst + AwayGoalsAgainst AS GoalsAgainst
		,HomeGoalsFor + AwayGoalsFor - HomeGoalsAgainst - AwayGoalsAgainst AS GoalDifference
		,(HomeWins + AwayWins) * 3 + HomeDraws + AwayDraws AS Points
	FROM (
		SELECT HomeTeam AS Team
		FROM Matches
		GROUP BY HomeTeam
	) t
	INNER JOIN (
			SELECT HomeTeam
				,SUM(CASE WHEN HomeGoals > AwayGoals THEN 1 ELSE 0 END) AS HomeWins
				,SUM(CASE WHEN HomeGoals = AwayGoals THEN 1 ELSE 0 END) AS HomeDraws 
				,SUM(CASE WHEN HomeGoals < AwayGoals THEN 1 ELSE 0 END) AS HomeLosses
				,SUM(HomeGoals) AS HomeGoalsFor
				,SUM(AwayGoals) AS HomeGoalsAgainst
			FROM Matches
			GROUP BY HomeTeam
		) AS m1 ON t.Team = m1.HomeTeam
	INNER JOIN (
			SELECT AwayTeam
				,SUM(CASE WHEN AwayGoals > HomeGoals THEN 1 ELSE 0 END) AS AwayWins
				,SUM(CASE WHEN AwayGoals = HomeGoals THEN 1 ELSE 0 END) AS AwayDraws
				,SUM(CASE WHEN AwayGoals < HomeGoals THEN 1 ELSE 0 END) AS AwayLosses
				,SUM(AwayGoals) AS AwayGoalsFor
				,SUM(HomeGoals) AS AwayGoalsAgainst
			FROM Matches
			GROUP BY AwayTeam
		) AS m2 ON t.Team = m2.AwayTeam
	) r
ORDER BY Position;

SELECT Name 
FROM dbo.Divisions
WHERE Tier = @Tier AND [From] <= @SeasonStartYear AND ([To] IS NULL OR [To] >= @SeasonEndYear)
";

            using(var conn = m_Context.Database.GetDbConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add(new SqlParameter("@Tier", tier));
                cmd.Parameters.Add(new SqlParameter("@SeasonStartYear", seasonStartYear));
                cmd.Parameters.Add(new SqlParameter("@SeasonEndYear", seasonEndYear));

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        leagueTable.LeagueTableRow.Add(
                            new LeagueTableRow
                            {
                                Position = (int) reader.GetInt64(0),
                                Team = reader.GetString(1),
                                Played = reader.GetInt32(2),
                                Won = reader.GetInt32(3),
                                Drawn = reader.GetInt32(4),
                                Lost = reader.GetInt32(5),
                                GoalsFor = reader.GetInt32(6),
                                GoalsAgainst = reader.GetInt32(7),
                                GoalDifference = reader.GetInt32(8),
                                Points = reader.GetInt32(9)
                            }
                        );
                    }
                    
                    reader.NextResult();

                    while (reader.Read())
                    {
                        leagueTable.Competition = reader.GetString(0);
                    }
                } 
                else 
                {
                    System.Console.WriteLine("No rows found");
                }
                reader.Close();
            }

            return leagueTable;
        }

        public LeagueFilterOptions GetLeagueFilterOptions()
        {
            var leagueFilterOptions = new LeagueFilterOptions
            {
                AllSeasons = new List<string>(),
                AllTiers = new List<Tier>()
            };

            var sql = @"
SELECT Season
FROM (
    SELECT CASE WHEN MONTH(MatchDate) >= 7 
                THEN CAST(YEAR(MatchDate) AS CHAR(4)) + ' - ' + CAST(YEAR(MatchDate) + 1 AS CHAR(4))
                ELSE CAST(YEAR(MatchDate) - 1 AS CHAR(4)) + ' - ' + CAST(YEAR(MatchDate) AS CHAR(4))
                END AS Season
    FROM dbo.Matches
) m
GROUP BY Season;

SELECT Name, Tier, [From], [To]
FROM dbo.Divisions;
";

            using(var conn = m_Context.Database.GetDbConnection())
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        leagueFilterOptions.AllSeasons.Add(
                            reader.GetString(0)
                        );
                    }

                    reader.NextResult();

                    while (reader.Read())
                    {
                        var tier = reader.GetByte(1);
                        var division = new Division
                            {
                                Name = reader.GetString(0),
                                SeasonStartYear = reader.GetInt16(2),
                                SeasonEndYear = reader.IsDBNull(3) ? DateTime.UtcNow.Year : reader.GetInt16(3)
                            };

                        AddDivision(tier, division, leagueFilterOptions);
                    }
                } 
                else 
                {
                    System.Console.WriteLine("No rows found");
                }
                reader.Close();
            }

            return leagueFilterOptions;
        }

        private void AddDivision(int tier, Division division, LeagueFilterOptions leagueFilterOptions)
        {
            var tierExists = leagueFilterOptions.AllTiers.Where(t => t.Level == tier).ToList().Count == 1;
            if (tierExists)
            {
                leagueFilterOptions.AllTiers = leagueFilterOptions.AllTiers
                    .Select(t => {
                        if (t.Level == tier) {
                            t.Divisions.Add(division);
                        }; 
                        return t;
                    }).ToList();
            }
            else 
            {
                leagueFilterOptions.AllTiers.Add(
                    new Tier
                    {
                        Level = tier,
                        Divisions = new List<Division> { division }
                    }
                );
            }
        }
    }
}