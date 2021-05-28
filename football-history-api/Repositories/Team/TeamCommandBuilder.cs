using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace football.history.api.Repositories.Team
{
    public interface ITeamCommandBuilder
    {
        public DbCommand Build(IDatabaseConnection connection);
        public DbCommand Build(IDatabaseConnection connection, long teamId);
    }

    public class TeamCommandBuilder : ITeamCommandBuilder
    {
        public DbCommand Build(IDatabaseConnection connection)
        {
            const string sql = @"
SELECT t.Id, t.Name, t.Abbreviation, t.Notes
FROM [dbo].[Teams] AS t
";

            return BuildCommand(connection, sql);
        }

        public DbCommand Build(IDatabaseConnection connection, long teamId)
        {
            const string sql = @"
SELECT t.Id, t.Name, t.Abbreviation, t.Notes
FROM [dbo].[Teams] AS t
WHERE t.Id = @Id
";

            var cmd = BuildCommand(connection, sql);
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@Id",
                    Value         = teamId
                });

            return cmd;
        }
        
        private static DbCommand BuildCommand(IDatabaseConnection connection, string sql)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            return cmd;
        }
    }
}
