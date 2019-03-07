using System.Collections.Generic;

namespace FootballHistory.Api.Repositories.PointDeductionRepository
{
    public interface IPointDeductionsRepository
    {
        List<PointDeductionModel> GetPointDeductions(int tier, string season);
        List<PointDeductionModel> GetPointDeductions(List<(int, string)> seasonTier);
    }
}