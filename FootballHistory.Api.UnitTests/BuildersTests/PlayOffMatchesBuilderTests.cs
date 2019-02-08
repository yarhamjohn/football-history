using System;
using System.Collections.Generic;
using System.Linq;
using FootballHistory.Api.Builders;
using FootballHistory.Api.Repositories.Models;
using NUnit.Framework;

namespace FootballHistory.Api.UnitTests.BuildersTests
{
    [TestFixture]
    public class PlayOffMatchesBuilderTests
    {
        private List<MatchDetailModel> _matchDetails;
        private PlayOffMatchesBuilder _playOffMatchesBuilder;
        private readonly DateTime _dayOne = DateTime.Now;
        
        [SetUp]
        public void SetUp()
        {
            _playOffMatchesBuilder = new PlayOffMatchesBuilder();
            _matchDetails = new List<MatchDetailModel>
            {
                new MatchDetailModel { Round = "SemiFinal", HomeTeam = "Team1", AwayTeam = "Team2", Date = _dayOne },
                new MatchDetailModel { Round = "SemiFinal", HomeTeam = "Team2", AwayTeam = "Team1", Date = _dayOne.AddDays(1) },
                new MatchDetailModel { Round = "SemiFinal", HomeTeam = "Team4", AwayTeam = "Team3", Date = _dayOne },
                new MatchDetailModel { Round = "SemiFinal", HomeTeam = "Team3", AwayTeam = "Team4", Date = _dayOne.AddDays(2) },
                new MatchDetailModel { Round = "Final", HomeTeam = "Team1", AwayTeam = "Team4", Date = _dayOne.AddDays(3) }
            };
        }

        [Test]
        public void GivenASetOfPlayOffMatches_ReturnPlayOffsWithCorrectFinal()
        {
            var playOffs = _playOffMatchesBuilder.Build(_matchDetails);

            var expectedFinal = _matchDetails.Single(m => m.Round == "Final");

            Assert.AreEqual(playOffs.Final.HomeTeam, expectedFinal.HomeTeam);
            Assert.AreEqual(playOffs.Final.AwayTeam, expectedFinal.AwayTeam);
        }

        [Test]
        public void GivenASetOfPlayOffMatches_ReturnPlayOffsWithTwoSemFinals()
        {
            var playOffs = _playOffMatchesBuilder.Build(_matchDetails);
            
            Assert.That(playOffs.SemiFinals.Count, Is.EqualTo(2));
        }

        [Test]
        public void GivenASetOfPlayOffMatches_SemiFinalFirstLegHomeTeam_ShouldBeTheSecondLegAwayTeam()
        {
            var playOffs = _playOffMatchesBuilder.Build(_matchDetails);

            foreach (var semiFinal in playOffs.SemiFinals)
            {
                Assert.AreEqual(semiFinal.FirstLeg.HomeTeam, semiFinal.SecondLeg.AwayTeam);
                Assert.AreEqual(semiFinal.FirstLeg.AwayTeam, semiFinal.SecondLeg.HomeTeam);
            }
        }
                
        [Test]
        public void GivenASetOfPlayOffMatches_SemiFinalFirstLeg_ShouldOccurBeforeTheSemiFinalSecondLeg()
        {
            var playOffs = _playOffMatchesBuilder.Build(_matchDetails);

            foreach (var semiFinal in playOffs.SemiFinals)
            {
                Assert.That(semiFinal.FirstLeg.Date, Is.LessThan(semiFinal.SecondLeg.Date));
            }
        }
    }
}
