using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace football.history.api.Repositories.Season
{
    public interface ISeasonCommandBuilder
    {
        public DbCommand Build(IDatabaseConnection connection);
        public DbCommand Build(IDatabaseConnection connection, long seasonId);
    }

    public class SeasonCommandBuilder : ISeasonCommandBuilder
    {
        public DbCommand Build(IDatabaseConnection connection)
        {
            const string sql = @"
SELECT s.Id, s.StartYear, s.EndYear
FROM [dbo].[Seasons] AS s
";

            return BuildCommand(connection, sql);
        }

        public DbCommand Build(IDatabaseConnection connection, long seasonId)
        {
            const string sql = @"
SELECT s.Id, s.StartYear, s.EndYear
FROM [dbo].[Seasons] AS s
WHERE s.Id = @Id
";

            var cmd = BuildCommand(connection, sql);
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@Id",
                    Value = seasonId
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
