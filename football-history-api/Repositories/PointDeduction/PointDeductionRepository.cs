using System.Collections.Generic;
using System.Data.Common;

namespace football.history.api.Repositories.PointDeduction
{
    public interface IPointDeductionRepository
    {
        List<PointDeductionModel> GetPointDeductions(long competitionId);
    }

    public class PointDeductionRepository : IPointDeductionRepository
    {
        private readonly IDatabaseConnection _connection;
        private readonly IPointDeductionCommandBuilder _queryBuilder;

        public PointDeductionRepository(IDatabaseConnection connection, IPointDeductionCommandBuilder queryBuilder)
        {
            _connection   = connection;
            _queryBuilder = queryBuilder;
        }

        public List<PointDeductionModel> GetPointDeductions(long competitionId)
        {
            _connection.Open();
            var cmd = _queryBuilder.Build(_connection, competitionId);
            var pointsDeductions = GetPointDeductionModels(cmd);
            _connection.Close();

            return pointsDeductions;
        }

        private static List<PointDeductionModel> GetPointDeductionModels(DbCommand cmd)
        {
            var pointDeductions = new List<PointDeductionModel>();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var pointDeductionModel = GetPointDeductionModel(reader);
                pointDeductions.Add(pointDeductionModel);
            }

            return pointDeductions;
        }

        private static PointDeductionModel GetPointDeductionModel(DbDataReader reader)
        {
            return new(
                Id: reader.GetInt64(0),
                CompetitionId: reader.GetInt64(1),
                PointsDeducted: reader.GetInt16(2),
                TeamId: reader.GetInt64(3),
                TeamName: reader.GetString(4),
                Reason: reader.GetString(5));
        }
    }
}
