using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using FootballHistory.Api.Domain;
using FootballHistory.Api.Models.Controller;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Repositories
{
    public class PointDeductionsRepository : IPointDeductionsRepository
    {
        private PointDeductionsRepositoryContext RepositoryContext { get; }

        public PointDeductionsRepository(PointDeductionsRepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public List<PointDeduction>  GetPointDeductions(int tier, string season)
        {
            using(var conn = RepositoryContext.Database.GetDbConnection())
            {
                var cmd = GetDbCommand(conn, tier, season);
                return CalculatePointDeductions(cmd);
            }
        }

        private static List<PointDeduction> CalculatePointDeductions(DbCommand cmd)
        {
            var pointDeductions = new List<PointDeduction>();
            
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    pointDeductions.Add(
                        new PointDeduction
                        {
                            Competition = reader.GetString(0),
                            Team = reader.GetString(1),
                            PointsDeducted = reader.GetByte(2),
                            Reason = reader.GetString(3)
                        }
                    );
                }
            }

            return pointDeductions;
        }

        private static DbCommand GetDbCommand(DbConnection conn, int tier, string season)
        {
            const string sql = @"
SELECT d.Name AS Competition
    ,c.Name AS TeamName
    ,pd.PointsDeducted
    ,pd.Reason
FROM dbo.PointDeductions AS pd
INNER JOIN dbo.Divisions d ON d.Id = pd.DivisionId
INNER JOIN dbo.Clubs AS c ON c.Id = pd.ClubId
WHERE d.Tier = @Tier AND pd.Season = @Season
";

            conn.Open();
            
            var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@Tier", tier));
            cmd.Parameters.Add(new SqlParameter("@Season", season));
            
            return cmd;
        }
    }
}
