using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace football.history.api.Repositories.PointDeduction
{
    public interface IPointDeductionCommandBuilder
    {
        public DbCommand Build(IDatabaseConnection connection, long competitionId);
    }

    public class PointDeductionCommandBuilder : IPointDeductionCommandBuilder
    {
        public DbCommand Build(IDatabaseConnection connection, long competitionId)
        {
            const string sql = @"
SELECT d.Id
      ,d.CompetitionId
      ,d.PointsDeducted
      ,d.TeamId
      ,t.Name
      ,d.Reason
FROM [dbo].[Deductions] AS d
LEFT JOIN [dbo].[Teams] AS t ON t.Id = d.TeamId
WHERE d.CompetitionId = @CompetitionId
";

            var cmd = BuildCommand(connection, sql);
            cmd.Parameters.Add(
                new SqlParameter
                {
                    ParameterName = "@CompetitionId",
                    Value         = competitionId
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
