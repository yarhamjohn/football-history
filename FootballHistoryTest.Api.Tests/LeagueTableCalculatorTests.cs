using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistoryTest.Api.Builders;
using FootballHistoryTest.Api.Calculators;
using FootballHistoryTest.Api.Repositories.League;
using FootballHistoryTest.Api.Repositories.Match;
using FootballHistoryTest.Api.Repositories.PointDeductions;
using NUnit.Framework;

namespace FootballHistoryTest.Api.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetFullLeagueTable_returns_complete_league_table()
        {
            var leagueMatches = new List<MatchModel> { new MatchModel
                {
                    Tier = 1,
                    Division = "League",
                    Date = new DateTime(2010,  1, 1),
                    HomeTeam = "Team One",
                    HomeTeamAbbreviation = "One",
                    AwayTeam = "Team Two",
                    AwayTeamAbbreviation = "Two",
                    HomeGoals = 1,
                    AwayGoals = 0,
                },
                new MatchModel
                {
                    Tier = 1,
                    Division = "League",
                    Date = new DateTime(2010, 1, 2),
                    HomeTeam = "Team One",
                    HomeTeamAbbreviation = "One",
                    AwayTeam = "Team Three",
                    AwayTeamAbbreviation = "Three",
                    HomeGoals = 1,
                    AwayGoals = 0,
                },
                new MatchModel
                {
                    Tier = 1,
                    Division = "League",
                    Date = new DateTime(2010, 1, 3),
                    HomeTeam = "Team Two",
                    HomeTeamAbbreviation = "Two",
                    AwayTeam = "Team Three",
                    AwayTeamAbbreviation = "Three",
                    HomeGoals = 1,
                    AwayGoals = 0,
                },
                new MatchModel
                {
                    Tier = 1,
                    Division = "League",
                    Date = new DateTime(2010, 1, 4),
                    HomeTeam = "Team Two",
                    HomeTeamAbbreviation = "Two",
                    AwayTeam = "Team One",
                    AwayTeamAbbreviation = "One",
                    HomeGoals = 0,
                    AwayGoals = 1,
                },
                new MatchModel
                {
                    Tier = 1,
                    Division = "League",
                    Date = new DateTime(2010, 1, 5),
                    HomeTeam = "Team Three",
                    HomeTeamAbbreviation = "Three",
                    AwayTeam = "Team Two",
                    AwayTeamAbbreviation = "Two",
                    HomeGoals = 0,
                    AwayGoals = 1,
                },
                new MatchModel
                {
                    Tier = 1,
                    Division = "League",
                    Date = new DateTime(2010, 1, 6),
                    HomeTeam = "Team Three",
                    HomeTeamAbbreviation = "Three",
                    AwayTeam = "Team One",
                    AwayTeamAbbreviation = "One",
                    HomeGoals = 0,
                    AwayGoals = 1,
                }
            };
            var playOffMatches = new List<MatchModel>();
            var leagueModel = new LeagueModel
            {
                Name = "League",
                Tier = 1,
                TotalPlaces = 5,
                PromotionPlaces = 1,
                PlayOffPlaces = 0,
                RelegationPlaces = 1,
                PointsForWin = 3,
                StartYear = 2009
            };
            var pointsDeductions = new List<PointsDeductionModel>();

            var expectedLeagueTable = new List<LeagueTableRow>
            {
                new LeagueTableRow
                {
                    Position = 1,
                    Team = "Team One",
                    Played = 4,
                    Won = 4,
                    Drawn = 0,
                    Lost = 0,
                    GoalsFor = 4,
                    GoalsAgainst = 0,
                    GoalDifference = 4,
                    Points = 12,
                    PointsDeducted = 0,
                    PointsDeductionReason = null,
                    Status = "Champions"
                },
                new LeagueTableRow
                {
                    Position = 2,
                    Team = "Team Two",
                    Played = 4,
                    Won = 2,
                    Drawn = 0,
                    Lost = 2,
                    GoalsFor = 2,
                    GoalsAgainst = 2,
                    GoalDifference = 0,
                    Points = 6,
                    PointsDeducted = 0,
                    PointsDeductionReason = null,
                    Status = null
                },
                new LeagueTableRow
                {
                    Position = 3,
                    Team = "Team Three",
                    Played = 4,
                    Won = 0,
                    Drawn = 0,
                    Lost = 4,
                    GoalsFor = 0,
                    GoalsAgainst = 4,
                    GoalDifference = -4,
                    Points = 0,
                    PointsDeducted = 0,
                    PointsDeductionReason = null,
                    Status = "Relegated"
                }
            };
            var actualLeagueTable = LeagueTableCalculator.GetFullLeagueTable(leagueMatches, playOffMatches, leagueModel, pointsDeductions);

            AssertLeagueTablesMatch(actualLeagueTable, expectedLeagueTable);
        }

        [Test]
        public void GetPartialLeagueTable_without_points_deductions_returns_partial_league_table()
        {
            var leagueMatches = new List<MatchModel> { new MatchModel
                {
                    Tier = 1,
                    Division = "League",
                    Date = new DateTime(2010,  1, 1),
                    HomeTeam = "Team One",
                    HomeTeamAbbreviation = "One",
                    AwayTeam = "Team Two",
                    AwayTeamAbbreviation = "Two",
                    HomeGoals = 1,
                    AwayGoals = 0,
                },
                new MatchModel
                {
                    Tier = 1,
                    Division = "League",
                    Date = new DateTime(2010, 3, 1),
                    HomeTeam = "Team One",
                    HomeTeamAbbreviation = "One",
                    AwayTeam = "Team Three",
                    AwayTeamAbbreviation = "TwThreeo",
                    HomeGoals = 1,
                    AwayGoals = 0,
                }
            };
            var leagueModel = new LeagueModel
            {
                Name = "League",
                Tier = 1,
                TotalPlaces = 5,
                PromotionPlaces = 2,
                PlayOffPlaces = 0,
                RelegationPlaces = 1,
                PointsForWin = 3,
                StartYear = 2009
            };
            var pointsDeductions = new List<PointsDeductionModel>();

            var expectedLeagueTable = new List<LeagueTableRow> { new LeagueTableRow
                {
                    Position = 1,
                    Team = "Team One",
                    Played = 1,
                    Won = 1,
                    Drawn = 0,
                    Lost = 0,
                    GoalsFor = 1,
                    GoalsAgainst = 0,
                    GoalDifference = 1,
                    Points = 3,
                    PointsDeducted = 0,
                    PointsDeductionReason = null,
                    Status = null
                }, new LeagueTableRow
                {
                    Position = 2,
                    Team = "Team Three",
                    Played = 0,
                    Won = 0,
                    Drawn = 0,
                    Lost = 0,
                    GoalsFor = 0,
                    GoalsAgainst = 0,
                    GoalDifference = 0,
                    Points = 0,
                    PointsDeducted = 0,
                    PointsDeductionReason = null,
                    Status = null
                }, new LeagueTableRow
                {
                    Position = 3,
                    Team = "Team Two",
                    Played = 1,
                    Won = 0,
                    Drawn = 0,
                    Lost = 1,
                    GoalsFor = 0,
                    GoalsAgainst = 1,
                    GoalDifference = -1,
                    Points = 0,
                    PointsDeducted = 0,
                    PointsDeductionReason = null,
                    Status = null
                }
            };
            var actualLeagueTable = LeagueTableCalculator.GetPartialLeagueTable(leagueMatches, leagueModel, pointsDeductions, new DateTime(2010, 2, 1));

            AssertLeagueTablesMatch(actualLeagueTable, expectedLeagueTable);
        }

        [Test]
        public void GetPartialLeagueTable_with_points_deductions_returns_partial_league_table()
        {
            var leagueMatches = new List<MatchModel> { new MatchModel
                {
                    Tier = 1,
                    Division = "League",
                    Date = new DateTime(2010,  1, 1),
                    HomeTeam = "Team One",
                    HomeTeamAbbreviation = "One",
                    AwayTeam = "Team Two",
                    AwayTeamAbbreviation = "Two",
                    HomeGoals = 1,
                    AwayGoals = 0,
                },
                new MatchModel
                {
                    Tier = 1,
                    Division = "League",
                    Date = new DateTime(2010, 3, 1),
                    HomeTeam = "Team One",
                    HomeTeamAbbreviation = "One",
                    AwayTeam = "Team Three",
                    AwayTeamAbbreviation = "TwThreeo",
                    HomeGoals = 1,
                    AwayGoals = 0,
                }
            };
            var leagueModel = new LeagueModel
            {
                Name = "League",
                Tier = 1,
                TotalPlaces = 5,
                PromotionPlaces = 2,
                PlayOffPlaces = 0,
                RelegationPlaces = 1,
                PointsForWin = 3,
                StartYear = 2009
            };
            var pointsDeductions = new List<PointsDeductionModel> { new PointsDeductionModel
                {
                    Team = "Team Three",
                    SeasonStartYear = 2009,
                    PointsDeducted = 5,
                    Reason = "Financial irregularities",
                    Tier = 1
                }
            };

            var expectedLeagueTable = new List<LeagueTableRow> { new LeagueTableRow
                {
                    Position = 1,
                    Team = "Team One",
                    Played = 1,
                    Won = 1,
                    Drawn = 0,
                    Lost = 0,
                    GoalsFor = 1,
                    GoalsAgainst = 0,
                    GoalDifference = 1,
                    Points = 3,
                    PointsDeducted = 0,
                    PointsDeductionReason = null,
                    Status = null
                }, new LeagueTableRow
                {
                    Position = 2,
                    Team = "Team Two",
                    Played = 1,
                    Won = 0,
                    Drawn = 0,
                    Lost = 1,
                    GoalsFor = 0,
                    GoalsAgainst = 1,
                    GoalDifference = -1,
                    Points = 0,
                    PointsDeducted = 0,
                    PointsDeductionReason = null,
                    Status = null
                },
                new LeagueTableRow
                {
                    Position = 3,
                    Team = "Team Three",
                    Played = 0,
                    Won = 0,
                    Drawn = 0,
                    Lost = 0,
                    GoalsFor = 0,
                    GoalsAgainst = 0,
                    GoalDifference = 0,
                    Points = -5,
                    PointsDeducted = 5,
                    PointsDeductionReason = "Financial irregularities",
                    Status = null
                }
            };
            var actualLeagueTable = LeagueTableCalculator.GetPartialLeagueTable(leagueMatches, leagueModel, pointsDeductions, new DateTime(2010, 2, 1));

            AssertLeagueTablesMatch(actualLeagueTable, expectedLeagueTable);
        }

        private void AssertLeagueTablesMatch(List<LeagueTableRow> actualLeagueTable, List<LeagueTableRow> expectedLeagueTable)
        {
            foreach (var actualRow in actualLeagueTable)
            {
                var expectedRow = expectedLeagueTable.Single(exp => actualRow.Position == exp.Position);

                Assert.That(actualRow.Team, Is.EqualTo(expectedRow.Team));
                Assert.That(actualRow.Played, Is.EqualTo(expectedRow.Played));
                Assert.That(actualRow.Won, Is.EqualTo(expectedRow.Won));
                Assert.That(actualRow.Drawn, Is.EqualTo(expectedRow.Drawn));
                Assert.That(actualRow.Lost, Is.EqualTo(expectedRow.Lost));
                Assert.That(actualRow.Points, Is.EqualTo(expectedRow.Points));
                Assert.That(actualRow.GoalsFor, Is.EqualTo(expectedRow.GoalsFor));
                Assert.That(actualRow.GoalsAgainst, Is.EqualTo(expectedRow.GoalsAgainst));
                Assert.That(actualRow.GoalDifference, Is.EqualTo(expectedRow.GoalDifference));
                Assert.That(actualRow.Status, Is.EqualTo(expectedRow.Status));
                Assert.That(actualRow.PointsDeducted, Is.EqualTo(expectedRow.PointsDeducted));
                Assert.That(actualRow.PointsDeductionReason, Is.EqualTo(expectedRow.PointsDeductionReason));
            }
        }
    }
}
