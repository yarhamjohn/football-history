using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Calculators;
using FootballHistoryTest.Api.Repositories.League;
using FootballHistoryTest.Api.Repositories.Match;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistoryTest.Api.Controllers
{
    [Route("api/[controller]")]
    public class LeagueController : Controller
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly IMatchRepository _matchRepository;

        public LeagueController(ILeagueRepository leagueRepository, IMatchRepository matchRepository)
        {
            _leagueRepository = leagueRepository;
            _matchRepository = matchRepository;
        }

        [HttpGet("[action]")]
        public League GetLeague(int seasonStartYear, int tier)
        {
            var leagueModel = _leagueRepository.GetLeagueModel(seasonStartYear, tier);
            return new League
            {
                Name = leagueModel.Name,
                Tier = leagueModel.Tier,
                TotalPlaces = leagueModel.TotalPlaces,
                PromotionPlaces = leagueModel.PromotionPlaces,
                RelegationPlaces = leagueModel.RelegationPlaces,
                PlayOffPlaces = leagueModel.PlayOffPlaces,
                PointsForWin = leagueModel.PointsForWin
            };
        }

        [HttpGet("[action]")]
        public List<League> GetLeaguesInSeason(int seasonStartYear)
        {
            // do it by tier...
            var playOffMatches = _matchRepository.GetPlayOffMatchModels(new List<int> {seasonStartYear}, new List<int>());
            var leagueMatches = _matchRepository.GetLeagueMatchModels(new List<int> { seasonStartYear }, new List<int>(), new List<string>());
            var leagueTable = LeagueTableCalculator.GetLeagueTable(leagueMatches, playOffMatches);

            var result = _leagueRepository.GetLeagueModels(seasonStartYear)
                .Select(d => new League
                {
                    Name = d.Name,
                    Tier = d.Tier,
                    TotalPlaces = d.TotalPlaces,
                    PromotionPlaces = d.PromotionPlaces,
                    RelegationPlaces = d.RelegationPlaces,
                    PlayOffPlaces = d.PlayOffPlaces,
                    PointsForWin = d.PointsForWin,
                    Table = leagueTable
                })
                .ToList();

            return result;
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
        public string Status { get; set; }
    }
}