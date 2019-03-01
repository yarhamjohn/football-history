using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders
{
    public class LeagueTableBuilder : ILeagueTableBuilder
    {
        public LeagueTab Build(List<MatchDetailModel> leagueMatches)
        {
            var leagueTable = new LeagueTab();

            var teams = GetTeams(leagueMatches);
            foreach (var team in teams)
            {
                var matches = new TeamLeagueMatches(leagueMatches, team);
                if (matches.AreInvalid())
                {
                    throw new Exception("An invalid set of league matches were provided.");
                }

                leagueTable.Rows.Add(
                    new LeagueTableRow
                    {
                        Team = team,
                        Played = matches.CountGamesPlayed(),
                        Won = matches.CountWins(),
                        Lost = matches.CountDefeats(),
                        Drawn = matches.CountDraws(),
                        GoalsFor = matches.CountGoalsFor(),
                        GoalsAgainst = matches.CountGoalsAgainst(),
                        GoalDifference = matches.CalculateGoalDifference(),
                        Points = matches.CalculatePoints()
                    }
                );
            }

            return leagueTable;
        }

        private List<string> GetTeams(List<MatchDetailModel> leagueMatches)
        {
            var homeTeams = leagueMatches.Select(m => m.HomeTeam).ToList();
            var awayTeams = leagueMatches.Select(m => m.AwayTeam).ToList();

            return homeTeams.Union(awayTeams).ToList();
        }
    }
}
