// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using football.history.api.Builders;
// using football.history.api.Calculators;
// using football.history.api.Repositories.League;
// using football.history.api.Repositories.Match;
// using football.history.api.Repositories.PointDeductions;
// using Newtonsoft.Json;
// using NUnit.Framework;
//
// namespace football.history.api.Tests
// {
//     public class Tests
//     {
//         private List<MatchModel> _leagueMatches;
//         private List<MatchModel> _playOffMatches;
//         private List<PointsDeductionModel> _pointsDeductions;
//
//         [SetUp]
//         public void Setup()
//         {
//             var leagueMatchesJson = File.ReadAllText("./TestSource/LeagueMatches.json");
//             _leagueMatches = JsonConvert.DeserializeObject<List<MatchModel>>(leagueMatchesJson);
//
//             var playOffMatchesJson = File.ReadAllText("./TestSource/PlayOffMatches.json");
//             _playOffMatches = JsonConvert.DeserializeObject<List<MatchModel>>(playOffMatchesJson);
//
//             var pointsDeductionsJson = File.ReadAllText("./TestSource/PointsDeductions.json");
//             _pointsDeductions =
//                 JsonConvert.DeserializeObject<List<PointsDeductionModel>>(pointsDeductionsJson);
//         }
//
//         [Test]
//         public void GetFullLeagueTable_returns_complete_league_table()
//         {
//             var leagueJson = File.ReadAllText("./TestSource/FinalLeague.json");
//             var league = JsonConvert.DeserializeObject<LeagueDto>(leagueJson);
//             var leagueModel = new LeagueModel
//             {
//                 Name = league.Name,
//                 Tier = league.Tier,
//                 TotalPlaces = league.TotalPlaces,
//                 PromotionPlaces = league.PromotionPlaces,
//                 PlayOffPlaces = league.PlayOffPlaces,
//                 RelegationPlaces = league.RelegationPlaces,
//                 PointsForWin = league.PointsForWin,
//                 StartYear = league.StartYear
//             };
//
//             var actualLeagueTable = new LeagueTableBuilder().GetFullLeagueTable(
//                 _leagueMatches,
//                 _playOffMatches,
//                 new List<MatchModel>(),
//                 leagueModel,
//                 _pointsDeductions);
//
//             AssertLeagueTablesMatch(actualLeagueTable, league.Table);
//         }
//
//         [Test]
//         public void GetPartialLeagueTable_with_points_deductions_returns_partial_league_table()
//         {
//             var leagueJson = File.ReadAllText("./TestSource/PartialLeague.json");
//             var league = JsonConvert.DeserializeObject<LeagueDto>(leagueJson);
//             var leagueModel = new LeagueModel
//             {
//                 Name = league.Name,
//                 Tier = league.Tier,
//                 TotalPlaces = league.TotalPlaces,
//                 PromotionPlaces = league.PromotionPlaces,
//                 PlayOffPlaces = league.PlayOffPlaces,
//                 RelegationPlaces = league.RelegationPlaces,
//                 PointsForWin = league.PointsForWin,
//                 StartYear = league.StartYear
//             };
//
//             var actualLeagueTable = new LeagueTableBuilder().GetPartialLeagueTable(
//                 _leagueMatches,
//                 leagueModel,
//                 _pointsDeductions,
//                 new DateTime(2012, 1, 1));
//
//             AssertLeagueTablesMatch(actualLeagueTable, league.Table);
//         }
//
//         private void AssertLeagueTablesMatch(
//             List<LeagueTableRowDto> actualLeagueTable,
//             List<LeagueTableRowDto> expectedLeagueTable)
//         {
//             foreach (var actualRow in actualLeagueTable)
//             {
//                 var expectedRow =
//                     expectedLeagueTable.Single(exp => actualRow.Position == exp.Position);
//
//                 Assert.That(actualRow.Team, Is.EqualTo(expectedRow.Team));
//                 Assert.That(actualRow.Played, Is.EqualTo(expectedRow.Played));
//                 Assert.That(actualRow.Won, Is.EqualTo(expectedRow.Won));
//                 Assert.That(actualRow.Drawn, Is.EqualTo(expectedRow.Drawn));
//                 Assert.That(actualRow.Lost, Is.EqualTo(expectedRow.Lost));
//                 Assert.That(actualRow.Points, Is.EqualTo(expectedRow.Points));
//                 Assert.That(actualRow.GoalsFor, Is.EqualTo(expectedRow.GoalsFor));
//                 Assert.That(actualRow.GoalsAgainst, Is.EqualTo(expectedRow.GoalsAgainst));
//                 Assert.That(actualRow.GoalDifference, Is.EqualTo(expectedRow.GoalDifference));
//                 Assert.That(actualRow.Status, Is.EqualTo(expectedRow.Status));
//                 Assert.That(actualRow.PointsDeducted, Is.EqualTo(expectedRow.PointsDeducted));
//                 Assert.That(
//                     actualRow.PointsDeductionReason,
//                     Is.EqualTo(expectedRow.PointsDeductionReason));
//             }
//         }
//     }
// }
