using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using FootballHistory.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace FootballHistory.Api.Repositories.PointDeductionRepository
{
    public class PointDeductionsRepository : IPointDeductionsRepository
    {
        private PointDeductionsRepositoryContext RepositoryContext { get; }

        public PointDeductionsRepository(PointDeductionsRepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public List<PointDeductionModel> GetPointDeductions(int tier, string season)
        {
            var seasonTier = new List<(int, string)> {(tier, season)};
            return GetPointDeductions(seasonTier);
        }

        public List<PointDeductionModel> GetPointDeductions(List<(int, string)> seasonTier)
        {
            using(var conn = RepositoryContext.Database.GetDbConnection())
            {
                var cmd = GetDbCommand(conn, seasonTier);
                return CalculatePointDeductions(cmd);
            }
        }

        private static List<PointDeductionModel> CalculatePointDeductions(DbCommand cmd)
        {
            var pointDeductions = new List<PointDeductionModel>();
            
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    pointDeductions.Add(
                        new PointDeductionModel
                        {
                            Competition = reader.GetString(0),
                            Team = reader.GetString(1),
                            PointsDeducted = reader.GetByte(2),
                            Reason = reader.GetString(3),
                            Season = reader.GetString(4)
                        }
                    );
                }
            }

            return pointDeductions;
        }

        private static DbCommand GetDbCommand(DbConnection conn, List<(int, string)> seasonTier)
        {
            conn.Open();
            
            var cmd = conn.CreateCommand();
            var fullSql = new StringBuilder();

            for (var i = 0; i < seasonTier.Count; i++)
            {
                fullSql.Append(BuildSql(i));

                if (i < seasonTier.Count - 1)
                {
                    fullSql.Append("\n UNION ALL \n");
                }

                cmd.Parameters.Add(new SqlParameter($"@Tier{i}", seasonTier.Single().Item1));
                cmd.Parameters.Add(new SqlParameter($"@Season{i}", seasonTier.Single().Item2));
            }

            cmd.CommandText = fullSql.ToString();
            return cmd;
        }

        private static string BuildSql(int num)
        {
            return $@"
SELECT d.Name AS Competition
    ,c.Name AS TeamName
    ,pd.PointsDeducted
    ,pd.Reason
    ,pd.Season
FROM dbo.PointDeductions AS pd
INNER JOIN dbo.Divisions d ON d.Id = pd.DivisionId
INNER JOIN dbo.Clubs AS c ON c.Id = pd.ClubId
WHERE d.Tier = @Tier{num} AND pd.Season = @Season{num}
";
        }
    }
}
