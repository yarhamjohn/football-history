using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Builders;
using FootballHistory.Api.Builders.Models;
using FootballHistory.Api.Repositories.Models;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.BuildersTests
{
    [TestFixture]
    public class ResultMatrixBuilderTests
    {
        private IResultMatrixBuilder _resultMatrixBuilder;
        
        [SetUp]
        public void SetUp()
        {
            _resultMatrixBuilder = new ResultMatrixBuilder(); 
        }

        [Test]
        public void ResultMatrix_IsEmpty_GivenNoMatches()
        {
            var matchDetails = new List<MatchDetailModel>();

            var resultMatrix = _resultMatrixBuilder.Build(matchDetails);
            
            Assert.That(resultMatrix.Rows, Is.Empty);
        }

        [Test]
        public void ResultMatrix_ContainsOneRow_GivenOneMatch()
        {
            var matchDetails = new List<MatchDetailModel>
            {
                new MatchDetailModel
                {
                    HomeTeam = "HomeTeam",
                    HomeTeamAbbreviation = "HT"
                }
            };

            var resultMatrix = _resultMatrixBuilder.Build(matchDetails);
            var expected = resultMatrix.Rows.Select(r => (r.HomeTeam, r.HomeTeamAbbreviation)).ToList();
            
            Assert.AreEqual(expected, new List<(string, string)> { ("HomeTeam", "HT") });
        }

        [Test]
        public void ResultMatrix_ContainsOneRow_GivenTwoMatchesWithTheSameHomeTeam()
        {
            var matchDetails = new List<MatchDetailModel>
            {
                new MatchDetailModel
                {
                    HomeTeam = "HomeTeam",
                    HomeTeamAbbreviation = "HT"
                },
                new MatchDetailModel
                {
                    HomeTeam = "HomeTeam",
                    HomeTeamAbbreviation = "HT"
                }
            };

            var resultMatrix = _resultMatrixBuilder.Build(matchDetails);
            var expected = resultMatrix.Rows.Select(r => (r.HomeTeam, r.HomeTeamAbbreviation)).ToList();
            
            Assert.AreEqual(expected, new List<(string, string)> { ("HomeTeam", "HT") });
        }
        
        [Test]
        public void ResultMatrix_ContainsTwoRows_GivenTwoMatches()
        {
            var matchDetails = new List<MatchDetailModel>
            {
                new MatchDetailModel
                {
                    HomeTeam = "HomeTeam",
                    HomeTeamAbbreviation = "HT"
                },
                new MatchDetailModel
                {
                    HomeTeam = "HomeTeam2",
                    HomeTeamAbbreviation = "HT2"
                }
            };

            var resultMatrix = _resultMatrixBuilder.Build(matchDetails);
            var expected = resultMatrix.Rows.Select(r => (r.HomeTeam, r.HomeTeamAbbreviation)).ToList();
            
            Assert.AreEqual(expected, new List<(string, string)> { ("HomeTeam", "HT"), ("HomeTeam2", "HT2") });
        }

        [Test]
        public void ResultMatrix_AddsResultWithoutDataForTheoreticalMatchAgainstSelf()
        {
            var matchDetails = new List<MatchDetailModel>
            {
                new MatchDetailModel
                {
                    HomeTeam = "HomeTeam",
                    HomeTeamAbbreviation = "HT"
                }
            };

            var resultMatrix = _resultMatrixBuilder.Build(matchDetails);
            var expected = resultMatrix.Rows.SelectMany(r => r.Results).Single(m => m.AwayTeam == "HomeTeam");
            
            Assert.AreEqual(expected.AwayTeam, "HomeTeam");
            Assert.AreEqual(expected.AwayTeamAbbreviation, "HT");
            Assert.That(expected.AwayScore, Is.Null);
            Assert.That(expected.HomeScore, Is.Null);
            Assert.That(expected.MatchDate, Is.Null);
        }

        [Test]
        public void ResultMatrix_AddsCorrectResult_GivenOneMatch()
        {
            var matchDate = new DateTime(2017, 1, 1);
            var matchDetailModel = new MatchDetailModel
            {
                HomeTeam = "HomeTeam",
                HomeTeamAbbreviation = "HT",
                AwayTeam = "AwayTeam",
                AwayTeamAbbreviation = "AT",
                HomeGoals = 1,
                AwayGoals = 2,
                Date = matchDate
            };
            
            var matchDetails = new List<MatchDetailModel> { matchDetailModel };
            var resultMatrix = _resultMatrixBuilder.Build(matchDetails);
            var expected = resultMatrix.Rows.SelectMany(r => r.Results).Single(m => m.AwayTeam != "HomeTeam");
            
            Assert.That(MatchResultsMatch(expected, matchDetailModel));
        }
        
        [Test]
        public void ResultMatrix_AddsCorrectResults_GivenTwoMatchesWithOneHomeTeam()
        {
            var matchDate = new DateTime(2017, 1, 1);
            var matchDetailModelOne = new MatchDetailModel
            {
                HomeTeam = "HomeTeam",
                HomeTeamAbbreviation = "HT",
                AwayTeam = "AwayTeam",
                AwayTeamAbbreviation = "AT",
                HomeGoals = 1,
                AwayGoals = 2,
                Date = matchDate
            };
            var matchDetailModelTwo = new MatchDetailModel
            {
                HomeTeam = "HomeTeam",
                HomeTeamAbbreviation = "HT",
                AwayTeam = "AwayTeam2",
                AwayTeamAbbreviation = "AT2",
                HomeGoals = 1,
                AwayGoals = 0,
                Date = matchDate.AddDays(7)
            };
            
            var matchDetails = new List<MatchDetailModel> { matchDetailModelOne, matchDetailModelTwo };
            var resultMatrix = _resultMatrixBuilder.Build(matchDetails);
            
            var results = resultMatrix.Rows.Select(r => r.Results).Single();
            var expected = results.Where(m => m.AwayTeam != "HomeTeam").OrderBy(m => m.AwayTeam).ToList();
            
            Assert.That(MatchResultsMatch(expected.First(), matchDetailModelOne));
            Assert.That(MatchResultsMatch(expected.Last(), matchDetailModelTwo));
        }
                
        [Test]
        public void ResultMatrix_AddsCorrectResults_GivenTwoMatchesWithDifferentHomeTeams()
        {
            var matchDate = new DateTime(2017, 1, 1);
            var matchDetailModelOne = new MatchDetailModel
            {
                HomeTeam = "HomeTeam",
                HomeTeamAbbreviation = "HT",
                AwayTeam = "AwayTeam",
                AwayTeamAbbreviation = "AT",
                HomeGoals = 1,
                AwayGoals = 2,
                Date = matchDate
            };
            var matchDetailModelTwo = new MatchDetailModel
            {
                HomeTeam = "HomeTeam2",
                HomeTeamAbbreviation = "HT2",
                AwayTeam = "AwayTeam2",
                AwayTeamAbbreviation = "AT2",
                HomeGoals = 1,
                AwayGoals = 0,
                Date = matchDate.AddDays(7)
            };
            
            var matchDetails = new List<MatchDetailModel> { matchDetailModelOne, matchDetailModelTwo };
            var resultMatrix = _resultMatrixBuilder.Build(matchDetails);

            var orderedResults = resultMatrix.Rows.Select(r => r.Results).OrderBy(a => a.Max(x => x.AwayTeam)).ToList();
            var homeTeamResults = orderedResults.First();
            var homeTeam2Results = orderedResults.Last();
            
            var expectedHomeTeamResults = homeTeamResults.Single(m => m.AwayTeam != "HomeTeam");
            var expectedHomeTeam2Results = homeTeam2Results.Single(m => m.AwayTeam != "HomeTeam2");

            Assert.That(MatchResultsMatch(expectedHomeTeamResults, matchDetailModelOne));
            Assert.That(MatchResultsMatch(expectedHomeTeam2Results, matchDetailModelTwo));
        }
        
        private static bool MatchResultsMatch(ResultMatrixMatch expectedMatchResult, MatchDetailModel sourceMatchDetail)
        {
            return expectedMatchResult.AwayTeam == sourceMatchDetail.AwayTeam
                   && expectedMatchResult.AwayTeamAbbreviation == sourceMatchDetail.AwayTeamAbbreviation
                   && expectedMatchResult.HomeScore == sourceMatchDetail.HomeGoals
                   && expectedMatchResult.AwayScore == sourceMatchDetail.AwayGoals
                   && expectedMatchResult.MatchDate == sourceMatchDetail.Date;
        }
    }
}
