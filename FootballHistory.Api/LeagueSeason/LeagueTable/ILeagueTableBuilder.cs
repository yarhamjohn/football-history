using System.Collections.Generic;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using FootballHistory.Api.Repositories.PointDeductionRepository;

namespace FootballHistory.Api.LeagueSeason.LeagueTable
{
    public interface ILeagueTableBuilder
    {
        LeagueTable Build(List<MatchDetailModel> leagueMatchDetails, List<PointDeductionModel> pointDeductions, LeagueDetailModel leagueDetailModel, List<MatchDetailModel> playOffMatches);
    }
}
