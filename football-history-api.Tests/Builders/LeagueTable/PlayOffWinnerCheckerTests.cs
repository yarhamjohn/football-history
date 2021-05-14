using System;
using System.Collections.Generic;
using FluentAssertions;
using football.history.api.Builders;
using football.history.api.Exceptions;
using football.history.api.Repositories.Competition;
using football.history.api.Repositories.Match;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Builders.LeagueTable
{
    [TestFixture]
    public class PlayOffWinnerCheckerTests
    {
        [Test]
        public void IsPlayOffWinner_returns_false_if_competition_has_no_play_off_places()
        {
            var competition = GetCompetition(false);
            var checker = new PlayOffWinnerChecker(
                new Mock<IMatchRepository>().Object,
                new Mock<ICompetitionRepository>().Object,
                new Mock<IPlayOffWinnerCalculator>().Object);

            var result = checker.IsPlayOffWinner(competition, "Norwich City");

            result.Should().BeFalse();
        }

        [Test]
        public void IsPlayOffWinner_returns_false_if_team_was_not_in_play_offs()
        {
            var competition = GetCompetition();
            var matches = GetOneLeggedFinalMatches();

            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository
                .Setup(x => x.GetPlayOffMatches(competition.Id))
                .Returns(() => matches);

            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerCalculator>();
            mockPlayOffWinnerCalculator
                .Setup(x => x.GetOneLeggedFinalWinner(It.IsAny<MatchModel>()))
                .Returns("Norwich City");

            var checker = new PlayOffWinnerChecker(
                mockMatchRepository.Object,
                new Mock<ICompetitionRepository>().Object,
                mockPlayOffWinnerCalculator.Object);

            var result = checker.IsPlayOffWinner(competition, "NotInPlayOffs");

            mockMatchRepository.VerifyAll();
            result.Should().BeFalse();
        }

        [TestCase("Swindon Town", false)]
        [TestCase("Sunderland", true)]
        public void IsPlayOffWinner_fixes_winner_due_to_financial_irregularities_in_tier_2_in_1989(string teamName,
            bool expected)
        {
            var competition = GetCompetition(true, 2);
            var matches = GetOneLeggedFinalMatches();

            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository
                .Setup(x => x.GetPlayOffMatches(competition.Id))
                .Returns(() => matches);

            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerCalculator>();
            mockPlayOffWinnerCalculator
                .Setup(x => x.GetOneLeggedFinalWinner(It.IsAny<MatchModel>()))
                .Returns("Swindon Town");

            var checker = new PlayOffWinnerChecker(
                mockMatchRepository.Object,
                new Mock<ICompetitionRepository>().Object,
                mockPlayOffWinnerCalculator.Object);

            var result = checker.IsPlayOffWinner(competition, teamName);

            mockMatchRepository.VerifyAll();
            result.Should().Be(expected);
        }

        [Test]
        public void IsPlayOffWinner_calls_one_legged_final_winner_calculator()
        {
            var competition = GetCompetition();
            var matches = GetOneLeggedFinalMatches();

            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository
                .Setup(x => x.GetPlayOffMatches(competition.Id))
                .Returns(matches);

            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerCalculator>();
            mockPlayOffWinnerCalculator
                .Setup(x => x.GetOneLeggedFinalWinner(It.IsAny<MatchModel>()))
                .Returns("Norwich City");

            var checker = new PlayOffWinnerChecker(
                mockMatchRepository.Object,
                new Mock<ICompetitionRepository>().Object,
                mockPlayOffWinnerCalculator.Object);

            var result = checker.IsPlayOffWinner(competition, "Norwich City");

            new Mock<ICompetitionRepository>().VerifyAll();
            result.Should().BeTrue();
        }

        [Test]
        public void IsPlayOffWinner_calls_two_legged_final_winner_calculator()
        {
            var competition = GetCompetition();
            var matches = GetTwoLeggedFinalMatches();

            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository
                .Setup(x => x.GetPlayOffMatches(competition.Id))
                .Returns(matches);

            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerCalculator>();
            mockPlayOffWinnerCalculator
                .Setup(x => x.GetTwoLeggedFinalWinner(It.IsAny<List<MatchModel>>()))
                .Returns("Norwich City");

            var checker = new PlayOffWinnerChecker(
                mockMatchRepository.Object,
                new Mock<ICompetitionRepository>().Object,
                mockPlayOffWinnerCalculator.Object);

            var result = checker.IsPlayOffWinner(competition, "Norwich City");

            new Mock<ICompetitionRepository>().VerifyAll();
            result.Should().BeTrue();
        }

        [Test]
        public void IsPlayOffWinner_calls_replay_final_winner_calculator()
        {
            var competition = GetCompetition();
            var matches = GetReplayFinalMatches();

            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository
                .Setup(x => x.GetPlayOffMatches(competition.Id))
                .Returns(matches);

            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerCalculator>();
            mockPlayOffWinnerCalculator
                .Setup(x => x.GetReplayFinalWinner(It.IsAny<List<MatchModel>>()))
                .Returns("Norwich City");

            var checker = new PlayOffWinnerChecker(
                mockMatchRepository.Object,
                new Mock<ICompetitionRepository>().Object,
                mockPlayOffWinnerCalculator.Object);

            var result = checker.IsPlayOffWinner(competition, "Norwich City");

            new Mock<ICompetitionRepository>().VerifyAll();
            result.Should().BeTrue();
        }

        [Test]
        public void IsPlayOffWinner_throws_if_too_many_final_matches_are_returned()
        {
            var competition = GetCompetition();
            var matches = GetTooManyFinalMatches();

            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository
                .Setup(x => x.GetPlayOffMatches(competition.Id))
                .Returns(matches);

            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerCalculator>();
            mockPlayOffWinnerCalculator
                .Setup(x => x.GetReplayFinalWinner(It.IsAny<List<MatchModel>>()))
                .Returns("Norwich City");

            var checker = new PlayOffWinnerChecker(
                mockMatchRepository.Object,
                new Mock<ICompetitionRepository>().Object,
                mockPlayOffWinnerCalculator.Object);

            var ex = Assert.Throws<DataInvalidException>(() => checker.IsPlayOffWinner(competition, "Norwich City"));

            new Mock<ICompetitionRepository>().VerifyAll();
            ex.Message.Should().Be("Cannot determine play off winner as too many final matches (4) were found.");
        }

        [Test]
        public void IsRelegationPlayOffWinner_returns_false_if_competition_has_no_relegation_play_off_places()
        {
            var competition = GetCompetition(false);
            var checker = new PlayOffWinnerChecker(
                new Mock<IMatchRepository>().Object,
                new Mock<ICompetitionRepository>().Object,
                new Mock<IPlayOffWinnerCalculator>().Object);

            var result = checker.IsRelegationPlayOffWinner(competition, "Norwich City");

            result.Should().BeFalse();
        }

        [Test]
        public void IsRelegationPlayOffWinner_returns_false_if_team_was_not_in_play_offs()
        {
            var competition = GetCompetition();
            var feederCompetition = GetCompetition(true, 2);
            var matches = GetOneLeggedFinalMatches();

            var mockCompetitionRepository = new Mock<ICompetitionRepository>();
            mockCompetitionRepository
                .Setup(x => x.GetCompetitionForSeasonAndTier(competition.SeasonId, competition.Tier + 1))
                .Returns(feederCompetition);

            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository
                .Setup(x => x.GetPlayOffMatches(feederCompetition.Id))
                .Returns(matches);

            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerCalculator>();
            mockPlayOffWinnerCalculator
                .Setup(x => x.GetOneLeggedFinalWinner(It.IsAny<MatchModel>()))
                .Returns("Norwich City");

            var checker = new PlayOffWinnerChecker(
                mockMatchRepository.Object,
                mockCompetitionRepository.Object,
                mockPlayOffWinnerCalculator.Object);

            var result = checker.IsRelegationPlayOffWinner(competition, "NotInPlayOffs");

            mockCompetitionRepository.VerifyAll();
            result.Should().BeFalse();
        }

        [Test]
        public void IsRelegationPlayOffWinner_calls_one_legged_final_winner_calculator()
        {
            var competition = GetCompetition();
            var feederCompetition = GetCompetition(true, 2);
            var matches = GetOneLeggedFinalMatches();

            var mockCompetitionRepository = new Mock<ICompetitionRepository>();
            mockCompetitionRepository
                .Setup(x => x.GetCompetitionForSeasonAndTier(competition.SeasonId, competition.Tier + 1))
                .Returns(feederCompetition);

            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository
                .Setup(x => x.GetPlayOffMatches(feederCompetition.Id))
                .Returns(matches);

            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerCalculator>();
            mockPlayOffWinnerCalculator
                .Setup(x => x.GetOneLeggedFinalWinner(It.IsAny<MatchModel>()))
                .Returns("Norwich City");

            var checker = new PlayOffWinnerChecker(
                mockMatchRepository.Object,
                mockCompetitionRepository.Object,
                mockPlayOffWinnerCalculator.Object);

            var result = checker.IsRelegationPlayOffWinner(competition, "Norwich City");

            mockCompetitionRepository.VerifyAll();
            result.Should().BeTrue();
        }

        [Test]
        public void IsRelegationPlayOffWinner_calls_two_legged_final_winner_calculator()
        {
            var competition = GetCompetition();
            var feederCompetition = GetCompetition(true, 2);
            var matches = GetTwoLeggedFinalMatches();

            var mockCompetitionRepository = new Mock<ICompetitionRepository>();
            mockCompetitionRepository
                .Setup(x => x.GetCompetitionForSeasonAndTier(competition.SeasonId, competition.Tier + 1))
                .Returns(feederCompetition);

            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository
                .Setup(x => x.GetPlayOffMatches(feederCompetition.Id))
                .Returns(matches);

            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerCalculator>();
            mockPlayOffWinnerCalculator
                .Setup(x => x.GetTwoLeggedFinalWinner(It.IsAny<List<MatchModel>>()))
                .Returns("Norwich City");

            var checker = new PlayOffWinnerChecker(
                mockMatchRepository.Object,
                mockCompetitionRepository.Object,
                mockPlayOffWinnerCalculator.Object);

            var result = checker.IsRelegationPlayOffWinner(competition, "Norwich City");

            mockCompetitionRepository.VerifyAll();
            result.Should().BeTrue();
        }

        [Test]
        public void IsRelegationPlayOffWinner_calls_replay_final_winner_calculator()
        {
            var competition = GetCompetition();
            var feederCompetition = GetCompetition(true, 2);
            var matches = GetReplayFinalMatches();

            var mockCompetitionRepository = new Mock<ICompetitionRepository>();
            mockCompetitionRepository
                .Setup(x => x.GetCompetitionForSeasonAndTier(competition.SeasonId, competition.Tier + 1))
                .Returns(feederCompetition);

            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository
                .Setup(x => x.GetPlayOffMatches(feederCompetition.Id))
                .Returns(matches);

            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerCalculator>();
            mockPlayOffWinnerCalculator
                .Setup(x => x.GetReplayFinalWinner(It.IsAny<List<MatchModel>>()))
                .Returns("Norwich City");

            var checker = new PlayOffWinnerChecker(
                mockMatchRepository.Object,
                mockCompetitionRepository.Object,
                mockPlayOffWinnerCalculator.Object);

            var result = checker.IsRelegationPlayOffWinner(competition, "Norwich City");

            mockCompetitionRepository.VerifyAll();
            result.Should().BeTrue();
        }

        [Test]
        public void IsRelegationPlayOffWinner_throws_if_too_many_final_matches_are_returned()
        {
            var competition = GetCompetition();
            var feederCompetition = GetCompetition(true, 2);
            var matches = GetTooManyFinalMatches();

            var mockCompetitionRepository = new Mock<ICompetitionRepository>();
            mockCompetitionRepository
                .Setup(x => x.GetCompetitionForSeasonAndTier(competition.SeasonId, competition.Tier + 1))
                .Returns(feederCompetition);

            var mockMatchRepository = new Mock<IMatchRepository>();
            mockMatchRepository
                .Setup(x => x.GetPlayOffMatches(feederCompetition.Id))
                .Returns(matches);

            var mockPlayOffWinnerCalculator = new Mock<IPlayOffWinnerCalculator>();
            mockPlayOffWinnerCalculator
                .Setup(x => x.GetTwoLeggedFinalWinner(It.IsAny<List<MatchModel>>()))
                .Returns("Norwich City");

            var checker = new PlayOffWinnerChecker(
                mockMatchRepository.Object,
                mockCompetitionRepository.Object,
                mockPlayOffWinnerCalculator.Object);
            
            var ex = Assert.Throws<DataInvalidException>(() =>
                checker.IsRelegationPlayOffWinner(competition, "Norwich City"));

            new Mock<ICompetitionRepository>().VerifyAll();
            ex.Message.Should().Be("Cannot determine play off winner as too many final matches (4) were found.");
        }

        private static CompetitionModel GetCompetition(bool withPlayOffMatches = true, int? tier = null)
            => new(
                1,
                "Premier League",
                1,
                1989,
                1990,
                tier ?? 1,
                null,
                null,
                0,
                0,
                0,
                0,
                withPlayOffMatches ? 1 : 0,
                withPlayOffMatches ? 1 : 0,
                0,
                0);

        private static List<MatchModel> GetOneLeggedFinalMatches()
            => new()
            {
                GetMatch("SemiFinal"),
                GetMatch("Final")
            };

        private static List<MatchModel> GetTwoLeggedFinalMatches()
            => new()
            {
                GetMatch("SemiFinal"),
                GetMatch("Final"),
                GetMatch("Final")
            };

        private static List<MatchModel> GetReplayFinalMatches()
            => new()
            {
                GetMatch("SemiFinal"),
                GetMatch("Final"),
                GetMatch("Final"),
                GetMatch("Final")
            };


        private static List<MatchModel> GetTooManyFinalMatches()
            => new()
            {
                GetMatch("SemiFinal"),
                GetMatch("Final"),
                GetMatch("Final"),
                GetMatch("Final"),
                GetMatch("Final")
            };


        private static MatchModel GetMatch(string stage)
            => new(
                Id: 1,
                MatchDate: new DateTime(2000, 1, 1),
                CompetitionId: 1,
                CompetitionName: "Championship",
                CompetitionStartYear: 2000,
                CompetitionEndYear: 2001,
                CompetitionTier: 2,
                CompetitionRegion: null,
                RulesType: "PlayOff",
                RulesStage: stage,
                RulesExtraTime: false,
                RulesPenalties: false,
                RulesNumLegs: 1,
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
    }
}