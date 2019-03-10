using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Controllers;
using FootballHistory.Api.LeagueSeason.LeagueTable;
using FootballHistory.Api.Repositories.LeagueDetailRepository;
using FootballHistory.Api.Repositories.MatchDetailRepository;
using FootballHistory.Api.Repositories.PointDeductionRepository;

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
            var years = filters.OrderBy(f => f.Season).Select(f => Convert.ToInt32(f.Season.Substring(0, 4))).ToList();
            foreach (var year in years)
            {
                var season = $"{year} - {year + 1}";
                if (filters.Where(f => f.Season == season).Select(f => f.Tier).Single() == 0)
                {
                    historicalPositions.Add(new HistoricalPosition
                        {AbsolutePosition = 0, Season = season, Status = ""});
                }
                else
                {
                    var filteredLeagueMatchDetails = leagueMatchDetails.Where(m =>
                        m.Date > new DateTime(year, 7, 1) && m.Date < new DateTime(year + 1, 6, 30)).ToList();
                    var filteredPlayOffMatches = playOffMatches.Where(m =>
                        m.Date > new DateTime(year, 7, 1) && m.Date < new DateTime(year + 1, 6, 30)).ToList();
                    var filteredPointDeductions = pointDeductions.Where(pd => pd.Season == season).ToList();
                    var filteredLeagueDetail = leagueDetails.Single(ld => ld.Season == season);

                    var leagueTable = _leagueTableBuilder.BuildWithStatuses(filteredLeagueMatchDetails,
                        filteredPointDeductions, filteredLeagueDetail, filteredPlayOffMatches);
                    var tier = filters.Where(st => st.Season == season).Select(st => st.Tier).Single();
                    var position = leagueTable.Rows.Single(r => r.Team == team).Position;
                    historicalPositions.Add(new HistoricalPosition
                    {
                        AbsolutePosition = tier == 1
                            ? position
                            : position + 20 + ((tier - 2) * 24),
                        Season = season,
                        Status = leagueTable.Rows.Single(r => r.Team == team).Status
                    });
                }
            }

            return historicalPositions;
        }
    }
}