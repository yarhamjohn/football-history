using System;
using System.Collections.Generic;
using FluentAssertions;
using football.history.api.Builders.Match;
using football.history.api.Controllers;
using football.history.api.Exceptions;
using football.history.api.Repositories.Match;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Controllers
{
    [TestFixture]
    public class MatchControllerTests
    {
        [Test]
        public void GetMatches_should_return_message_for_unhandled_error()
        {
            var mockRepository = new Mock<IMatchRepository>();
            mockRepository
                .Setup(x => 
                    x.GetMatches(null, null, null, null, null))
                .Throws(new Exception("Unhandled error occurred."));

            var controller = new MatchController(mockRepository.Object);
            var (result, error) = controller.GetMatches(null, null, null, null, null);

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Something went wrong. Unhandled error occurred.");
            error!.Code.Should().Be("UNKNOWN_ERROR");
        }

        [Test]
        public void GetMatches_should_return_message_for_handled_error()
        {
            var mockRepository = new Mock<IMatchRepository>();
            mockRepository
                .Setup(x => 
                    x.GetMatches(null, null, null, null, null))
                .Throws(new DataInvalidException("Repository data was invalid."));

            var controller = new MatchController(mockRepository.Object);
            var (result, error) = controller.GetMatches(null, null, null, null, null);

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Repository data was invalid.");
            error!.Code.Should().Be("DATA_INVALID");
        }

        [Test]
        public void GetMatches_should_return_result()
        {
            var mockRepository = new Mock<IMatchRepository>();
            var matchModels = new List<MatchModel>
            {
                new(
                    Id: 1,
                    MatchDate: new DateTime(2000, 1, 1),
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
                new(
                    Id: 2,
                    MatchDate: new DateTime(2000, 2, 1),
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
                    AwayTeamId: 1,
                    AwayTeamName: "Norwich City",
                    AwayTeamAbbreviation: "NOR",
                    HomeGoals: 0,
                    AwayGoals: 1,
                    HomeGoalsExtraTime: 0,
                    AwayGoalsExtraTime: 0,
                    HomePenaltiesTaken: 0,
                    HomePenaltiesScored: 0,
                    AwayPenaltiesTaken: 0,
                    AwayPenaltiesScored: 0)
            };

            mockRepository
                .Setup(x => 
                    x.GetMatches(null, null, null, null, null))
                .Returns(matchModels);

            var controller = new MatchController(mockRepository.Object);
            var (result, error) = controller.GetMatches(null, null, null, null, null);

            var matchDtos = new List<MatchDto>
            {
                new(Id: 1,
                    MatchDate: new DateTime(2000, 1, 1),
                    Competition: new(
                        Id: 1,
                        Name: "Premier League",
                        StartYear: 2000,
                        EndYear: 2001,
                        Level: "1"),
                    Rules: new(
                        Type: "League",
                        Stage: null,
                        ExtraTime: false,
                        Penalties: false,
                        NumLegs: null,
                        AwayGoals: false,
                        Replays: false),
                    HomeTeam: new(
                        Id: 1,
                        Name: "Norwich City",
                        Abbreviation: "NOR",
                        Goals: 1,
                        GoalsExtraTime: 0,
                        PenaltiesTaken: 0,
                        PenaltiesScored: 0),
                    AwayTeam: new(
                        Id: 2,
                        Name: "Newcastle United",
                        Abbreviation: "NEW",
                        Goals: 0,
                        GoalsExtraTime: 0,
                        PenaltiesTaken: 0,
                        PenaltiesScored: 0)),
                new(Id: 2,
                    MatchDate: new DateTime(2000, 2, 1),
                    Competition: new(
                        Id: 1,
                        Name: "Premier League",
                        StartYear: 2000,
                        EndYear: 2001,
                        Level: "1"),
                    Rules: new(
                        Type: "League",
                        Stage: null,
                        ExtraTime: false,
                        Penalties: false,
                        NumLegs: null,
                        AwayGoals: false,
                        Replays: false),
                    HomeTeam: new(
                        Id: 2,
                        Name: "Newcastle United",
                        Abbreviation: "NEW",
                        Goals: 0,
                        GoalsExtraTime: 0,
                        PenaltiesTaken: 0,
                        PenaltiesScored: 0),
                    AwayTeam: new(
                        Id: 1,
                        Name: "Norwich City",
                        Abbreviation: "NOR",
                        Goals: 1,
                        GoalsExtraTime: 0,
                        PenaltiesTaken: 0,
                        PenaltiesScored: 0))
            };
            
            mockRepository.VerifyAll();
            result.Should().BeEquivalentTo(matchDtos);
            error.Should().BeNull();
        }

        [Test]
        public void GetMatch_should_return_message_for_unhandled_error()
        {
            var mockRepository = new Mock<IMatchRepository>();
            mockRepository
                .Setup(x => x.GetMatch(1))
                .Throws(new Exception("Unhandled error occurred."));

            var controller = new MatchController(mockRepository.Object);
            var (result, error) = controller.GetMatch(1);

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Something went wrong. Unhandled error occurred.");
            error!.Code.Should().Be("UNKNOWN_ERROR");
        }

        [Test]
        public void GetMatch_should_return_message_for_handled_error()
        {
            var mockRepository = new Mock<IMatchRepository>();
            mockRepository
                .Setup(x => x.GetMatch(1))
                .Throws(new DataInvalidException("Repository data was invalid."));

            var controller = new MatchController(mockRepository.Object);
            var (result, error) = controller.GetMatch(1);

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Repository data was invalid.");
            error!.Code.Should().Be("DATA_INVALID");
        }

        [Test]
        public void GetMatch_should_return_result()
        {
            var mockRepository = new Mock<IMatchRepository>();
            var matchModel = new MatchModel(
                Id: 1,
                MatchDate: new DateTime(2000, 1, 1),
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

            mockRepository
                .Setup(x => x.GetMatch(1))
                .Returns(matchModel);

            var controller = new MatchController(mockRepository.Object);
            var (result, error) = controller.GetMatch(1);

            var matchDto = new MatchDto(
                Id: 1,
                MatchDate: new DateTime(2000, 1, 1),
                Competition: new(
                    Id: 1,
                    Name: "Premier League",
                    StartYear: 2000,
                    EndYear: 2001,
                    Level: "1"),
                Rules: new(
                    Type: "League",
                    Stage: null,
                    ExtraTime: false,
                    Penalties: false,
                    NumLegs: null,
                    AwayGoals: false,
                    Replays: false),
                HomeTeam: new(
                    Id: 1,
                    Name: "Norwich City",
                    Abbreviation: "NOR",
                    Goals: 1,
                    GoalsExtraTime: 0,
                    PenaltiesTaken: 0,
                    PenaltiesScored: 0),
                AwayTeam: new(
                    Id: 2,
                    Name: "Newcastle United",
                    Abbreviation: "NEW",
                    Goals: 0,
                    GoalsExtraTime: 0,
                    PenaltiesTaken: 0,
                    PenaltiesScored: 0));
            
            mockRepository.VerifyAll();
            result.Should().Be(matchDto);
            error.Should().BeNull();
        }
    }
}