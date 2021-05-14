using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using football.history.api.Exceptions;
using football.history.api.Repositories.Team;

namespace football.history.api.Repositories.Season
{
    public interface ISeasonRepository
    {
        IEnumerable<SeasonModel> GetAllSeasons();
        SeasonModel GetSeason(long seasonId);
    }

    public class SeasonRepository : ISeasonRepository
    {
        private readonly IDatabaseConnection _connection;
        private readonly ISeasonCommandBuilder _queryBuilder;

        public SeasonRepository(IDatabaseConnection connection, ISeasonCommandBuilder queryBuilder)
        {
            _connection = connection;
            _queryBuilder = queryBuilder;
        }

        public IEnumerable<SeasonModel> GetAllSeasons()
        {
            _connection.Open();
            var cmd = _queryBuilder.Build(_connection);
            var seasons = GetSeasonModels(cmd);
            _connection.Close();

            return seasons;
        }

        public SeasonModel GetSeason(long seasonId)
        {
            _connection.Open();
            var cmd = _queryBuilder.Build(_connection, seasonId);
            var seasons = GetSeasonModels(cmd);
            _connection.Close();

            return seasons.Count switch
            {
                1 => seasons.Single(),
                0 => throw new DataNotFoundException($"No season matched the specified id ({seasonId})."),
                _ => throw new DataInvalidException($"{seasons.Count} seasons matched the specified id ({seasonId}).")
            };
        }

        private static List<SeasonModel> GetSeasonModels(DbCommand cmd)
        {
            var seasons = new List<SeasonModel>();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var seasonModel = GetSeasonModel(reader);
                seasons.Add(seasonModel);
            }

            return seasons;
        }

        private static SeasonModel GetSeasonModel(DbDataReader reader)
            => new(
                Id: reader.GetInt64(0),
                StartYear: reader.GetInt16(1),
                EndYear: reader.GetInt16(2));
    }
}