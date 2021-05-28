using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using football.history.api.Builders;
using football.history.api.Repositories.Competition;
using football.history.api.Repositories.Match;
using football.history.api.Repositories.PointDeduction;
using football.history.api.Repositories.Team;
using football.history.api.Tests.Builders.LeagueTable.Sorter;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Builders.LeagueTable
{
    [TestFixture]
    public class LeagueTableBuilderTests
    {
        [Test]
        public void BuildFullLeagueTable_should_set_position_and_status()
        {
            var competition = GetCompetitionModel();

            var mockMatchRepository = new Mock<IMatchRepository>();
            var matches = GetMatches();
            mockMatchRepository
                .Setup(x => x.GetLeagueMatches(competition.Id))
                .Returns(matches);
            
            var mockPointDeductionRepository = new Mock<IPointDeductionRepository>();
            var pointDeductions = new List<PointDeductionModel>();
            mockPointDeductionRepository
                .Setup(x => x.GetPointDeductions(competition.Id))
                .Returns(pointDeductions);
            
            var mockRowBuilder = new Mock<IRowBuilder>();
            mockRowBuilder
                .Setup(x => x.Build(competition, new TeamModel(1, "Norwich City", "NOR", null), matches, pointDeductions))
                .Returns(new LeagueTableRowDto() { Points = 3});
            mockRowBuilder
                .Setup(x => x.Build(competition, new TeamModel(2, "Newcastle United", "NEW", null), matches, pointDeductions))
                .Returns(new LeagueTableRowDto() { Points = 2});
            mockRowBuilder
                .Setup(x => x.Build(competition, new TeamModel(3, "Sunderland", "SUN", null), matches, pointDeductions))
                .Returns(new LeagueTableRowDto() { Points = 1});
            mockRowBuilder
                .Setup(x => x.Build(competition, new TeamModel(4, "Arsenal", "ARS", null), matches, pointDeductions))
                .Returns(new LeagueTableRowDto() { Points = 0});
            
            var mockRowComparerFactory = new Mock<IRowComparerFactory>();
            mockRowComparerFactory
                .Setup(x => x.GetLeagueTableComparer(competition))
                .Returns(new FakeComparer());

            var mockStatusCalculator = new Mock<IStatusCalculator>();
            mockStatusCalculator
                .Setup(x => x.GetStatus(It.IsAny<string>(), It.IsAny<int>(), competition))
                .Returns("assigned-status");

            var builder = new LeagueTableBuilder(
                mockMatchRepository.Object,
                mockPointDeductionRepository.Object,
                mockRowBuilder.Object,
                mockRowComparerFactory.Object,
                mockStatusCalculator.Object);
            var leagueTable = builder.BuildFullLeagueTable(competition);

            mockMatchRepository.VerifyAll();
            mockRowComparerFactory.VerifyAll();
            mockStatusCalculator.VerifyAll();
            
            mockRowBuilder.Verify(x => x.Build(competition, new TeamModel(1, "Norwich City", "NOR", null), matches, pointDeductions), Times.Once);
            mockRowBuilder.Verify(x => x.Build(competition, new TeamModel(2, "Newcastle United", "NEW", null), matches, pointDeductions), Times.Once);
            mockRowBuilder.Verify(x => x.Build(competition, new TeamModel(3, "Sunderland", "SUN", null), matches, pointDeductions), Times.Once);
            mockRowBuilder.Verify(x => x.Build(competition, new TeamModel(4, "Arsenal", "ARS", null), matches, pointDeductions), Times.Once);
            
            leagueTable.GetRows().Count.Should().Be(4);
            leagueTable.GetRows().Select(x => x.Status).Distinct().Single().Should().Be("assigned-status");
            leagueTable.GetRows().Select(x => x.Position).Should().BeEquivalentTo(new List<int> { 1, 2, 3, 4});
        }

        [Test]
        public void BuildPartialLeagueTable_should_set_position_but_not_status()
        {
            var competition = GetCompetitionModel();

            var mockMatchRepository = new Mock<IMatchRepository>();
            var matches = GetMatches();
            
            var mockPointDeductionRepository = new Mock<IPointDeductionRepository>();
            var pointDeductions = new List<PointDeductionModel>();
            mockPointDeductionRepository
                .Setup(x => x.GetPointDeductions(competition.Id))
                .Returns(pointDeductions);
            
            var mockRowBuilder = new Mock<IRowBuilder>();
            var matchesBeforeTarget = matches.Where(x => x.MatchDate < new DateTime(2000, 1, 2)).ToList();
            mockRowBuilder
                .Setup(x => x.Build(competition, new TeamModel(1, "Norwich City", "NOR", null), matchesBeforeTarget, pointDeductions))
                .Returns(new LeagueTableRowDto() { Points = 3});
            mockRowBuilder
                .Setup(x => x.Build(competition, new TeamModel(2, "Newcastle United", "NEW", null), matchesBeforeTarget, pointDeductions))
                .Returns(new LeagueTableRowDto() { Points = 2});
            mockRowBuilder
                .Setup(x => x.Build(competition, new TeamModel(3, "Sunderland", "SUN", null), matchesBeforeTarget, pointDeductions))
                .Returns(new LeagueTableRowDto() { Points = 1});
            mockRowBuilder
                .Setup(x => x.Build(competition, new TeamModel(4, "Arsenal", "ARS", null), matchesBeforeTarget, pointDeductions))
                .Returns(new LeagueTableRowDto() { Points = 0});

            var mockRowComparerFactory = new Mock<IRowComparerFactory>();
            mockRowComparerFactory
                .Setup(x => x.GetLeagueTableComparer(competition))
                .Returns(new FakeComparer());

            var mockStatusCalculator = new Mock<IStatusCalculator>();

            var builder = new LeagueTableBuilder(
                mockMatchRepository.Object,
                mockPointDeductionRepository.Object,
                mockRowBuilder.Object,
                mockRowComparerFactory.Object,
                mockStatusCalculator.Object);
            var leagueTable = builder.BuildPartialLeagueTable(competition, matches, new DateTime(2000, 1, 2), pointDeductions);

            mockMatchRepository.VerifyAll();
            mockRowComparerFactory.VerifyAll();
            mockStatusCalculator.VerifyAll();
            
            mockRowBuilder.Verify(x => x.Build(competition, new TeamModel(1, "Norwich City", "NOR", null), matchesBeforeTarget, pointDeductions), Times.Once);
            mockRowBuilder.Verify(x => x.Build(competition, new TeamModel(2, "Newcastle United", "NEW", null), matchesBeforeTarget, pointDeductions), Times.Once);
            mockRowBuilder.Verify(x => x.Build(competition, new TeamModel(3, "Sunderland", "SUN", null), matchesBeforeTarget, pointDeductions), Times.Once);
            mockRowBuilder.Verify(x => x.Build(competition, new TeamModel(4, "Arsenal", "ARS", null), matchesBeforeTarget, pointDeductions), Times.Once);
            
            leagueTable.GetRows().Count.Should().Be(4);
            leagueTable.GetRows().Select(x => x.Status).Distinct().Single().Should().BeNull();
            leagueTable.GetRows().Select(x => x.Position).Should().BeEquivalentTo(new List<int> { 1, 2, 3, 4});
        }

        private static CompetitionModel GetCompetitionModel()
        {
            return new(
                Id: 1,
                Name: "Premier League",
                SeasonId: 1,
                StartYear: 2000,
                EndYear: 2001,
                Tier: 1,
                Region: null,
                Comment: null,
                PointsForWin: 3,
                TotalPlaces: 20,
                PromotionPlaces: 0,
                RelegationPlaces: 3,
                PlayOffPlaces: 0,
                RelegationPlayOffPlaces: 0,
                ReElectionPlaces: 0,
                FailedReElectionPosition: null);
        }

        private static List<MatchModel> GetMatches()
            => new()
            {
                new MatchModel(1,
                    new DateTime(2000, 1, 1),
                    CompetitionId: 1,
                    CompetitionName: "Premier League",
                    CompetitionStartYear: 2000,
                    CompetitionEndYear: 2001,
                    CompetitionTier: 1,
                    CompetitionRegion: null,
                    RulesType: "League",
                    RulesStage: null,
                    RulesExtraTime: false,
                    RulesPenalties: false,
                    RulesNumLegs: null,
                    RulesAwayGoals: false,
                    RulesReplays: false,
                    HomeTeamId: 1,
                    HomeTeamName: "Norwich City",
                    HomeTeamAbbreviation: "NOR",
                    AwayTeamId: 2,
                    AwayTeamName: "Newcastle United",
                    AwayTeamAbbreviation: "NEW",
                    HomeGoals: 1,
                    AwayGoals: 0,
                    HomeGoalsExtraTime: 0,
                    AwayGoalsExtraTime: 0,
                    HomePenaltiesTaken: 0,
                    HomePenaltiesScored: 0,
                    AwayPenaltiesTaken: 0,
                    AwayPenaltiesScored: 0),
                new MatchModel(2,
                    new DateTime(2000, 1, 2),
                    CompetitionId: 1,
                    CompetitionName: "Premier League",
                    CompetitionStartYear: 2000,
                    CompetitionEndYear: 2001,
                    CompetitionTier: 1,
                    CompetitionRegion: null,
                    RulesType: "League",
                    RulesStage: null,
                    RulesExtraTime: false,
                    RulesPenalties: false,
                    RulesNumLegs: null,
                    RulesAwayGoals: false,
                    RulesReplays: false,
                    HomeTeamId: 3,
                    HomeTeamName: "Sunderland",
                    HomeTeamAbbreviation: "SUN",
                    AwayTeamId: 1,
                    AwayTeamName: "Norwich City",
                    AwayTeamAbbreviation: "NOR",
                    HomeGoals: 1,
                    AwayGoals: 2,
                    HomeGoalsExtraTime: 0,
                    AwayGoalsExtraTime: 0,
                    HomePenaltiesTaken: 0,
                    HomePenaltiesScored: 0,
                    AwayPenaltiesTaken: 0,
                    AwayPenaltiesScored: 0),
                new MatchModel(3,
                    new DateTime(2000, 1, 3),
                    CompetitionId: 1,
                    CompetitionName: "Premier League",
                    CompetitionStartYear: 2000,
                    CompetitionEndYear: 2001,
                    CompetitionTier: 1,
                    CompetitionRegion: null,
                    RulesType: "League",
                    RulesStage: null,
                    RulesExtraTime: false,
                    RulesPenalties: false,
                    RulesNumLegs: null,
                    RulesAwayGoals: false,
                    RulesReplays: false,
                    HomeTeamId: 2,
                    HomeTeamName: "Newcastle United",
                    HomeTeamAbbreviation: "NEW",
                    AwayTeamId: 4,
                    AwayTeamName: "Arsenal",
                    AwayTeamAbbreviation: "ARS",
                    HomeGoals: 1,
                    AwayGoals: 1,
                    HomeGoalsExtraTime: 0,
                    AwayGoalsExtraTime: 0,
                    HomePenaltiesTaken: 0,
                    HomePenaltiesScored: 0,
                    AwayPenaltiesTaken: 0,
                    AwayPenaltiesScored: 0)
            };

        private static List<LeagueTableRowDto> GetExpectedRows(string? status = null)
        {
            // norwich 1 - 0 newcastle
            // sunderland 1 - 2 norwich
            // newcastle 1 - 1 arsenal
            return new()
            {
                new LeagueTableRowDto()
                {
                    Position              = 4,
                    TeamId                = 3,
                    Team                  = "Sunderland",
                    Played                = 1,
                    Won                   = 0,
                    Drawn                 = 0,
                    Lost                  = 1,
                    GoalsFor              = 1,
                    GoalsAgainst          = 2,
                    GoalDifference        = -1,
                    GoalAverage           = 1,
                    Points                = 0,
                    PointsPerGame         = 0,
                    PointsDeducted        = 0,
                    PointsDeductionReason = null,
                    Status                = status
                },
                new LeagueTableRowDto()
                {
                    Position              = 1,
                    TeamId                = 1,
                    Team                  = "Norwich City",
                    Played                = 2,
                    Won                   = 2,
                    Drawn                 = 0,
                    Lost                  = 0,
                    GoalsFor              = 3,
                    GoalsAgainst          = 1,
                    GoalDifference        = 2,
                    GoalAverage           = 1.5,
                    Points                = 6,
                    PointsPerGame         = 3,
                    PointsDeducted        = 0,
                    PointsDeductionReason = null,
                    Status                = status
                },
                new LeagueTableRowDto()
                {
                    Position              = 3,
                    TeamId                = 2,
                    Team                  = "Newcastle United",
                    Played                = 2,
                    Won                   = 0,
                    Drawn                 = 1,
                    Lost                  = 1,
                    GoalsFor              = 1,
                    GoalsAgainst          = 2,
                    GoalDifference        = -1,
                    GoalAverage           = 1,
                    Points                = 1,
                    PointsPerGame         = 0.5,
                    PointsDeducted        = 0,
                    PointsDeductionReason = null,
                    Status                = status
                },
                new LeagueTableRowDto()
                {
                    Position              = 2,
                    TeamId                = 4,
                    Team                  = "Arsenal",
                    Played                = 1,
                    Won                   = 0,
                    Drawn                 = 1,
                    Lost                  = 0,
                    GoalsFor              = 1,
                    GoalsAgainst          = 1,
                    GoalDifference        = 0,
                    GoalAverage           = 1,
                    Points                = 1,
                    PointsPerGame         = 1,
                    PointsDeducted        = 0,
                    PointsDeductionReason = null,
                    Status                = status
                }
            };
        }
    }
}