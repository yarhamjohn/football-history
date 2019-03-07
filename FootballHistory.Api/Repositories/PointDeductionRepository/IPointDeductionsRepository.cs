using System.Collections.Generic;
using FootballHistory.Api.Controllers;

namespace FootballHistory.Api.Repositories.PointDeductionRepository
{
    public interface IPointDeductionsRepository
    {
        List<PointDeductionModel> GetPointDeductions(params SeasonTierFilter[] filter);
    }
}