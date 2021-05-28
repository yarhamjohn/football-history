using System;
using System.Collections.Generic;
using FluentAssertions;
using football.history.api.Builders;
using football.history.api.Controllers;
using football.history.api.Exceptions;
using football.history.api.Repositories.Competition;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Controllers
{
    [TestFixture]
    public class LeaguePositionControllerTests
    {
        [Test]
        public void GetLeaguePositions_should_return_message_for_unhandled_error()
        {
            var mockBuilder = new Mock<ILeaguePositionBuilder>();
            var mockRepository = new Mock<ICompetitionRepository>();
            mockRepository
                .Setup(x => x.GetCompetition(1))
                .Throws(new Exception("Unhandled error occurred."));

            var controller = new LeaguePositionController(mockRepository.Object, mockBuilder.Object);
            var (result, error) = controller.GetLeaguePositions(1, 1);

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Something went wrong. Unhandled error occurred.");
            error!.Code.Should().Be("UNKNOWN_ERROR");
        }

        [Test]
        public void GetLeaguePositions_should_return_message_for_handled_error()
        {
            var mockBuilder = new Mock<ILeaguePositionBuilder>();
            var mockRepository = new Mock<ICompetitionRepository>();
            mockRepository
                .Setup(x => x.GetCompetition(1))
                .Throws(new DataInvalidException("Repository data was invalid."));

            var controller = new LeaguePositionController(mockRepository.Object, mockBuilder.Object);
            var (result, error) = controller.GetLeaguePositions(1, 1);

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Repository data was invalid.");
            error!.Code.Should().Be("DATA_INVALID");
        }

        [Test]
        public void GetLeaguePositions_should_return_result()
        {
            var mockRepository = new Mock<ICompetitionRepository>();
            var competitionModel = new CompetitionModel(
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
                RelegationPlaces: 3,
                PlayOffPlaces: 0,
                RelegationPlayOffPlaces: 0,
                ReElectionPlaces: 0,
                FailedReElectionPosition: null);
            
            mockRepository
                .Setup(x => x.GetCompetition(1))
                .Returns(competitionModel);

            var mockBuilder = new Mock<ILeaguePositionBuilder>();
            var leaguePositionDtos = new List<LeaguePositionDto>();
            mockBuilder
                .Setup(x => x.GetPositions(1, competitionModel))
                .Returns(leaguePositionDtos);
            
            var controller = new LeaguePositionController(mockRepository.Object, mockBuilder.Object);
            var (result, error) = controller.GetLeaguePositions(1, 1);

            mockBuilder.VerifyAll();
            mockRepository.VerifyAll();
            result.Should().BeEquivalentTo(leaguePositionDtos);
            error.Should().BeNull();
        }
    }}