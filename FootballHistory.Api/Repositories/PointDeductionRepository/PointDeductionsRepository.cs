using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using FootballHistory.Api.Controllers;
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

        public List<PointDeductionModel> GetPointDeductions(SeasonTierFilter filter)
        {
            var filters = new List<SeasonTierFilter> {filter};
            return GetPointDeductions(filters);
        }

        public List<PointDeductionModel> GetPointDeductions(List<SeasonTierFilter> filters)
        {
            using(var conn = RepositoryContext.Database.GetDbConnection())
            {
                var cmd = GetDbCommand(conn, filters);
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

        private static DbCommand GetDbCommand(DbConnection conn, List<SeasonTierFilter> filters)
        {
            conn.Open();
            
            var cmd = conn.CreateCommand();
            var fullSql = new StringBuilder();

            for (var i = 0; i < filters.Count; i++)
            {
                fullSql.Append(BuildSql(i));

                if (i < filters.Count - 1)
                {
                    fullSql.Append("\n UNION ALL \n");
                }

                cmd.Parameters.Add(new SqlParameter($"@Tier{i}", filters[i].Tier));
                cmd.Parameters.Add(new SqlParameter($"@Season{i}", filters[i].Season));
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
