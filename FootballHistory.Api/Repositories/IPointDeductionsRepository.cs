using System.Collections.Generic;
using FootballHistory.Api.Models.Controller;

namespace FootballHistory.Api.Repositories
{
    public interface IPointDeductionsRepository
    {
        List<PointDeduction> GetPointDeductions(int tier, string season);
    }
}