using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Controllers;
using FootballHistory.Api.Domain;
using FootballHistory.Api.LeagueSeason.Table;
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
            var years = filters.OrderBy(f => f.SeasonStartYear).Select(f => f.SeasonStartYear).Distinct().ToList();
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
                    if (leagueMatchDetails.Count == 0 || leagueDetails.Count == 0)
                    {
                        throw new Exception($"No league matches or no league detail was found for the season ({season}).");
                    }
                    var filteredLeagueMatchDetails = FilterMatches(leagueMatchDetails, year);
                    var filteredPlayOffMatches = FilterMatches(playOffMatches, year);
                    var filteredPointDeductions = FilterPointDeductions(pointDeductions, season);
                    var filteredLeagueDetail = FilterLeagueDetails(leagueDetails, season);

                    var leagueTable = _leagueTableBuilder.BuildWithStatuses(
                        filteredLeagueMatchDetails,
                        filteredPointDeductions, 
                        filteredLeagueDetail, 
                        filteredPlayOffMatches);
                    
                    var position = leagueTable.GetPosition(team);

                    historicalPositions.Add(new HistoricalPosition
                    {
                        AbsolutePosition = CalculateAbsolutePosition(tier, year, position),
                        Season = season,
                        Status = leagueTable.Rows.Single(r => r.Team == team).Status
                    });
                }
            }

            return historicalPositions;
        }

        private static int CalculateAbsolutePosition(Tier tier, int seasonStartYear, int position)
        {
            const int topTierPositionsFrom1995 = 20;
            const int topTierPositionsBefore1995 = 22;
            const int secondTierPositions = 24;
            const int thirdTierPositions = 24;
            
            var topTierPositions = seasonStartYear >= 1995 ? topTierPositionsFrom1995 : topTierPositionsBefore1995;
            switch (tier)
            {
                case Tier.TopTier:
                    return position;
                case Tier.SecondTier:
                    return position + topTierPositions;
                case Tier.ThirdTier:
                    return position + topTierPositions + secondTierPositions;
                case Tier.FourthTier:
                    return position + topTierPositions + secondTierPositions + thirdTierPositions;
                default:
                    return 0;
            }
        }

        private static LeagueDetailModel FilterLeagueDetails(List<LeagueDetailModel> leagueDetails, string season)
        {
            var details = leagueDetails.Where(ld => ld.Season == season).ToList();
            if (details.Count != 1)
            {
                throw new Exception($"The incorrect number of league details ({details.Count}) were found for the given season.");
            }

            return details.Single();
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