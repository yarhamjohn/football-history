using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Repositories.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace FootballHistory.Api.Builders
{
    public class LeagueTableBuilder : ILeagueTableBuilder
    {
        public LeagueTab Build(List<MatchDetailModel> leagueMatches, List<PointDeductionModel> pointDeductions)
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

                var pointsDeducted = pointDeductions.Where(d => d.Team == team).Sum(d => d.PointsDeducted);
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
                        Points = matches.CalculatePoints() - pointsDeducted,
                        PointsDeducted = pointsDeducted,
                        PointsDeductionReason = string.Join(", ", pointDeductions.Where(d=> d.Team == team).Select(d => d.Reason))
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
