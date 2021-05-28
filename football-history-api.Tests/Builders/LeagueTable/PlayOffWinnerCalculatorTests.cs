using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;
using football.history.api.Builders;
using football.history.api.Exceptions;
using football.history.api.Repositories.Match;
using NUnit.Framework;

namespace football.history.api.Tests.Builders.LeagueTable
{
    [TestFixture]
    public class PlayOffWinnerCalculatorTests
    {
        [Test]
        public void GetOneLeggedFinalWinner_throws_given_final_with_no_winner()
        {
            var match = GetOneLeggedFinalMatch();
            var calculator = new PlayOffWinnerCalculator();

            var ex = Assert.Throws<InvalidOperationException>(() => calculator.GetOneLeggedFinalWinner(match));

            ex.Message.Should().Be("The specified match had no winner.");
        }

        [TestCase(1, 0, 0, 0, 0, 0, "HomeTeam")]
        [TestCase(0, 1, 0, 0, 0, 0, "AwayTeam")]
        [TestCase(1, 1, 1, 0, 0, 0, "HomeTeam")]
        [TestCase(1, 1, 0, 1, 0, 0, "AwayTeam")]
        [TestCase(1, 1, 1, 1, 1, 0, "HomeTeam")]
        [TestCase(1, 1, 1, 1, 0, 1, "AwayTeam")]
        public void GetOneLeggedFinalWinner_calculates_home_team_winner(
            int homeGoals,
            int awayGoals,
            int homeGoalsExtraTime,
            int awayGoalsExtraTime,
            int homePenaltiesScored,
            int awayPenaltiesScored,
            string expectedWinner)
        {
            var match = GetOneLeggedFinalMatch(
                homeGoals,
                awayGoals,
                homeGoalsExtraTime,
                awayGoalsExtraTime,
                homePenaltiesScored,
                awayPenaltiesScored);
            var calculator = new PlayOffWinnerCalculator();

            var actualWinner = calculator.GetOneLeggedFinalWinner(match);

            actualWinner.Should().Be(expectedWinner);
        }

        [Test]
        public void GetTwoLeggedFinalWinner_throws_given_final_with_no_winner()
        {
            var matches = GetTwoLeggedFinalMatches();
            var calculator = new PlayOffWinnerCalculator();

            var ex = Assert.Throws<InvalidOperationException>(() => calculator.GetTwoLeggedFinalWinner(matches));

            ex.Message.Should().Be("The specified two legged matches had no winner.");
        }

        [Test]
        public void GetTwoLeggedFinalWinner_throws_given_no_final_matches()
        {
            var matches = new List<MatchModel>();
            var calculator = new PlayOffWinnerCalculator();

            var ex = Assert.Throws<InvalidOperationException>(() => calculator.GetTwoLeggedFinalWinner(matches));

            ex.Message.Should().Be("Expected 2 matches but got 0.");
        }

        [Test]
        public void GetTwoLeggedFinalWinner_throws_given_too_few_final_matches()
        {
            var matches = new List<MatchModel> {GetOneLeggedFinalMatch()};
            var calculator = new PlayOffWinnerCalculator();

            var ex = Assert.Throws<InvalidOperationException>(() => calculator.GetTwoLeggedFinalWinner(matches));

            ex.Message.Should().Be("Expected 2 matches but got 1.");
        }

        [Test]
        public void GetTwoLeggedFinalWinner_throws_given_two_many_final_matches()
        {
            var matches = GetReplayFinalMatches();
            var calculator = new PlayOffWinnerCalculator();

            var ex = Assert.Throws<InvalidOperationException>(() => calculator.GetTwoLeggedFinalWinner(matches));

            ex.Message.Should().Be("Expected 2 matches but got 3.");
        }

        [Test]
        public void GetTwoLeggedFinalWinner_throws_if_both_matches_were_on_the_same_day()
        {
            var matches = new List<MatchModel> {GetOneLeggedFinalMatch(), GetOneLeggedFinalMatch()};
            var calculator = new PlayOffWinnerCalculator();

            var ex = Assert.Throws<DataInvalidException>(() => calculator.GetTwoLeggedFinalWinner(matches));

            ex.Message.Should()
                .Be(
                    $"Both matches have the same date ({new DateTime(2000, 1, 1).ToString(CultureInfo.InvariantCulture)})");
        }

        [Test]
        public void GetTwoLeggedFinalWinner_throws_if_first_leg_went_to_extra_time()
        {
            var matches = new List<MatchModel>
            {
                GetMatch(new DateTime(2000, 1, 1), 0, 0, 1, 0, 0, 0), 
                GetMatch(new DateTime(2000, 1, 2), 0, 0, 0, 0, 0, 0)
            };
            var calculator = new PlayOffWinnerCalculator();

            var ex = Assert.Throws<DataInvalidException>(() => calculator.GetTwoLeggedFinalWinner(matches));

            ex.Message.Should().Be("The first leg of a two-legged final should not go to extra time or penalties.");
        }

        [Test]
        public void GetTwoLeggedFinalWinner_throws_if_first_leg_went_to_penalties()
        {
            var matches = new List<MatchModel>
            {
                GetMatch(new DateTime(2000, 1, 1), 0, 0, 0, 0, 1, 0), 
                GetMatch(new DateTime(2000, 1, 2), 0, 0, 0, 0, 0, 0)
            };
            var calculator = new PlayOffWinnerCalculator();

            var ex = Assert.Throws<DataInvalidException>(() => calculator.GetTwoLeggedFinalWinner(matches));

            ex.Message.Should().Be("The first leg of a two-legged final should not go to extra time or penalties.");
        }

        private static IEnumerable Two_legged_matches()
        {
            var dateOne = new DateTime(2000, 1, 1);
            var dateTwo = new DateTime(2000, 1, 2);

            yield return new TestCaseData(
                GetMatch(dateOne, 1, 0, 0, 0, 0, 0, false),
                GetMatch(dateTwo, 0, 1, 0, 0, 0, 0, true),
                "HomeTeam").SetName("Home team wins after normal time.");
            
            yield return new TestCaseData(
                GetMatch(dateOne, 1, 2, 0, 0, 0, 0, false),
                GetMatch(dateTwo, 0, 1, 1, 0, 0, 0, true),
                "AwayTeam").SetName("Away team wins after extra time.");
            
            yield return new TestCaseData(
                GetMatch(dateOne, 2, 2, 0, 0, 0, 0, false),
                GetMatch(dateTwo, 1, 1, 1, 1, 0, 1, true),
                "HomeTeam").SetName("Home team wins after penalties.");
        }

        [TestCaseSource(nameof(Two_legged_matches))]
        public void GetTwoLeggedFinalWinner_calculates_home_team_winner(MatchModel matchOne, MatchModel matchTwo,
            string expectedWinner)
        {
            var matches = new List<MatchModel> {matchOne, matchTwo};

            var calculator = new PlayOffWinnerCalculator();

            var actualWinner = calculator.GetTwoLeggedFinalWinner(matches);

            actualWinner.Should().Be(expectedWinner);
        }

        [Test]
        public void GetReplayFinalWinner_throws_given_finals_with_no_winner()
        {
            var matches = GetReplayFinalMatches();
            var calculator = new PlayOffWinnerCalculator();

            var ex = Assert.Throws<InvalidOperationException>(() => calculator.GetReplayFinalWinner(matches));

            ex.Message.Should().Be("The specified match had no winner.");
        }

        [Test]
        public void GetReplayFinalWinner_throws_given_no_final_matches()
        {
            var matches = new List<MatchModel>();
            var calculator = new PlayOffWinnerCalculator();

            var ex = Assert.Throws<InvalidOperationException>(() => calculator.GetReplayFinalWinner(matches));

            ex.Message.Should().Be("Expected 3 matches but got 0.");
        }

        [Test]
        public void GetReplayFinalWinner_throws_given_too_few_final_matches()
        {
            var matches = GetTwoLeggedFinalMatches();
            var calculator = new PlayOffWinnerCalculator();

            var ex = Assert.Throws<InvalidOperationException>(() => calculator.GetReplayFinalWinner(matches));

            ex.Message.Should().Be("Expected 3 matches but got 2.");
        }

        [Test]
        public void GetReplayFinalWinner_throws_given_too_many_final_matches()
        {
            var matches = GetTwoLeggedFinalMatches();
            matches.AddRange(GetTwoLeggedFinalMatches());
            var calculator = new PlayOffWinnerCalculator();

            var ex = Assert.Throws<InvalidOperationException>(() => calculator.GetReplayFinalWinner(matches));

            ex.Message.Should().Be("Expected 3 matches but got 4.");
        }

        [Test]
        public void GetReplayFinalWinner_throws_given_multiple_replays()
        {
            var matches = new List<MatchModel>
                {GetOneLeggedFinalMatch(), GetOneLeggedFinalMatch(), GetOneLeggedFinalMatch()};
            var calculator = new PlayOffWinnerCalculator();

            var ex = Assert.Throws<InvalidOperationException>(() => calculator.GetReplayFinalWinner(matches));

            ex.Message.Should().Be("Expected only one replay match but got 3.");
        }

        [TestCase(1, 0, 0, 0, 0, 0, "HomeTeam")]
        [TestCase(0, 1, 0, 0, 0, 0, "AwayTeam")]
        [TestCase(1, 1, 1, 0, 0, 0, "HomeTeam")]
        [TestCase(1, 1, 0, 1, 0, 0, "AwayTeam")]
        [TestCase(1, 1, 1, 1, 1, 0, "HomeTeam")]
        [TestCase(1, 1, 1, 1, 0, 1, "AwayTeam")]
        public void GetReplayFinalWinner_calculates_home_team_winner(
            int homeGoals,
            int awayGoals,
            int homeGoalsExtraTime,
            int awayGoalsExtraTime,
            int homePenaltiesScored,
            int awayPenaltiesScored,
            string expectedWinner)
        {
            var matches = GetReplayFinalMatches(
                homeGoals,
                awayGoals,
                homeGoalsExtraTime,
                awayGoalsExtraTime,
                homePenaltiesScored,
                awayPenaltiesScored);

            var calculator = new PlayOffWinnerCalculator();

            var actualWinner = calculator.GetReplayFinalWinner(matches);

            actualWinner.Should().Be(expectedWinner);
        }

        private static MatchModel GetOneLeggedFinalMatch(
            int homeGoals = 0,
            int awayGoals = 0,
            int homeGoalsExtraTime = 0,
            int awayGoalsExtraTime = 0,
            int homePenaltiesScored = 0,
            int awayPenaltiesScored = 0)
            => GetMatch(new DateTime(2000, 1, 1),
                homeGoals,
                awayGoals,
                homeGoalsExtraTime,
                awayGoalsExtraTime,
                homePenaltiesScored,
                awayPenaltiesScored);

        private static List<MatchModel> GetTwoLeggedFinalMatches(
            int homeGoals = 0,
            int awayGoals = 0,
            int homeGoalsExtraTime = 0,
            int awayGoalsExtraTime = 0,
            int homePenaltiesScored = 0,
            int awayPenaltiesScored = 0)
            => new()
            {
                GetMatch(new DateTime(2000, 1, 1)),
                GetMatch(
                    new DateTime(2000, 1, 2),
                    homeGoals,
                    awayGoals,
                    homeGoalsExtraTime,
                    awayGoalsExtraTime,
                    homePenaltiesScored,
                    awayPenaltiesScored)
            };

        private static List<MatchModel> GetReplayFinalMatches(
            int homeGoals = 0,
            int awayGoals = 0,
            int homeGoalsExtraTime = 0,
            int awayGoalsExtraTime = 0,
            int homePenaltiesScored = 0,
            int awayPenaltiesScored = 0)
            => new()
            {
                GetMatch(new DateTime(2000, 1, 1)),
                GetMatch(new DateTime(2000, 1, 2)),
                GetMatch(new DateTime(2000, 1, 3),
                    homeGoals,
                    awayGoals,
                    homeGoalsExtraTime,
                    awayGoalsExtraTime,
                    homePenaltiesScored,
                    awayPenaltiesScored)
            };

        private static MatchModel GetMatch(
            DateTime matchDate,
            int homeGoals = 0,
            int awayGoals = 0,
            int homeGoalsExtraTime = 0,
            int awayGoalsExtraTime = 0,
            int homePenaltiesScored = 0,
            int awayPenaltiesScored = 0,
            bool invertedTeams = false)
            => new(1,
                matchDate,
                CompetitionId: 1,
                CompetitionName: "Championship",
                CompetitionStartYear: 2000,
                CompetitionEndYear: 2001,
                CompetitionTier: 2,
                CompetitionRegion: null,
                RulesType: "PlayOff",
                RulesStage: "Final",
                RulesExtraTime: false,
                RulesPenalties: false,
                RulesNumLegs: 1,
                RulesAwayGoals: false,
                RulesReplays: false,
                HomeTeamId: 1,
                HomeTeamName: invertedTeams ? "AwayTeam" : "HomeTeam",
                HomeTeamAbbreviation: invertedTeams ? "AT" : "HT",
                AwayTeamId: 2,
                AwayTeamName: invertedTeams ? "HomeTeam" : "AwayTeam",
                AwayTeamAbbreviation: invertedTeams ? "HT" : "AT",
                homeGoals,
                awayGoals,
                homeGoalsExtraTime,
                awayGoalsExtraTime,
                HomePenaltiesTaken: 0,
                homePenaltiesScored,
                AwayPenaltiesTaken: 0,
                awayPenaltiesScored);
    }
}