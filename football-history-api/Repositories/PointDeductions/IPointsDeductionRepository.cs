using System.Collections.Generic;

namespace football.history.api.Repositories.PointDeductions
{
    public interface IPointsDeductionRepository
    {
        List<PointsDeductionModel> GetPointsDeductionModels(int seasonStartYear, int tier);
        List<PointsDeductionModel> GetPointsDeductionModels(List<int> seasonStartYears, List<int> tiers);
    }
}
