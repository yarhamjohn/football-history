using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Repositories.League;
using Microsoft.AspNetCore.Mvc;

namespace FootballHistoryTest.Api.Controllers
{
    [Route("api/[controller]")]
    public class LeagueController : Controller
    {
        private readonly ILeagueRepository _leagueRepository;

        public LeagueController(ILeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
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
            var result = _leagueRepository.GetLeagueModels(seasonStartYear)
                .Select(d => new League
                {
                    Name = d.Name,
                    Tier = d.Tier,
                    TotalPlaces = d.TotalPlaces,
                    PromotionPlaces = d.PromotionPlaces,
                    RelegationPlaces = d.RelegationPlaces,
                    PlayOffPlaces = d.PlayOffPlaces,
                    PointsForWin = d.PointsForWin
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
    }
}