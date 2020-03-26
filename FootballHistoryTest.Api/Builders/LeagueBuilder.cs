using System;
using System.Collections.Generic;
using FootballHistoryTest.Api.Calculators;
using FootballHistoryTest.Api.Domain;
using FootballHistoryTest.Api.Repositories.League;
using FootballHistoryTest.Api.Repositories.Match;
using FootballHistoryTest.Api.Repositories.PointDeductions;
using Microsoft.EntityFrameworkCore;

namespace FootballHistoryTest.Api.Builders
{
    public interface ILeagueBuilder
    {
        League GetLeague(int seasonStartYear, int tier);
        League GetLeagueOnDate(int tier, DateTime date);
    }
    
    public class LeagueBuilder : ILeagueBuilder
    {
        private readonly DatabaseContext _context;
        private readonly ILeagueRepository _leagueRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IPointsDeductionRepository _pointDeductionsRepository;

        public LeagueBuilder(DatabaseContext context, ILeagueRepository leagueRepository, IMatchRepository matchRepository, IPointsDeductionRepository pointDeductionsRepository)
        {
            _context = context;
            _leagueRepository = leagueRepository;
            _matchRepository = matchRepository;
            _pointDeductionsRepository = pointDeductionsRepository;
        }

        public League GetLeague(int seasonStartYear, int tier)
        {
            using var conn = _context.Database.GetDbConnection();

            var playOffMatches = _matchRepository.GetPlayOffMatchModels(conn, seasonStartYear, tier);
            var leagueMatches = _matchRepository.GetLeagueMatchModels(conn, seasonStartYear, tier);
            var pointsDeductions = _pointDeductionsRepository.GetPointsDeductionModels(conn, seasonStartYear, tier);
            var leagueModel = _leagueRepository.GetLeagueModel(conn, seasonStartYear, tier);
            var leagueTable = LeagueTableCalculator.GetFullLeagueTable(leagueMatches, playOffMatches, leagueModel, pointsDeductions);

            return new League
            {
                Name = leagueModel.Name,
                Tier = leagueModel.Tier,
                TotalPlaces = leagueModel.TotalPlaces,
                PromotionPlaces = leagueModel.PromotionPlaces,
                RelegationPlaces = leagueModel.RelegationPlaces,
                PlayOffPlaces = leagueModel.PlayOffPlaces,
                PointsForWin = leagueModel.PointsForWin,
                StartYear = leagueModel.StartYear,
                Table = leagueTable
            };
        }

        public League GetLeagueOnDate(int tier, DateTime date)
        {
            using var conn = _context.Database.GetDbConnection();

            var seasonStartYear = date.Month > 6 ? date.Year : date.Year - 1;
            
            var leagueMatches = _matchRepository.GetLeagueMatchModels(conn, seasonStartYear, tier);
            var pointsDeductions = _pointDeductionsRepository.GetPointsDeductionModels(conn, seasonStartYear, tier);
            var leagueModel = _leagueRepository.GetLeagueModel(conn, seasonStartYear, tier);
            var leagueTable = LeagueTableCalculator.GetPartialLeagueTable(leagueMatches, leagueModel, pointsDeductions, date);

            return new League
            {
                Name = leagueModel.Name,
                Tier = leagueModel.Tier,
                TotalPlaces = leagueModel.TotalPlaces,
                PromotionPlaces = leagueModel.PromotionPlaces,
                RelegationPlaces = leagueModel.RelegationPlaces,
                PlayOffPlaces = leagueModel.PlayOffPlaces,
                PointsForWin = leagueModel.PointsForWin,
                StartYear = leagueModel.StartYear,
                Table = leagueTable
            };
        }
    }

    public class League
    {
        public string Name { get; set; }
        public int Tier { get; set; }
        public int TotalPlaces { get; set; }
        public int PromotionPlaces { get; set; }
        public int PlayOffPlaces { get; set; }
        public int RelegationPlaces { get; set; }
        public int PointsForWin { get; set; }
        public int StartYear { get; set; }
        public List<LeagueTableRow> Table { get; set; }
    }

    public class LeagueTableRow
    {
        public int Position { get; set; }
        public string Team { get; set; }
        public int Played { get; set; }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference { get; set; }
        public int Points { get; set; }
        public int PointsDeducted { get; set; }
        public string PointsDeductionReason { get; set; }
        public LeagueStatus Status { get; set; }
    }
}