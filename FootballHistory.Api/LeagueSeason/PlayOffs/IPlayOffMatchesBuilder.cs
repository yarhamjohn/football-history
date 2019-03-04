using System.Collections.Generic;
using FootballHistory.Api.Repositories.MatchDetailRepository;

namespace FootballHistory.Api.LeagueSeason.PlayOffs
{
    public interface IPlayOffMatchesBuilder
    {
        PlayOffs Build(List<MatchDetailModel> matchDetails);
    }
}
