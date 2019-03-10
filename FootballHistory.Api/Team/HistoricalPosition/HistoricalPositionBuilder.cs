using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Controllers;
using FootballHistory.Api.Domain;
using FootballHistory.Api.LeagueSeason.LeagueTable;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using FootballHistory.Api.Repositories.PointDeductionRepository;
using FootballHistory.Api.Repositories.TierRepository;

namespace FootballHistory.Api.Team.HistoricalPosition
{
    public class HistoricalPositionBuilder : IHistoricalPositionBuilder
    {
        private readonly ILeagueTableBuilder _leagueTableBuilder;

        public HistoricalPositionBuilder(ILeagueTableBuilder leagueTableBuilder)
        {
            _leagueTableBuilder = leagueTableBuilder;
        }

        public List<HistoricalPosition> Build(string team, SeasonTierFilter[] filters, List<MatchDetailModel> leagueMatchDetails, List<MatchDetailModel> playOffMatches, List<PointDeductionModel> pointDeductions, List<LeagueDetailModel> leagueDetails)
        {
            var historicalPositions = new List<HistoricalPosition>();
            var years = filters.OrderBy(f => f.SeasonStartYear).Select(f => f.SeasonStartYear).ToList();
            foreach (var year in years)
            {
                var season = $"{year} - {year + 1}";
                var tier = filters.Where(f => f.SeasonStartYear == year).Select(f => f.Tier).Single();
                if (tier == Tier.UnknownTier)
                {
                    historicalPositions.Add(new HistoricalPosition
                    {
                        AbsolutePosition = 0, 
                        Season = season, 
                        Status = ""
                    });
                }
                else
                {
                    var filteredLeagueMatchDetails = FilterMatches(leagueMatchDetails, year);
                    var filteredPlayOffMatches = FilterMatches(playOffMatches, year);
                    var filteredPointDeductions = FilterPointDeductions(pointDeductions, season);
                    var filteredLeagueDetail = FilterLeagueDetails(leagueDetails, season);

                    var leagueTable = _leagueTableBuilder.BuildWithStatuses(
                        filteredLeagueMatchDetails,
                        filteredPointDeductions, 
                        filteredLeagueDetail, 
                        filteredPlayOffMatches);
                    
                    var position = leagueTable.Rows.Single(r => r.Team == team).Position;
                    historicalPositions.Add(new HistoricalPosition
                    {
                        AbsolutePosition = CalculateAbsolutePosition(tier, position),
                        Season = season,
                        Status = leagueTable.Rows.Single(r => r.Team == team).Status
                    });
                }
            }

            return historicalPositions;
        }

        private static int CalculateAbsolutePosition(Tier tier, int position)
        {
            const int topTierPositions = 20;
            const int lowerTierPositions = 24;

            if (tier == Tier.TopTier)
            {
                return position;
            }

            var numLowerTiersToSkip = (int) tier - 2;
            return position + topTierPositions + lowerTierPositions * numLowerTiersToSkip;
        }

        private static LeagueDetailModel FilterLeagueDetails(List<LeagueDetailModel> leagueDetails, string season)
        {
            return leagueDetails.Single(ld => ld.Season == season);
        }

        private static List<PointDeductionModel> FilterPointDeductions(List<PointDeductionModel> pointDeductions, string season)
        {
            return pointDeductions.Where(pd => pd.Season == season).ToList();
        }

        private static List<MatchDetailModel> FilterMatches(List<MatchDetailModel> matches, int year)
        {
            return matches.Where(m =>
            {
                var seasonStart = new DateTime(year, 7, 1);
                var seasonEnd = new DateTime(year + 1, 6, 30);
                return m.Date > seasonStart && m.Date < seasonEnd;
            }).ToList();
        }
    }
}