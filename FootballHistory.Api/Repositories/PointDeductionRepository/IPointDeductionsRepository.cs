using System.Collections.Generic;
using FootballHistory.Api.Controllers;
using FootballHistory.Api.Repositories.TierRepository;

namespace FootballHistory.Api.Repositories.PointDeductionRepository
{
    public interface IPointDeductionsRepository
    {
        List<PointDeductionModel> GetPointDeductions(params SeasonTierFilter[] filter);
    }
}