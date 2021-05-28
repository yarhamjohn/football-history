// using System;
// using System.Collections.Generic;
// using FluentAssertions;
// using football.history.api.Builders;
// using football.history.api.Repositories.Competition;
// using football.history.api.Repositories.Match;
// using football.history.api.Repositories.PointDeduction;
// using football.history.api.Repositories.Team;
// using Moq;
// using NUnit.Framework;
//
// namespace football.history.api.Tests.Builders.LeagueTable
// {
//     [TestFixture]
//     public class LeagueTableRowsBuilderTests
//     {
//         [Test]
//         public void BuildRows_without_matches_builds_rows_from_matches_returned_via_repository()
//         {
//             var competition = GetCompetitionModel();
//
//             var matches = GetMatches();
//             var mockMatchRepository = new Mock<IMatchRepository>();
//             mockMatchRepository
//                 .Setup(x => x.GetLeagueMatches(competition.Id))
//                 .Returns(matches);
//
//             var pointDeductions = new List<PointDeductionModel>();
//             var mockPointDeductionRepository = new Mock<IPointDeductionRepository>();
//             mockPointDeductionRepository
//                 .Setup(x => x.GetPointDeductions(competition.Id))
//                 .Returns(pointDeductions);
//
//             var mockRowBuilder = new Mock<IRowBuilder>();
//             mockRowBuilder
//                 .Setup(x => x.Build(competition, It.IsAny<TeamModel>(), matches, pointDeductions))
//                 .Returns(new LeagueTableRowDto());
//
//             var builder = new LeagueTableRowsBuilder(mockMatchRepository.Object, mockPointDeductionRepository.Object,
//                 mockRowBuilder.Object);
//
//             var rows = builder.BuildRows(competition);
//
//             mockMatchRepository.VerifyAll();
//             mockPointDeductionRepository.VerifyAll();
//             mockRowBuilder.Verify(x => x.Build(competition, new TeamModel(1, "Norwich City", "NOR", null), matches, pointDeductions), Times.Once);
//             mockRowBuilder.Verify(x => x.Build(competition, new TeamModel(2, "Newcastle United", "NEW", null), matches, pointDeductions), Times.Once);
//             mockRowBuilder.Verify(x => x.Build(competition, new TeamModel(3, "Sunderland", "SUN", null), matches, pointDeductions), Times.Once);
//             mockRowBuilder.Verify(x => x.Build(competition, new TeamModel(4, "Arsenal", "ARS", null), matches, pointDeductions), Times.Once);
//             rows.Should().HaveCount(4);
//         }
//         
//         [Test]
//         public void BuildRows_with_matches_builds_rows_from_matches_provided()
//         {
//             var competition = GetCompetitionModel();
//
//             var matches = GetMatches();
//             var mockMatchRepository = new Mock<IMatchRepository>();
//
//             var pointDeductions = new List<PointDeductionModel>();
//             var mockPointDeductionRepository = new Mock<IPointDeductionRepository>();
//             mockPointDeductionRepository
//                 .Setup(x => x.GetPointDeductions(competition.Id))
//                 .Returns(pointDeductions);
//
//             var mockRowBuilder = new Mock<IRowBuilder>();
//             mockRowBuilder
//                 .Setup(x => x.Build(competition, It.IsAny<TeamModel>(), matches, pointDeductions))
//                 .Returns(new LeagueTableRowDto());
//
//             var builder = new LeagueTableRowsBuilder(mockMatchRepository.Object, mockPointDeductionRepository.Object,
//                 mockRowBuilder.Object);
//
//             var rows = builder.BuildRows(competition, matches);
//
//             mockPointDeductionRepository.VerifyAll();
//             mockRowBuilder.Verify(x => x.Build(competition, new TeamModel(1, "Norwich City", "NOR", null), matches, pointDeductions), Times.Once);
//             mockRowBuilder.Verify(x => x.Build(competition, new TeamModel(2, "Newcastle United", "NEW", null), matches, pointDeductions), Times.Once);
//             mockRowBuilder.Verify(x => x.Build(competition, new TeamModel(3, "Sunderland", "SUN", null), matches, pointDeductions), Times.Once);
//             mockRowBuilder.Verify(x => x.Build(competition, new TeamModel(4, "Arsenal", "ARS", null), matches, pointDeductions), Times.Once);
//             rows.Should().HaveCount(4);
//         }
//
//         private static CompetitionModel GetCompetitionModel()
//         {
//             return new(
//                 Id: 1,
//                 Name: "Premier League",
//                 SeasonId: 1,
//                 StartYear: 2000,
//                 EndYear: 2001,
//                 Tier: 1,
//                 Region: null,
//                 Comment: null,
//                 PointsForWin: 3,
//                 TotalPlaces: 20,
//                 PromotionPlaces: 0,
//                 RelegationPlaces: 3,
//                 PlayOffPlaces: 0,
//                 RelegationPlayOffPlaces: 0,
//                 ReElectionPlaces: 0,
//                 FailedReElectionPosition: null);
//         }
//
//         private static List<MatchModel> GetMatches()
//             => new()
//             {
//                 new MatchModel(1,
//                     new DateTime(2000, 1, 1),
//                     CompetitionId: 1,
//                     CompetitionName: "Premier League",
//                     CompetitionStartYear: 2000,
//                     CompetitionEndYear: 2001,
//                     CompetitionTier: 1,
//                     CompetitionRegion: null,
//                     RulesType: "League",
//                     RulesStage: null,
//                     RulesExtraTime: false,
//                     RulesPenalties: false,
//                     RulesNumLegs: null,
//                     RulesAwayGoals: false,
//                     RulesReplays: false,
//                     HomeTeamId: 1,
//                     HomeTeamName: "Norwich City",
//                     HomeTeamAbbreviation: "NOR",
//                     AwayTeamId: 2,
//                     AwayTeamName: "Newcastle United",
//                     AwayTeamAbbreviation: "NEW",
//                     HomeGoals: 1,
//                     AwayGoals: 0,
//                     HomeGoalsExtraTime: 0,
//                     AwayGoalsExtraTime: 0,
//                     HomePenaltiesTaken: 0,
//                     HomePenaltiesScored: 0,
//                     AwayPenaltiesTaken: 0,
//                     AwayPenaltiesScored: 0),
//                 new MatchModel(2,
//                     new DateTime(2000, 1, 2),
//                     CompetitionId: 1,
//                     CompetitionName: "Premier League",
//                     CompetitionStartYear: 2000,
//                     CompetitionEndYear: 2001,
//                     CompetitionTier: 1,
//                     CompetitionRegion: null,
//                     RulesType: "League",
//                     RulesStage: null,
//                     RulesExtraTime: false,
//                     RulesPenalties: false,
//                     RulesNumLegs: null,
//                     RulesAwayGoals: false,
//                     RulesReplays: false,
//                     HomeTeamId: 3,
//                     HomeTeamName: "Sunderland",
//                     HomeTeamAbbreviation: "SUN",
//                     AwayTeamId: 1,
//                     AwayTeamName: "Norwich City",
//                     AwayTeamAbbreviation: "NOR",
//                     HomeGoals: 1,
//                     AwayGoals: 0,
//                     HomeGoalsExtraTime: 0,
//                     AwayGoalsExtraTime: 0,
//                     HomePenaltiesTaken: 0,
//                     HomePenaltiesScored: 0,
//                     AwayPenaltiesTaken: 0,
//                     AwayPenaltiesScored: 0),
//                 new MatchModel(3,
//                     new DateTime(2000, 1, 3),
//                     CompetitionId: 1,
//                     CompetitionName: "Premier League",
//                     CompetitionStartYear: 2000,
//                     CompetitionEndYear: 2001,
//                     CompetitionTier: 1,
//                     CompetitionRegion: null,
//                     RulesType: "League",
//                     RulesStage: null,
//                     RulesExtraTime: false,
//                     RulesPenalties: false,
//                     RulesNumLegs: null,
//                     RulesAwayGoals: false,
//                     RulesReplays: false,
//                     HomeTeamId: 2,
//                     HomeTeamName: "Newcastle United",
//                     HomeTeamAbbreviation: "NEW",
//                     AwayTeamId: 4,
//                     AwayTeamName: "Arsenal",
//                     AwayTeamAbbreviation: "ARS",
//                     HomeGoals: 1,
//                     AwayGoals: 0,
//                     HomeGoalsExtraTime: 0,
//                     AwayGoalsExtraTime: 0,
//                     HomePenaltiesTaken: 0,
//                     HomePenaltiesScored: 0,
//                     AwayPenaltiesTaken: 0,
//                     AwayPenaltiesScored: 0)
//             };
//     }
// }