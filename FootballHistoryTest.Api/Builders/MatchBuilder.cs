using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Domain;
using FootballHistoryTest.Api.Repositories.Match;
using Microsoft.EntityFrameworkCore;

namespace FootballHistoryTest.Api.Builders
{
    public interface IMatchBuilder
    {
        List<Match> GetLeagueMatches(List<int> seasonStartYears, List<int> tiers, List<string> teams);

        List<Match> GetHeadToHeadLeagueMatches(List<int> seasonStartYears, List<int> tiers, string teamOne,
            string teamTwo);

        List<KnockoutMatch> GetPlayOffMatches(List<int> seasonStartYears, List<int> tiers);
    }
    
    public class MatchBuilder : IMatchBuilder
    {
        private readonly DatabaseContext _context;
        private readonly IMatchRepository _matchRepository;

        public MatchBuilder(DatabaseContext context, IMatchRepository matchRepository)
        {
            _context = context;
            _matchRepository = matchRepository;
        }
    
        public List<Match> GetLeagueMatches(List<int> seasonStartYears, List<int> tiers, List<string> teams)
        {
            using var conn = _context.Database.GetDbConnection();

            var matchModels = _matchRepository.GetLeagueMatchModels(conn, seasonStartYears, tiers, teams);
            return GetMatches(matchModels);
        }
    
        public List<Match> GetHeadToHeadLeagueMatches(List<int> seasonStartYears, List<int> tiers, string teamOne, string teamTwo)
        {
            using var conn = _context.Database.GetDbConnection();

            var matchModels = _matchRepository.GetLeagueHeadToHeadMatchModels(conn, seasonStartYears, tiers, teamOne, teamTwo);
            return GetMatches(matchModels);
        }
    
        public List<KnockoutMatch> GetPlayOffMatches(List<int> seasonStartYears, List<int> tiers)
        {
            using var conn = _context.Database.GetDbConnection();

            var matchModels = _matchRepository.GetPlayOffMatchModels(conn, seasonStartYears, tiers);
            return matchModels
                .Select(m => new KnockoutMatch
                {
                    Tier = m.Tier,
                    Division = m.Division,
                    Date = m.Date,
                    HomeTeam = m.HomeTeam,
                    HomeTeamAbbreviation = m.HomeTeamAbbreviation,
                    AwayTeam = m.AwayTeam,
                    AwayTeamAbbreviation = m.AwayTeamAbbreviation,
                    HomeGoals = m.HomeGoals,
                    AwayGoals = m.AwayGoals,
                    Round = m.Round,
                    ExtraTime = m.ExtraTime,
                    HomeGoalsExtraTime = m.HomeGoalsExtraTime,
                    AwayGoalsExtraTime = m.AwayGoalsExtraTime,
                    PenaltyShootout = m.PenaltyShootout,
                    HomePenaltiesTaken = m.HomePenaltiesTaken,
                    HomePenaltiesScored = m.HomePenaltiesScored,
                    AwayPenaltiesTaken = m.AwayPenaltiesTaken,
                    AwayPenaltiesScored = m.AwayPenaltiesScored

                })
                .ToList();
        }

        private List<Match> GetMatches(List<MatchModel> matchModels)
        {
            return matchModels
                .Select(m => new Match
                {
                    Tier = m.Tier,
                    Division = m.Division,
                    Date = m.Date, 
                    HomeTeam = m.HomeTeam,
                    HomeTeamAbbreviation = m.HomeTeamAbbreviation,
                    AwayTeam = m.AwayTeam,
                    AwayTeamAbbreviation = m.AwayTeamAbbreviation,
                    HomeGoals = m.HomeGoals,
                    AwayGoals = m.AwayGoals
                    
                })
                .ToList();
        }
    }

    public class Match
    {
        public int Tier { get; set; }
        public string Division { get; set; }
        public DateTime Date { get; set; }
        public string HomeTeam { get; set; }
        public string HomeTeamAbbreviation { get; set; }
        public string AwayTeam { get; set; }
        public string AwayTeamAbbreviation { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
    }

    public class KnockoutMatch : Match
    {
        public string Round { get; set; }
        public bool ExtraTime { get; set; }
        public int? HomeGoalsExtraTime { get; set; }
        public int? AwayGoalsExtraTime { get; set; }
        public bool PenaltyShootout { get; set; }
        public int? HomePenaltiesTaken { get; set; }
        public int? HomePenaltiesScored { get; set; }
        public int? AwayPenaltiesTaken { get; set; }
        public int? AwayPenaltiesScored { get; set; }
    }
}