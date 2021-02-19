using System;
using football.history.api.Calculators;
using football.history.api.Repositories.League;
using football.history.api.Repositories.Tier;

namespace football.history.api.Builders
{
    public interface ILeagueBuilder
    {
        LeagueDto GetCompletedLeague(int tier, int seasonStartYear);
        LeagueDto GetCompletedLeagueForTeam(string team, int seasonStartYear);
        LeagueDto GetLeagueOnDate(int tier, DateTime date);
    }

    public class LeagueBuilder : ILeagueBuilder
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly ILeagueTableBuilder _leagueTableBuilder;
        private readonly ITierRepository _tierRepository;

        public LeagueBuilder(
            ILeagueRepository leagueRepository,
            ILeagueTableBuilder leagueTableBuilder,
            ITierRepository tierRepository)
        {
            _leagueRepository = leagueRepository;
            _leagueTableBuilder = leagueTableBuilder;
            _tierRepository = tierRepository;
        }

        public LeagueDto GetCompletedLeague(int tier, int seasonStartYear)
        {
            var seasonEndDate = DateCalculator.GetSeasonEndDate(seasonStartYear);
            return GetLeagueDto(tier, seasonStartYear, seasonEndDate);
        }

        public LeagueDto GetCompletedLeagueForTeam(string team, int seasonStartYear)
        {
            var tier = _tierRepository.GetTierForTeamInYear(seasonStartYear, team);
            if (tier == null)
            {
                // No league data is available for the team specified
                return new LeagueDto();
            }

            var seasonEndDate = DateCalculator.GetSeasonEndDate(seasonStartYear);
            return GetLeagueDto((int) tier, seasonStartYear, seasonEndDate);
        }

        public LeagueDto GetLeagueOnDate(int tier, DateTime date)
        {
            var seasonStartYear = DateCalculator.GetSeasonStartYear(date);
            return GetLeagueDto(tier, seasonStartYear, date);
        }

        private LeagueDto GetLeagueDto(int tier, int seasonStartYear, DateTime date)
        {
            var leagueModel = _leagueRepository.GetLeagueModel(seasonStartYear, tier);
            var leagueTable = _leagueTableBuilder.Build(tier, seasonStartYear, leagueModel, date);

            return new LeagueDto
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
}
