using System;
using football.history.api.Calculators;
using football.history.api.Repositories.League;
using football.history.api.Repositories.Tier;

namespace football.history.api.Builders
{
    public interface ILeagueBuilder
    {
        LeagueDto BuildForTier(int seasonStartYear, int tier);
        LeagueDto BuildForTeam(int seasonStartYear, string team);
        LeagueDto BuildForTier(DateTime date, int tier);
        LeagueDto BuildForTeam(DateTime date, string team);
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

        public LeagueDto BuildForTier(int seasonStartYear, int tier)
        {
            var seasonEndDate = _dateCalculator.GetSeasonEndDate(seasonStartYear);
            return BuildForTier(seasonEndDate, tier);
        }

        public LeagueDto BuildForTeam(int seasonStartYear, string team)
        {
            var seasonEndDate = _dateCalculator.GetSeasonEndDate(seasonStartYear);
            return BuildForTeam(seasonEndDate, team);
        }

        public LeagueDto BuildForTier(DateTime date, int tier)
        {
            return GetLeagueDto(date, tier);
        }

        public LeagueDto BuildForTeam(DateTime date, string team)
        {
            var seasonStartYear = _dateCalculator.GetSeasonStartYear(date);
            var tier = _tierRepository.GetTierForTeamInYear(seasonStartYear, team);
            if (tier == null)
            {
                // No league data is available for the team specified
                return new LeagueDto();
            }

            return GetLeagueDto(date, (int) tier);
        }

        private LeagueDto GetLeagueDto(DateTime date, int tier)
        {
            // TODO: handle not finding a matching league model
            var leagueModel = _leagueRepository.GetLeagueModel(date, tier);
            var leagueTable = _leagueTableBuilder.Build(leagueModel, date);

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
