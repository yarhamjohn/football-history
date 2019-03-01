using System.Collections.Generic;
using FootballHistory.Api.Builders;
using FootballHistory.Api.Repositories.Models;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.BuildersTests
{
    [TestFixture]
    public class TeamLeagueMatchesTests
    {
        [Test]
        public void AllMethods_ShouldReturnZero_GivenNoGames()
        {
            var teamLeagueMatches = new TeamLeagueMatches(new List<MatchDetailModel>(), "Team1");

            Assert.Multiple(() =>
                {
                    Assert.That(teamLeagueMatches.CountGamesPlayed(), Is.EqualTo(0)); 
                    Assert.That(teamLeagueMatches.CountWins(), Is.EqualTo(0));
                    Assert.That(teamLeagueMatches.CountDraws(), Is.EqualTo(0));
                    Assert.That(teamLeagueMatches.CountDefeats(), Is.EqualTo(0));
                    Assert.That(teamLeagueMatches.CountGoalsFor(), Is.EqualTo(0));
                    Assert.That(teamLeagueMatches.CountGoalsAgainst(), Is.EqualTo(0));
                    Assert.That(teamLeagueMatches.CalculateGoalDifference(), Is.EqualTo(0));
                }
            );
        }
        
        [Test]
        public void AllMethods_ShouldReturnZero_GivenNoGamesForProvidedTeam()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel
                {
                    HomeTeam = "Team2", 
                    AwayTeam = "Team3",
                    HomeGoals = 1,
                    AwayGoals = 1
                }
            };
            var teamLeagueMatches = new TeamLeagueMatches(leagueMatches, "Team1");

            Assert.Multiple(() =>
                {
                    Assert.That(teamLeagueMatches.CountGamesPlayed(), Is.EqualTo(0)); 
                    Assert.That(teamLeagueMatches.CountWins(), Is.EqualTo(0));
                    Assert.That(teamLeagueMatches.CountDraws(), Is.EqualTo(0));
                    Assert.That(teamLeagueMatches.CountDefeats(), Is.EqualTo(0));
                    Assert.That(teamLeagueMatches.CountGoalsFor(), Is.EqualTo(0));
                    Assert.That(teamLeagueMatches.CountGoalsAgainst(), Is.EqualTo(0));
                    Assert.That(teamLeagueMatches.CalculateGoalDifference(), Is.EqualTo(0));
                }
            );
        }
        
        [Test]
        public void AllMethods_ShouldReturnCorrectResults_GivenASetOfMatchesForProvidedTeam()
        {
            var homeDraw = new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2", HomeGoals = 0, AwayGoals = 0 };
            var awayDraw = new MatchDetailModel { HomeTeam = "Team3", AwayTeam = "Team1", HomeGoals = 1, AwayGoals = 1 };
            var homeWin = new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team4", HomeGoals = 2, AwayGoals = 0 };
            var awayWin = new MatchDetailModel { HomeTeam = "Team5", AwayTeam = "Team1", HomeGoals = 0, AwayGoals = 1 };
            var homeDefeat = new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team6", HomeGoals = 1, AwayGoals = 2 };
            var awayDefeat = new MatchDetailModel { HomeTeam = "Team7", AwayTeam = "Team1", HomeGoals = 3, AwayGoals = 2 };
            var leagueMatches = new List<MatchDetailModel>
            {
                homeDraw,
                awayDraw,
                homeWin,
                awayWin,
                homeDefeat,
                awayDefeat
            };
            var teamLeagueMatches = new TeamLeagueMatches(leagueMatches, "Team1");

            Assert.Multiple(() =>
                {
                    Assert.That(teamLeagueMatches.CountGamesPlayed(), Is.EqualTo(6)); 
                    Assert.That(teamLeagueMatches.CountWins(), Is.EqualTo(2));
                    Assert.That(teamLeagueMatches.CountDraws(), Is.EqualTo(2));
                    Assert.That(teamLeagueMatches.CountDefeats(), Is.EqualTo(2));
                    Assert.That(teamLeagueMatches.CountGoalsFor(), Is.EqualTo(7));
                    Assert.That(teamLeagueMatches.CountGoalsAgainst(), Is.EqualTo(6));
                    Assert.That(teamLeagueMatches.CalculateGoalDifference(), Is.EqualTo(1));
                    Assert.That(teamLeagueMatches.AreInvalid, Is.False);
                }
            );
        }
        
        [Test]
        public void Build_ShouldThrowAnException_GivenTwoMatchesWithTheSameHomeAndAwayTeams()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" },
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team2" }
            };
            
            var teamLeagueMatches = new TeamLeagueMatches(leagueMatches, "Team1");
            
            Assert.That(teamLeagueMatches.AreInvalid(), Is.True);
        }
                
        [Test]
        public void Build_ShouldThrowAnException_GivenOneMatchWithTheSameHomeAndAwayTeam()
        {
            var leagueMatches = new List<MatchDetailModel>
            {
                new MatchDetailModel { HomeTeam = "Team1", AwayTeam = "Team1" }
            };
            
            var teamLeagueMatches = new TeamLeagueMatches(leagueMatches, "Team1");
            
            Assert.That(teamLeagueMatches.AreInvalid(), Is.True);
        }
    }
}
