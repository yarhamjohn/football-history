using System.Collections.Generic;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Repositories.Models;

namespace FootballHistory.Api.Builders
{
    public interface IPlayOffMatchesBuilder
    {
        PlayOffs Build(List<MatchDetailModel> matchDetails);
    }
}
