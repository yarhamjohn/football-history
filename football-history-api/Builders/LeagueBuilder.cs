using System;
using football.history.api.Calculators;
using football.history.api.Repositories.League;
using football.history.api.Repositories.Tier;

namespace football.history.api.Builders
{
    public interface ILeagueBuilder
    {
        LeagueDto Build(int seasonStartYear, int tier);
        LeagueDto Build(int seasonStartYear, string team);
        LeagueDto Build(DateTime date, int tier);
    }

    public class LeagueBuilder : ILeagueBuilder
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly ILeagueTableBuilder _leagueTableBuilder;
        private readonly ITierRepository _tierRepository;
        private readonly IDateCalculator _dateCalculator;

        public LeagueBuilder(
            ILeagueRepository leagueRepository,
            ILeagueTableBuilder leagueTableBuilder,
            ITierRepository tierRepository,
            IDateCalculator dateCalculator)
        {
            _leagueRepository = leagueRepository;
            _leagueTableBuilder = leagueTableBuilder;
            _tierRepository = tierRepository;
            _dateCalculator = dateCalculator;
        }

        public LeagueDto Build(int seasonStartYear, int tier)
        {
            var seasonEndDate = _dateCalculator.GetSeasonEndDate(seasonStartYear);
            return GetLeagueDto(tier, seasonStartYear, seasonEndDate);
        }

        public LeagueDto Build(int seasonStartYear, string team)
        {
            var tier = _tierRepository.GetTierForTeamInYear(seasonStartYear, team);
            if (tier == null)
            {
                // No league data is available for the team specified
                return new LeagueDto();
            }

            var seasonEndDate = _dateCalculator.GetSeasonEndDate(seasonStartYear);
            return GetLeagueDto((int) tier, seasonStartYear, seasonEndDate);
        }

        public LeagueDto Build(DateTime date, int tier)
        {
            var seasonStartYear = _dateCalculator.GetSeasonStartYear(date);
            return GetLeagueDto(tier, seasonStartYear, date);
        }

        private LeagueDto GetLeagueDto(int tier, int seasonStartYear, DateTime date)
        {
            // TODO: handle not finding a matching league model etc
            var leagueModel = _leagueRepository.GetLeagueModel(seasonStartYear, tier);
            var leagueTable = _leagueTableBuilder.Build(seasonStartYear, leagueModel, date);

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
