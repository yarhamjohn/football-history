using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using football.history.api.Builders;
using football.history.api.Repositories.Competition;
using football.history.api.Repositories.Match;
using football.history.api.Repositories.PointDeduction;
using football.history.api.Repositories.Team;
using NUnit.Framework;

namespace football.history.api.Tests.Builders.LeagueTable
{
    [TestFixture]
    public class RowBuilderTests
    {
        private static IEnumerable RowCalculatedForTeam()
        {
            var norwichCityRow = new LeagueTableRowDto
            {
                TeamId                = 1,
                Team                  = "Norwich City",
                Played                = 2,
                Won                   = 2,
                Drawn                 = 0,
                Lost                  = 0,
                GoalsFor              = 3,
                GoalsAgainst          = 1,
                GoalDifference        = 2,
                GoalAverage           = 3,
                Points                = 6,
                PointsPerGame         = 3,
                PointsDeducted        = 0,
                PointsDeductionReason = null
            };
            yield return new TestCaseData(norwichCityRow).SetName("Row calculated for Norwich City");
            
            var newcastleUnitedRow = new LeagueTableRowDto
            {
                TeamId                = 2,
                Team                  = "Newcastle United",
                Played                = 2,
                Won                   = 0,
                Drawn                 = 1,
                Lost                  = 1,
                GoalsFor              = 1,
                GoalsAgainst          = 2,
                GoalDifference        = -1,
                GoalAverage           = 0.5,
                Points                = 1,
                PointsPerGame         = 0.5,
                PointsDeducted        = 0,
                PointsDeductionReason = null
            };
            yield return new TestCaseData(newcastleUnitedRow).SetName("Row calculated for Newcastle United");
            
            var sunderlandRow = new LeagueTableRowDto
            {
                TeamId                = 3,
                Team                  = "Sunderland",
                Played                = 1,
                Won                   = 0,
                Drawn                 = 0,
                Lost                  = 1,
                GoalsFor              = 1,
                GoalsAgainst          = 2,
                GoalDifference        = -1,
                GoalAverage           = 0.5,
                Points                = 0,
                PointsPerGame         = 0,
                PointsDeducted        = 0,
                PointsDeductionReason = null
            };
            yield return new TestCaseData(sunderlandRow).SetName("Row calculated for Sunderland");
            
            var arsenalRow = new LeagueTableRowDto
            {
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
                Points                = -2,
                PointsPerGame         = -2,
                PointsDeducted        = 3,
                PointsDeductionReason = "Financial Irregularities"
            };
            yield return new TestCaseData(arsenalRow).SetName("Row calculated for Arsenal");
        }

        [TestCaseSource(nameof(RowCalculatedForTeam))]
        public void Build_calculates_row_correctly(LeagueTableRowDto expectedRow)
        {
            var competition = GetCompetitionModel();
            var matches = GetMatches();
            var pointDeductions = GetPointDeductions();

            var builder = new RowBuilder();

            var teamModel = new TeamModel(expectedRow.TeamId, expectedRow.Team, expectedRow.Team, null);
            var result = builder.Build(competition, teamModel, matches, pointDeductions);

            result.Should().BeEquivalentTo(expectedRow);
        }

        [Test]
        public void Build_should_return_null_goal_average_given_team_that_conceded_no_goals()
        {
            var competition = GetCompetitionModel();
            var match = new MatchModel(1,
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
                AwayPenaltiesScored: 0);
                
            var builder = new RowBuilder();
            
            var teamModel = new TeamModel(1, "Norwich City", "NOR", null);
            var result = builder.Build(competition, teamModel, new List<MatchModel> { match }, new List<PointDeductionModel>());

            result.Should().BeEquivalentTo(new LeagueTableRowDto
            {
                TeamId                = 1,
                Team                  = "Norwich City",
                Played                = 1,
                Won                   = 1,
                Drawn                 = 0,
                Lost                  = 0,
                GoalsFor              = 1,
                GoalsAgainst          = 0,
                GoalDifference        = 1,
                GoalAverage           = null,
                Points                = 3,
                PointsPerGame         = 3,
                PointsDeducted        = 0,
                PointsDeductionReason = null
            });
        }
        
        [Test]
        public void Build_should_return_null_points_per_game_given_team_that_played_no_games()
        {
            var competition = GetCompetitionModel();
            
            var builder = new RowBuilder();
            
            var teamModel = new TeamModel(1, "Norwich City", "NOR", null);
            var result = builder.Build(competition, teamModel, new List<MatchModel>(), new List<PointDeductionModel>());

            result.Should().BeEquivalentTo(new LeagueTableRowDto
            {
                TeamId                = 1,
                Team                  = "Norwich City",
                Played                = 0,
                Won                   = 0,
                Drawn                 = 0,
                Lost                  = 0,
                GoalsFor              = 0,
                GoalsAgainst          = 0,
                GoalDifference        = 0,
                GoalAverage           = null,
                Points                = 0,
                PointsPerGame         = null,
                PointsDeducted        = 0,
                PointsDeductionReason = null
            });
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
                RelegationPlaces: 1,
                PlayOffPlaces: 1,
                RelegationPlayOffPlaces: 0,
                ReElectionPlaces: 0,
                FailedReElectionPosition: null);
        }

        private static List<PointDeductionModel> GetPointDeductions()
            => new()
            {
                new PointDeductionModel(1, 1, 3, 4, "Arsenal", "Financial Irregularities")
            };

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
    }
}