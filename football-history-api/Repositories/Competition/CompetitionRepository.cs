using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using football.history.api.Exceptions;

namespace football.history.api.Repositories.Competition
{
    public interface ICompetitionRepository
    {
        IEnumerable<CompetitionModel> GetAllCompetitions();
        CompetitionModel GetCompetition(long competitionId);
        CompetitionModel GetCompetitionForSeasonAndTier(long seasonId, int tier);
        CompetitionModel? GetCompetitionForSeasonAndTeam(long seasonId, long teamId);
        IEnumerable<CompetitionModel> GetCompetitionsInSeason(long seasonId);
    }

    public class CompetitionRepository : ICompetitionRepository
    {
        private readonly IDatabaseConnection _connection;
        private readonly ICompetitionCommandBuilder _queryBuilder;

        public CompetitionRepository(IDatabaseConnection connection, ICompetitionCommandBuilder queryBuilder)
        {
            _connection   = connection;
            _queryBuilder = queryBuilder;
        }

        public IEnumerable<CompetitionModel> GetAllCompetitions()
        {
            _connection.Open();
            var cmd = _queryBuilder.Build(_connection);
            var competitions = GetCompetitionModels(cmd);
            _connection.Close();
            
            return competitions;
        }

        public CompetitionModel GetCompetition(long competitionId)
        {
            _connection.Open();
            var cmd = _queryBuilder.Build(_connection, competitionId);
            var competitions = GetCompetitionModels(cmd);
            _connection.Close();

            return competitions.Count switch
            {
                1 => competitions.Single(),
                0 => throw new DataNotFoundException($"No competition matched the specified id ({competitionId})."),
                _ => throw new DataInvalidException($"{competitions.Count} competitions matched the specified id ({competitionId}).")
            };
        }

        public CompetitionModel GetCompetitionForSeasonAndTier(long seasonId, int tier)
        {
            _connection.Open();
            var cmd = _queryBuilder.Build(_connection, competitionId: null, seasonId, tier);
            var competitions = GetCompetitionModels(cmd);
            _connection.Close();

            return competitions.Count switch
            {
                1 => competitions.Single(),
                0 => throw new DataNotFoundException($"No competition matched the specified seasonId ({seasonId}) and tier ({tier})."),
                _ => throw new DataInvalidException($"{competitions.Count} competitions matched the specified seasonId ({seasonId}) and tier ({tier}).")
            };
        }

        public CompetitionModel? GetCompetitionForSeasonAndTeam(long seasonId, long teamId)
        {
            _connection.Open();
            var firstCmd = _queryBuilder.BuildForCompetitionId(_connection, seasonId, teamId);
            var competitionId = (long?) firstCmd.ExecuteScalar();
            if (competitionId is null)
            {
                _connection.Close();
                return null;
            }

            var secondCmd = _queryBuilder.Build(_connection, (long) competitionId);
            var competitions = GetCompetitionModels(secondCmd);
            _connection.Close();

            return competitions.Count switch
            {
                1 => competitions.Single(),
                0 => null,
                _ => throw new DataInvalidException($"{competitions.Count} competitions matched the specified id ({competitionId}).")
            };
        }
        
        public IEnumerable<CompetitionModel> GetCompetitionsInSeason(long seasonId)
        {
            _connection.Open();
            var cmd = _queryBuilder.Build(_connection, competitionId: null, seasonId);
            var competitions = GetCompetitionModels(cmd);
            _connection.Close();

            return competitions;
        }

        private static List<CompetitionModel> GetCompetitionModels(DbCommand cmd)
        {
            var competitions = new List<CompetitionModel>();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var competitionModel = GetCompetitionModel(reader);
                competitions.Add(competitionModel);
            }

            return competitions;
        }

        private static CompetitionModel GetCompetitionModel(DbDataReader reader)
        {
            return new(
                Id: reader.GetInt64(0),
                Name: reader.GetString(1),
                SeasonId: reader.GetInt64(2),
                StartYear: reader.GetInt16(3),
                EndYear: reader.GetInt16(4),
                Tier: reader.GetByte(5),
                Region: reader.IsDBNull(6) ? null : reader.GetString(6),
                Comment: reader.IsDBNull(7) ? null : reader.GetString(7),
                PointsForWin: reader.GetByte(8),
                TotalPlaces: reader.GetByte(9),
                PromotionPlaces: reader.GetByte(10),
                RelegationPlaces: reader.GetByte(11),
                PlayOffPlaces: reader.GetByte(12),
                RelegationPlayOffPlaces: reader.GetByte(13),
                ReElectionPlaces: reader.GetByte(14),
                FailedReElectionPosition: reader.IsDBNull(15) ? null : reader.GetByte(15)
            );
        }
    }
}
