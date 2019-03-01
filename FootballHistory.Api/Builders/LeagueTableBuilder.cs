using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Models.Controller;
using FootballHistory.Api.Repositories;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders
{
    public class LeagueTableBuilder : ILeagueTableBuilder
    {
        private readonly ILeagueTable _leagueTable;
        public LeagueTab Build(List<MatchDetailModel> leagueMatches)
        {
            var leagueTable = new LeagueTab();

            var teams = GetTeams(leagueMatches);
            foreach (var team in teams)
            {
                var matches = GetGames(leagueMatches, team);
                leagueTable.Rows.Add(
                    new LeagueTableRow
                    {
                        Team = team,
                        Played = matches.Count,
                    }
                );
            }
            
            return leagueTable;
        }

        public LeagueTableBuilder(ILeagueTable leagueTable)
        {
            _leagueTable = leagueTable;
        }

        private static List<MatchDetailModel> GetGames(List<MatchDetailModel> leagueMatches, string team)
        {
            var games = new List<MatchDetailModel>();
            var homeGames = leagueMatches.Where(m => m.HomeTeam == team).ToList();
            var awayGames = leagueMatches.Where(m => m.AwayTeam == team).ToList();
            
            games.AddRange(homeGames);
            games.AddRange(awayGames);
            
            return games;
        }

        private List<string> GetTeams(List<MatchDetailModel> leagueMatches)
        {
            var homeTeams = leagueMatches.Select(m => m.HomeTeam).ToList();
            var awayTeams = leagueMatches.Select(m => m.AwayTeam).ToList();
            
            return homeTeams.Union(awayTeams).ToList();
        }
    }
}
