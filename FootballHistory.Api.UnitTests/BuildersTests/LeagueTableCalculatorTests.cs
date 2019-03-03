using System.Collections.Generic;
using FootballHistory.Api.Builders;
using FootballHistory.Api.Repositories.Models;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.BuildersTests
{
    [TestFixture]
    public class LeagueTableCalculatorTests
    {
        private const string SelectedTeam = "Team1";
        private const string OtherTeam = "Team2";
        
        private static readonly MatchDetailModel HomeWin = new MatchDetailModel { HomeTeam = SelectedTeam, AwayTeam = OtherTeam, HomeGoals = 3, AwayGoals = 1 };
        private static readonly MatchDetailModel HomeDraw = new MatchDetailModel { HomeTeam = SelectedTeam, AwayTeam = OtherTeam, HomeGoals = 2, AwayGoals = 2 };
        private static readonly MatchDetailModel HomeDefeat = new MatchDetailModel { HomeTeam = SelectedTeam, AwayTeam = OtherTeam, HomeGoals = 0, AwayGoals = 1 };
        
        private static readonly MatchDetailModel AwayDefeat = new MatchDetailModel { HomeTeam = OtherTeam, AwayTeam = SelectedTeam, HomeGoals = 2, AwayGoals = 1 };
        private static readonly MatchDetailModel AwayDraw = new MatchDetailModel { HomeTeam = OtherTeam, AwayTeam = SelectedTeam, HomeGoals = 1, AwayGoals = 1 };
        private static readonly MatchDetailModel AwayWin = new MatchDetailModel { HomeTeam = OtherTeam, AwayTeam = SelectedTeam, HomeGoals = 2, AwayGoals = 3 };
        
        private static readonly MatchDetailModel OtherWinDefeat = new MatchDetailModel { HomeTeam = OtherTeam, AwayTeam = OtherTeam, HomeGoals = 1, AwayGoals = 0 };
        private static readonly MatchDetailModel OtherDraw = new MatchDetailModel { HomeTeam = OtherTeam, AwayTeam = OtherTeam, HomeGoals = 0, AwayGoals = 0 };
        
        [Test]
        public void CountGamesPlayed_ShouldReturnZero_GivenNoGames()
        {
            var matches = new List<MatchDetailModel>();
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CountGamesPlayed(), Is.EqualTo(0)); 
        }
                
        [Test]
        public void CountGamesPlayed_ShouldReturnZero_GivenNoMatchingTeam()
        {
            var matches = new List<MatchDetailModel> { OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CountGamesPlayed(), Is.EqualTo(0)); 
        }
                        
        [Test]
        public void CountGamesPlayed_ShouldReturnCorrectCount_GivenAMatchingTeam()
        {
            var matches = new List<MatchDetailModel> { HomeWin, HomeDraw, HomeDefeat, AwayDefeat, AwayDraw, AwayWin, OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CountGamesPlayed(), Is.EqualTo(6)); 
        }
        
        [Test]
        public void CountWins_ShouldReturnZero_GivenNoGames()
        {
            var matches = new List<MatchDetailModel>();
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CountWins(), Is.EqualTo(0)); 
        }
                
        [Test]
        public void CountWins_ShouldReturnZero_GivenNoMatchingTeam()
        {
            var matches = new List<MatchDetailModel> { OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CountWins(), Is.EqualTo(0)); 
        }
                        
        [Test]
        public void CountWins_ShouldReturnCorrectCount_GivenMatchingTeam()
        {
            var matches = new List<MatchDetailModel> { HomeWin, HomeDraw, HomeDefeat, AwayDefeat, AwayDraw, AwayWin, OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CountWins(), Is.EqualTo(2)); 
        }
        
        [Test]
        public void CountDraws_ShouldReturnZero_GivenNoGames()
        {
            var matches = new List<MatchDetailModel>();
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CountDraws(), Is.EqualTo(0)); 
        }
                
        [Test]
        public void CountDraws_ShouldReturnZero_GivenNoMatchingTeam()
        {
            var matches = new List<MatchDetailModel> { OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CountDraws(), Is.EqualTo(0)); 
        }
                        
        [Test]
        public void CountDraws_ShouldReturnCorrectCount_GivenMatchingTeam()
        {
            var matches = new List<MatchDetailModel> { HomeWin, HomeDraw, HomeDefeat, AwayDefeat, AwayDraw, AwayWin, OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CountDraws(), Is.EqualTo(2)); 
        }
                
        [Test]
        public void CountDefeats_ShouldReturnZero_GivenNoGames()
        {
            var matches = new List<MatchDetailModel>();
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CountDefeats(), Is.EqualTo(0)); 
        }
                
        [Test]
        public void CountDefeats_ShouldReturnZero_GivenNoMatchingTeam()
        {
            var matches = new List<MatchDetailModel> { OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CountDefeats(), Is.EqualTo(0)); 
        }
                        
        [Test]
        public void CountDefeats_ShouldReturnCorrectCount_GivenMatchingTeam()
        {
            var matches = new List<MatchDetailModel> { HomeWin, HomeDraw, HomeDefeat, AwayDefeat, AwayDraw, AwayWin, OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CountDefeats(), Is.EqualTo(2)); 
        }
        
        [Test]
        public void CountGoalsFor_ShouldReturnZero_GivenNoGames()
        {
            var matches = new List<MatchDetailModel>();
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CountGoalsFor(), Is.EqualTo(0)); 
        }
                
        [Test]
        public void CountGoalsFor_ShouldReturnZero_GivenNoMatchingTeam()
        {
            var matches = new List<MatchDetailModel> { OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CountGoalsFor(), Is.EqualTo(0)); 
        }
                        
        [Test]
        public void CountGoalsFor_ShouldReturnCorrectCount_GivenMatchingTeam()
        {
            var matches = new List<MatchDetailModel> { HomeWin, HomeDraw, HomeDefeat, AwayDefeat, AwayDraw, AwayWin, OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CountGoalsFor(), Is.EqualTo(10)); 
        }
        
        [Test]
        public void CountGoalsAgainst_ShouldReturnZero_GivenNoGames()
        {
            var matches = new List<MatchDetailModel>();
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CountGoalsAgainst(), Is.EqualTo(0)); 
        }
                
        [Test]
        public void CountGoalsAgainst_ShouldReturnZero_GivenNoMatchingTeam()
        {
            var matches = new List<MatchDetailModel> { OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CountGoalsAgainst(), Is.EqualTo(0)); 
        }
                        
        [Test]
        public void CountGoalsAgainst_ShouldReturnCorrectCount_GivenMatchingTeam()
        {
            var matches = new List<MatchDetailModel> { HomeWin, HomeDraw, HomeDefeat, AwayDefeat, AwayDraw, AwayWin, OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CountGoalsAgainst(), Is.EqualTo(9)); 
        }
        
        [Test]
        public void CalculateGoalDifference_ShouldReturnZero_GivenNoGames()
        {
            var matches = new List<MatchDetailModel>();
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CalculateGoalDifference(), Is.EqualTo(0)); 
        }
                
        [Test]
        public void CalculateGoalDifference_ShouldReturnZero_GivenNoMatchingTeam()
        {
            var matches = new List<MatchDetailModel> { OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CalculateGoalDifference(), Is.EqualTo(0)); 
        }
                        
        [Test]
        public void CalculateGoalDifference_ShouldReturnCorrectCount_GivenMatchingTeam()
        {
            var matches = new List<MatchDetailModel> { HomeWin, HomeDraw, HomeDefeat, AwayDefeat, AwayDraw, AwayWin, OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CalculateGoalDifference(), Is.EqualTo(1)); 
        }
                
        [Test]
        public void CalculatePoints_ShouldReturnZero_GivenNoGamesAndNoPointDeductions()
        {
            var matches = new List<MatchDetailModel>();
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CalculatePoints(), Is.EqualTo(0)); 
        }
                              
        [Test]
        public void CalculatePoints_ShouldReturnCorrectTotal_GivenNoGamesAndAPointDeduction()
        {
            var matches = new List<MatchDetailModel>();
            var pointDeductions = new List<PointDeductionModel>
            {
                new PointDeductionModel { Team = SelectedTeam, PointsDeducted = 1 }, 
                new PointDeductionModel { Team = OtherTeam, PointsDeducted = 2}
            };
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CalculatePoints(), Is.EqualTo(-1)); 
        }
        
        [Test]
        public void CalculatePoints_ShouldReturnZero_GivenNoMatchingTeamAndNoPointDeductions()
        {
            var matches = new List<MatchDetailModel> { OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CalculatePoints(), Is.EqualTo(0)); 
        }
                      
        [Test]
        public void CalculatePoints_ShouldReturnCorrectTotal_GivenNoMatchingTeamAndAPointDeduction()
        {
            var matches = new List<MatchDetailModel> { OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>
            {
                new PointDeductionModel { Team = SelectedTeam, PointsDeducted = 1 }, 
                new PointDeductionModel { Team = OtherTeam, PointsDeducted = 2}
            };
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CalculatePoints(), Is.EqualTo(-1)); 
        }

        [Test]
        public void CalculatePoints_ShouldReturnCorrectTotal_GivenMatchingTeamAndNoPointDeductions()
        {
            var matches = new List<MatchDetailModel> { HomeWin, HomeDraw, HomeDefeat, AwayDefeat, AwayDraw, AwayWin, OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CalculatePoints(), Is.EqualTo(8)); 
        }
        
        [Test]
        public void CalculatePoints_ShouldReturnCorrectTotal_GivenMatchingTeamAndAPointDeduction()
        {
            var matches = new List<MatchDetailModel> { HomeWin, HomeDraw, HomeDefeat, AwayDefeat, AwayDraw, AwayWin, OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>
            {
                new PointDeductionModel { Team = SelectedTeam, PointsDeducted = 1 }, 
                new PointDeductionModel { Team = OtherTeam, PointsDeducted = 2}
            };
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CalculatePoints(), Is.EqualTo(7)); 
        }
                
        [Test]
        public void CalculatePoints_ShouldReturnCorrectTotal_GivenMatchingTeamAndMultiplePointDeductions()
        {
            var matches = new List<MatchDetailModel> { HomeWin, HomeDraw, HomeDefeat, AwayDefeat, AwayDraw, AwayWin, OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>
            {
                new PointDeductionModel { Team = SelectedTeam, PointsDeducted = 1 }, 
                new PointDeductionModel { Team = OtherTeam, PointsDeducted = 2},
                new PointDeductionModel { Team = SelectedTeam, PointsDeducted = 3 }, 
            };
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.CalculatePoints(), Is.EqualTo(4)); 
        }

        [Test]
        public void GetPointDeductionReasons_ShouldReturnEmptyString_GivenNoGamesAndNoPointDeductions()
        {
            var matches = new List<MatchDetailModel>();
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.GetPointDeductionReasons(), Is.EqualTo(string.Empty)); 
        }
        
        [Test]
        public void GetPointDeductionReasons_ShouldReturnEmptyString_GivenNoGamesAndAPointDeductions()
        {
            var matches = new List<MatchDetailModel>();
            var pointDeductions = new List<PointDeductionModel>
            {
                new PointDeductionModel { Team = SelectedTeam, PointsDeducted = 1, Reason = "Reason1" }, 
                new PointDeductionModel { Team = OtherTeam, PointsDeducted = 2, Reason = "Reason2" }
            };
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.GetPointDeductionReasons(), Is.EqualTo("Reason1")); 
        }
        [Test]
        
        public void GetPointDeductionReasons_ShouldReturnEmptyString_GivenNoMatchingTeamAndNoPointDeductions()
        {
            var matches = new List<MatchDetailModel> { OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.GetPointDeductionReasons(), Is.EqualTo(string.Empty)); 
        }
                      
        [Test]
        public void GetPointDeductionReasons_ShouldReturnEmptyString_GivenNoMatchingTeamAndAPointDeduction()
        {
            var matches = new List<MatchDetailModel> { OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>
            {
                new PointDeductionModel { Team = SelectedTeam, PointsDeducted = 1, Reason = "Reason1" }, 
                new PointDeductionModel { Team = OtherTeam, PointsDeducted = 2, Reason = "Reason2" }
            };
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.GetPointDeductionReasons(), Is.EqualTo("Reason1")); 
        }
        
        [Test]
        public void GetPointDeductionReasons_ShouldReturnEmptyString_GivenAMatchingTeamAndNoPointDeduction()
        {
            var matches = new List<MatchDetailModel> { HomeWin, HomeDraw, HomeDefeat, AwayDefeat, AwayDraw, AwayWin, OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.GetPointDeductionReasons(), Is.EqualTo(string.Empty)); 
        }

        [Test]
        public void GetPointDeductionReasons_ShouldReturnReason_GivenMatchingTeamAndAPointDeduction()
        {
            var matches = new List<MatchDetailModel> { HomeWin, HomeDraw, HomeDefeat, AwayDefeat, AwayDraw, AwayWin, OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>
            {
                new PointDeductionModel { Team = SelectedTeam, PointsDeducted = 1, Reason = "Reason1" }, 
                new PointDeductionModel { Team = OtherTeam, PointsDeducted = 2, Reason = "Reason2" }
            };
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.GetPointDeductionReasons(), Is.EqualTo("Reason1")); 
        }
        
        [Test]
        public void GetPointDeductionReasons_ShouldReturnReasons_GivenMatchingTeamAndMultiplePointDeductions()
        {
            var matches = new List<MatchDetailModel> { HomeWin, HomeDraw, HomeDefeat, AwayDefeat, AwayDraw, AwayWin, OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>
            {
                new PointDeductionModel { Team = SelectedTeam, PointsDeducted = 1, Reason = "Reason1" }, 
                new PointDeductionModel { Team = OtherTeam, PointsDeducted = 2, Reason = "Reason2" },
                new PointDeductionModel { Team = SelectedTeam, PointsDeducted = 1, Reason = "Reason3" } 
            };
            var calculator = new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    

            Assert.That(calculator.GetPointDeductionReasons(), Is.EqualTo("Reason1, Reason3")); 
        }
    }
}
