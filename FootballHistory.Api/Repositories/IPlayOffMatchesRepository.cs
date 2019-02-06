using System.Collections.Generic;
using FootballHistory.Api.Models.Controller;

namespace FootballHistory.Api.Repositories
{
    public interface IPlayOffMatchesRepository
    {
        PlayOffs GetPlayOffMatches(int tier, string season);
    }
}
