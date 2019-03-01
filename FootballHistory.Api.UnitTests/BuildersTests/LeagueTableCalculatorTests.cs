using System.Collections.Generic;
using FootballHistory.Api.Builders;
using FootballHistory.Api.Repositories.Models;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.BuildersTests
{
    [TestFixture]
    public class LeagueTableCalculatorTests
    {
        [Test]
        public void AllMethods_ShouldReturnZero_GivenNoGames()
        {
            var homeGames = new List<MatchDetailModel>();
            var awayGames = new List<MatchDetailModel>();

            Assert.Multiple(() =>
                {
                    Assert.That(LeagueTableCalculator.CountGamesPlayed(homeGames, awayGames), Is.EqualTo(0)); 
                    Assert.That(LeagueTableCalculator.CountWins(homeGames, awayGames), Is.EqualTo(0));
                    Assert.That(LeagueTableCalculator.CountDraws(homeGames, awayGames), Is.EqualTo(0));
                    Assert.That(LeagueTableCalculator.CountDefeats(homeGames, awayGames), Is.EqualTo(0));
                    Assert.That(LeagueTableCalculator.CountGoalsFor(homeGames, awayGames), Is.EqualTo(0));
                    Assert.That(LeagueTableCalculator.CountGoalsAgainst(homeGames, awayGames), Is.EqualTo(0));
                    Assert.That(LeagueTableCalculator.CalculateGoalDifference(homeGames, awayGames), Is.EqualTo(0));
                }
            );
        }
        
        [Test]
        public void AllMethods_ShouldReturnCorrectResults_GivenASetOfMatches()
        {
            var homeGames = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2", HomeGoals = 0, AwayGoals = 0 },
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team4", HomeGoals = 2, AwayGoals = 0 },
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team6", HomeGoals = 1, AwayGoals = 2 }
            };
            var awayGames = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team3", AwayTeam = "Team1", HomeGoals = 1, AwayGoals = 1 },
                new MatchDetailModel { HomeTeam = "Team5", AwayTeam = "Team1", HomeGoals = 0, AwayGoals = 1 },
                new MatchDetailModel { HomeTeam = "Team7", AwayTeam = "Team1", HomeGoals = 3, AwayGoals = 2 }
            };

            Assert.Multiple(() =>
                {
                    Assert.That(LeagueTableCalculator.CountGamesPlayed(homeGames, awayGames), Is.EqualTo(6)); 
                    Assert.That(LeagueTableCalculator.CountWins(homeGames, awayGames), Is.EqualTo(2));
                    Assert.That(LeagueTableCalculator.CountDraws(homeGames, awayGames), Is.EqualTo(2));
                    Assert.That(LeagueTableCalculator.CountDefeats(homeGames, awayGames), Is.EqualTo(2));
                    Assert.That(LeagueTableCalculator.CountGoalsFor(homeGames, awayGames), Is.EqualTo(7));
                    Assert.That(LeagueTableCalculator.CountGoalsAgainst(homeGames, awayGames), Is.EqualTo(6));
                    Assert.That(LeagueTableCalculator.CalculateGoalDifference(homeGames, awayGames), Is.EqualTo(1));
                }
            );
        }
    }
}
