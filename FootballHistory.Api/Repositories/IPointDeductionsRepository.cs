using System.Collections.Generic;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Repositories
{
    public interface IPointDeductionsRepository
    {
        List<PointDeductionModel> GetPointDeductions(int tier, string season);
    }
}