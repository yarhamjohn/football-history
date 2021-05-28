using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using football.history.api.Exceptions;

namespace football.history.api.Repositories.Team
{
    public interface ITeamRepository
    {
        List<TeamModel> GetAllTeams();
        TeamModel GetTeam(long teamId);
    }

    public class TeamRepository : ITeamRepository
    {
        private readonly IDatabaseConnection _connection;
        private readonly ITeamCommandBuilder _queryBuilder;

        public TeamRepository(IDatabaseConnection connection, ITeamCommandBuilder queryBuilder)
        {
            _connection = connection;
            _queryBuilder = queryBuilder;
        }

        public List<TeamModel> GetAllTeams()
        {
            _connection.Open();
            var cmd = _queryBuilder.Build(_connection);
            var teams = GetTeamModels(cmd);
            _connection.Close();

            return teams;
        }

        public TeamModel GetTeam(long teamId)
        {
            _connection.Open();
            var cmd = _queryBuilder.Build(_connection, teamId);
            var teams = GetTeamModels(cmd);
            _connection.Close();

            return teams.Count switch
            {
                1 => teams.Single(),
                0 => throw new DataNotFoundException($"No team matched the specified id ({teamId})."),
                _ => throw new DataInvalidException($"{teams.Count} teams matched the specified id ({teamId}).")
            };
        }

        private static List<TeamModel> GetTeamModels(DbCommand cmd)
        {
            var teams = new List<TeamModel>();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                teams.Add(GetTeamModel(reader));
            }

            return teams;
        }

        private static TeamModel GetTeamModel(DbDataReader reader)
            => new(
                Id: reader.GetInt64(0),
                Name: reader.GetString(1),
                Abbreviation: reader.GetString(2),
                Notes: reader.IsDBNull(3) ? null : reader.GetString(3));
    }
}