using System.Collections.Generic;
using FootballHistory.Api.Controllers;

namespace FootballHistory.Api.Repositories.PointDeductionRepository
{
    public interface IPointDeductionsRepository
    {
        List<PointDeductionModel> GetPointDeductions(SeasonTierFilter filter);
        List<PointDeductionModel> GetPointDeductions(List<SeasonTierFilter> filters);
    }
}