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
            var calculator = BuildCalculatorWithNoMatchesAndNoPointsDeduction();

            var gamesPlayed = calculator.CountGamesPlayed();
            
            Assert.That(gamesPlayed, Is.EqualTo(0)); 
        }
                
        [Test]
        public void CountGamesPlayed_ShouldReturnZero_GivenNoMatchingTeam()
        {
            var calculator = BuildCalculatorWithNoMatchingTeamAndNoPointsDeduction();

            var gamesPlayed = calculator.CountGamesPlayed();
            
            Assert.That(gamesPlayed, Is.EqualTo(0));
        }

        [Test]
        public void CountGamesPlayed_ShouldReturnCorrectCount_GivenAMatchingTeam()
        {
            var calculator = BuildCalculatorWithMatchingTeamAndNoPointsDeduction();

            var gamesPlayed = calculator.CountGamesPlayed();
            
            Assert.That(gamesPlayed, Is.EqualTo(6));
        }

        [Test]
        public void CountWins_ShouldReturnZero_GivenNoGames()
        {
            var calculator = BuildCalculatorWithNoMatchesAndNoPointsDeduction();

            var wins = calculator.CountWins();
            
            Assert.That(wins, Is.EqualTo(0)); 
        }
                
        [Test]
        public void CountWins_ShouldReturnZero_GivenNoMatchingTeam()
        {
            var calculator = BuildCalculatorWithNoMatchingTeamAndNoPointsDeduction();

            var wins = calculator.CountWins();
            
            Assert.That(wins, Is.EqualTo(0)); 
        }
                        
        [Test]
        public void CountWins_ShouldReturnCorrectCount_GivenMatchingTeam()
        {
            var calculator = BuildCalculatorWithMatchingTeamAndNoPointsDeduction();

            var wins = calculator.CountWins();
            
            Assert.That(wins, Is.EqualTo(2)); 
        }
        
        [Test]
        public void CountDraws_ShouldReturnZero_GivenNoGames()
        {
            var calculator = BuildCalculatorWithNoMatchesAndNoPointsDeduction();

            var draws = calculator.CountDraws();
            
            Assert.That(draws, Is.EqualTo(0)); 
        }
                
        [Test]
        public void CountDraws_ShouldReturnZero_GivenNoMatchingTeam()
        {
            var calculator = BuildCalculatorWithNoMatchingTeamAndNoPointsDeduction();

            var draws = calculator.CountDraws();
            
            Assert.That(draws, Is.EqualTo(0)); 
        }
                        
        [Test]
        public void CountDraws_ShouldReturnCorrectCount_GivenMatchingTeam()
        {
            var calculator = BuildCalculatorWithMatchingTeamAndNoPointsDeduction();

            var draws = calculator.CountDraws();
            
            Assert.That(draws, Is.EqualTo(2)); 
        }
                
        [Test]
        public void CountDefeats_ShouldReturnZero_GivenNoGames()
        {
            var calculator = BuildCalculatorWithNoMatchesAndNoPointsDeduction();

            var defeats = calculator.CountDefeats();
            
            Assert.That(defeats, Is.EqualTo(0)); 
        }
                
        [Test]
        public void CountDefeats_ShouldReturnZero_GivenNoMatchingTeam()
        {
            var calculator = BuildCalculatorWithNoMatchingTeamAndNoPointsDeduction();

            var defeats = calculator.CountDefeats();
            
            Assert.That(defeats, Is.EqualTo(0)); 
        }
                        
        [Test]
        public void CountDefeats_ShouldReturnCorrectCount_GivenMatchingTeam()
        {
            var calculator = BuildCalculatorWithMatchingTeamAndNoPointsDeduction();

            var defeats = calculator.CountDefeats();
            
            Assert.That(defeats, Is.EqualTo(2)); 
        }
        
        [Test]
        public void CountGoalsFor_ShouldReturnZero_GivenNoGames()
        {
            var calculator = BuildCalculatorWithNoMatchesAndNoPointsDeduction();

            var goalsFor = calculator.CountGoalsFor();
            
            Assert.That(goalsFor, Is.EqualTo(0)); 
        }
                
        [Test]
        public void CountGoalsFor_ShouldReturnZero_GivenNoMatchingTeam()
        {
            var calculator = BuildCalculatorWithNoMatchingTeamAndNoPointsDeduction();

            var goalsFor = calculator.CountGoalsFor();
            
            Assert.That(goalsFor, Is.EqualTo(0)); 
        }
                        
        [Test]
        public void CountGoalsFor_ShouldReturnCorrectCount_GivenMatchingTeam()
        {
            var calculator = BuildCalculatorWithMatchingTeamAndNoPointsDeduction();

            var goalsFor = calculator.CountGoalsFor();
            
            Assert.That(goalsFor, Is.EqualTo(10)); 
        }
        
        [Test]
        public void CountGoalsAgainst_ShouldReturnZero_GivenNoGames()
        {
            var calculator = BuildCalculatorWithNoMatchesAndNoPointsDeduction();

            var goalsAgainst = calculator.CountGoalsAgainst();
            
            Assert.That(goalsAgainst, Is.EqualTo(0)); 
        }
                
        [Test]
        public void CountGoalsAgainst_ShouldReturnZero_GivenNoMatchingTeam()
        {
            var calculator = BuildCalculatorWithNoMatchingTeamAndNoPointsDeduction();

            var goalsAgainst = calculator.CountGoalsAgainst();
            
            Assert.That(goalsAgainst, Is.EqualTo(0)); 
        }
                        
        [Test]
        public void CountGoalsAgainst_ShouldReturnCorrectCount_GivenMatchingTeam()
        {
            var calculator = BuildCalculatorWithMatchingTeamAndNoPointsDeduction();

            var goalsAgainst = calculator.CountGoalsAgainst();
            
            Assert.That(goalsAgainst, Is.EqualTo(9)); 
        }
        
        [Test]
        public void CalculateGoalDifference_ShouldReturnZero_GivenNoGames()
        {
            var calculator = BuildCalculatorWithNoMatchesAndNoPointsDeduction();

            var goalDifference = calculator.CalculateGoalDifference();
            
            Assert.That(goalDifference, Is.EqualTo(0)); 
        }
                
        [Test]
        public void CalculateGoalDifference_ShouldReturnZero_GivenNoMatchingTeam()
        {
            var calculator = BuildCalculatorWithNoMatchingTeamAndNoPointsDeduction();

            var goalsDifference = calculator.CalculateGoalDifference();
            
            Assert.That(goalsDifference, Is.EqualTo(0)); 
        }
                        
        [Test]
        public void CalculateGoalDifference_ShouldReturnCorrectCount_GivenMatchingTeam()
        {
            var calculator = BuildCalculatorWithMatchingTeamAndNoPointsDeduction();

            var goalDifference = calculator.CalculateGoalDifference();
            
            Assert.That(goalDifference, Is.EqualTo(1)); 
        }
                
        [Test]
        public void CalculatePoints_ShouldReturnZero_GivenNoGamesAndNoPointDeductions()
        {
            var calculator = BuildCalculatorWithNoMatchesAndNoPointsDeduction();

            var points = calculator.CalculatePoints();
            
            Assert.That(points, Is.EqualTo(0)); 
        }
                              
        [Test]
        public void CalculatePoints_ShouldReturnCorrectTotal_GivenNoGamesAndPointDeductions()
        {
            var calculator = BuildCalculatorWithNoMatchesAndPointDeductions();

            var points = calculator.CalculatePoints();
            
            Assert.That(points, Is.EqualTo(-4));
        }

        [Test]
        public void CalculatePoints_ShouldReturnZero_GivenNoMatchingTeamAndNoPointDeductions()
        {
            var calculator = BuildCalculatorWithNoMatchingTeamAndNoPointsDeduction();

            var points = calculator.CalculatePoints();
            
            Assert.That(points, Is.EqualTo(0)); 
        }
                      
        [Test]
        public void CalculatePoints_ShouldReturnCorrectTotal_GivenNoMatchingTeamAndPointDeductions()
        {
            var calculator = BuildCalculatorWithNoMatchingTeamAndPointDeductions();

            var points = calculator.CalculatePoints();
            
            Assert.That(points, Is.EqualTo(-4)); 
        }

        [Test]
        public void CalculatePoints_ShouldReturnCorrectTotal_GivenMatchingTeamAndNoPointDeductions()
        {
            var calculator = BuildCalculatorWithMatchingTeamAndNoPointsDeduction();

            var points = calculator.CalculatePoints();
            
            Assert.That(points, Is.EqualTo(8)); 
        }
        
        [Test]
        public void CalculatePoints_ShouldReturnCorrectTotal_GivenMatchingTeamAndPointDeductions()
        {
            var calculator = BuildCalculatorWithMatchingTeamAndPointDeductions();

            var points = calculator.CalculatePoints();
            
            Assert.That(points, Is.EqualTo(4));
        }

        [Test]
        public void GetPointDeductionReasons_ShouldReturnEmptyString_GivenNoGamesAndNoPointDeductions()
        {
            var calculator = BuildCalculatorWithNoMatchesAndNoPointsDeduction();

            var reasons = calculator.GetPointDeductionReasons();
            
            Assert.That(reasons, Is.EqualTo(string.Empty)); 
        }
        
        [Test]
        public void GetPointDeductionReasons_ShouldReturnCorrectString_GivenNoGamesAndPointDeductions()
        {
            var calculator = BuildCalculatorWithNoMatchesAndPointDeductions();

            var reasons = calculator.GetPointDeductionReasons();
            
            Assert.That(reasons, Is.EqualTo("Reason1, Reason3")); 
        }
        [Test]
        
        public void GetPointDeductionReasons_ShouldReturnEmptyString_GivenNoMatchingTeamAndNoPointDeductions()
        {
            var calculator = BuildCalculatorWithNoMatchingTeamAndNoPointsDeduction();

            var reasons = calculator.GetPointDeductionReasons();
            
            Assert.That(reasons, Is.EqualTo(string.Empty)); 
        }
                      
        [Test]
        public void GetPointDeductionReasons_ShouldReturnCorrectString_GivenNoMatchingTeamAndPointDeductions()
        {
            var calculator = BuildCalculatorWithNoMatchingTeamAndPointDeductions();

            var reasons = calculator.GetPointDeductionReasons();
            
            Assert.That(reasons, Is.EqualTo("Reason1, Reason3"));
        }

        [Test]
        public void GetPointDeductionReasons_ShouldReturnEmptyString_GivenAMatchingTeamAndNoPointDeduction()
        {
            var calculator = BuildCalculatorWithMatchingTeamAndNoPointsDeduction();

            var reasons = calculator.GetPointDeductionReasons();
            
            Assert.That(reasons, Is.EqualTo(string.Empty)); 
        }

        [Test]
        public void GetPointDeductionReasons_ShouldReturnCorrectString_GivenMatchingTeamAndPointDeductions()
        {
            var calculator = BuildCalculatorWithMatchingTeamAndPointDeductions();

            var reasons = calculator.GetPointDeductionReasons();
            
            Assert.That(reasons, Is.EqualTo("Reason1, Reason3")); 
        }
        
        private static LeagueTableCalculator BuildCalculatorWithNoMatchesAndNoPointsDeduction()
        {
            var matches = new List<MatchDetailModel>();
            var pointDeductions = new List<PointDeductionModel>();
            return new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);    
        }

        private static LeagueTableCalculator BuildCalculatorWithNoMatchingTeamAndNoPointsDeduction()
        {
            var matches = new List<MatchDetailModel> {OtherWinDefeat, OtherDraw};
            var pointDeductions = new List<PointDeductionModel>();
            return new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);
        }
        
        private static LeagueTableCalculator BuildCalculatorWithMatchingTeamAndNoPointsDeduction()
        {
            var matches = new List<MatchDetailModel> { HomeWin, HomeDraw, HomeDefeat, AwayDefeat, AwayDraw, AwayWin, OtherWinDefeat, OtherDraw };
            var pointDeductions = new List<PointDeductionModel>();
            return new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);
        }
        
        private static LeagueTableCalculator BuildCalculatorWithNoMatchesAndPointDeductions()
        {
            var matches = new List<MatchDetailModel>();
            var pointDeductions = BuildPointDeductions();
            return new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);
        }
        
        private static LeagueTableCalculator BuildCalculatorWithNoMatchingTeamAndPointDeductions()
        {
            var matches = new List<MatchDetailModel> {OtherWinDefeat, OtherDraw};
            var pointDeductions = BuildPointDeductions();
            return new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);
        }
        
        private static LeagueTableCalculator BuildCalculatorWithMatchingTeamAndPointDeductions()
        {
            var matches = new List<MatchDetailModel> { HomeWin, HomeDraw, HomeDefeat, AwayDefeat, AwayDraw, AwayWin, OtherWinDefeat, OtherDraw };
            var pointDeductions = BuildPointDeductions();
            return new LeagueTableCalculator(matches, pointDeductions, SelectedTeam);
        }

        private static List<PointDeductionModel> BuildPointDeductions()
        {
            return new List<PointDeductionModel>
            {
                new PointDeductionModel {Team = SelectedTeam, PointsDeducted = 1, Reason = "Reason1"},
                new PointDeductionModel {Team = OtherTeam, PointsDeducted = 2, Reason = "Reason2"},
                new PointDeductionModel {Team = SelectedTeam, PointsDeducted = 3, Reason = "Reason3"}
            };
        }
    }
}
