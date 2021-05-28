using System;
using System.Collections.Generic;
using FluentAssertions;
using football.history.api.Controllers;
using football.history.api.Dtos;
using football.history.api.Exceptions;
using football.history.api.Repositories.Competition;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Controllers
{
    [TestFixture]
    public class CompetitionControllerTests
    {
        [Test]
        public void GetAllCompetitions_should_return_message_for_unhandled_error()
        {
            var mockRepository = new Mock<ICompetitionRepository>();
            mockRepository
                .Setup(x => x.GetAllCompetitions())
                .Throws(new Exception("Unhandled error occurred."));

            var controller = new CompetitionController(mockRepository.Object);
            var (result, error) = controller.GetAllCompetitions();

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Something went wrong. Unhandled error occurred.");
            error!.Code.Should().Be("UNKNOWN_ERROR");
        }

        [Test]
        public void GetAllCompetitions_should_return_message_for_handled_error()
        {
            var mockRepository = new Mock<ICompetitionRepository>();
            mockRepository
                .Setup(x => x.GetAllCompetitions())
                .Throws(new DataInvalidException("Repository data was invalid."));

            var controller = new CompetitionController(mockRepository.Object);
            var (result, error) = controller.GetAllCompetitions();

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Repository data was invalid.");
            error!.Code.Should().Be("DATA_INVALID");
        }

        [Test]
        public void GetAllCompetitions_should_return_result()
        {
            var mockRepository = new Mock<ICompetitionRepository>();
            var matchModels = new List<CompetitionModel>
            {
                new(
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
                    FailedReElectionPosition: null),
                new(
                    Id: 2,
                    Name: "Premier League",
                    SeasonId: 2,
                    StartYear: 2001,
                    EndYear: 2002,
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
                    FailedReElectionPosition: null)
            };

            mockRepository
                .Setup(x => x.GetAllCompetitions())
                .Returns(matchModels);

            var controller = new CompetitionController(mockRepository.Object);
            var (result, error) = controller.GetAllCompetitions();

            mockRepository.VerifyAll();
            var competitionDtos = new List<CompetitionDto>
            {
                new(Id: 1,
                    Name: "Premier League",
                    Season: new (
                        Id: 1,
                        StartYear: 2000,
                        EndYear: 2001),
                    Level: "1",
                    Comment: null,
                    Rules: new (
                        PointsForWin: 3,
                        TotalPlaces: 20,
                        PromotionPlaces: 0,
                        RelegationPlaces: 3,
                        PlayOffPlaces: 0,
                        RelegationPlayOffPlaces: 0,
                        ReElectionPlaces: 0,
                        FailedReElectionPosition: null)),
                new(Id: 2,
                    Name: "Premier League",
                    Season: new (
                        Id: 2,
                        StartYear: 2001,
                        EndYear: 2002),
                    Level: "1",
                    Comment: null,
                    Rules: new (
                        PointsForWin: 3,
                        TotalPlaces: 20,
                        PromotionPlaces: 0,
                        RelegationPlaces: 3,
                        PlayOffPlaces: 0,
                        RelegationPlayOffPlaces: 0,
                        ReElectionPlaces: 0,
                        FailedReElectionPosition: null))
            };
            result.Should().BeEquivalentTo(competitionDtos);
            error.Should().BeNull();
        }

        [Test]
        public void GetCompetition_should_return_message_for_unhandled_error()
        {
            var mockRepository = new Mock<ICompetitionRepository>();
            mockRepository
                .Setup(x => x.GetCompetition(1))
                .Throws(new Exception("Unhandled error occurred."));

            var controller = new CompetitionController(mockRepository.Object);
            var (result, error) = controller.GetCompetition(1);

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Something went wrong. Unhandled error occurred.");
            error!.Code.Should().Be("UNKNOWN_ERROR");
        }

        [Test]
        public void GetCompetition_should_return_message_for_handled_error()
        {
            var mockRepository = new Mock<ICompetitionRepository>();
            mockRepository
                .Setup(x => x.GetCompetition(1))
                .Throws(new DataInvalidException("Repository data was invalid."));

            var controller = new CompetitionController(mockRepository.Object);
            var (result, error) = controller.GetCompetition(1);

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Repository data was invalid.");
            error!.Code.Should().Be("DATA_INVALID");
        }

        [Test]
        public void GetCompetition_should_return_result()
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

            var controller = new CompetitionController(mockRepository.Object);
            var (result, error) = controller.GetCompetition(1);

            var competitionDto = new CompetitionDto(Id: 1,
                Name: "Premier League",
                Season: new (
                    Id: 1,
                    StartYear: 2000,
                    EndYear: 2001),
                Level: "1",
                Comment: null,
                Rules: new (
                    PointsForWin: 3,
                    TotalPlaces: 20,
                    PromotionPlaces: 0,
                    RelegationPlaces: 3,
                    PlayOffPlaces: 0,
                    RelegationPlayOffPlaces: 0,
                    ReElectionPlaces: 0,
                    FailedReElectionPosition: null));
            
            mockRepository.VerifyAll();
            result.Should().Be(competitionDto);
            error.Should().BeNull();
        }
        
        [Test]
        public void GetCompetitions_should_return_message_for_unhandled_error()
        {
            var mockRepository = new Mock<ICompetitionRepository>();
            mockRepository
                .Setup(x => x.GetCompetitionsInSeason(1))
                .Throws(new Exception("Unhandled error occurred."));

            var controller = new CompetitionController(mockRepository.Object);
            var (result, error) = controller.GetCompetitions(1);

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Something went wrong. Unhandled error occurred.");
            error!.Code.Should().Be("UNKNOWN_ERROR");
        }

        [Test]
        public void GetCompetitions_should_return_message_for_handled_error()
        {
            var mockRepository = new Mock<ICompetitionRepository>();
            mockRepository
                .Setup(x => x.GetCompetitionsInSeason(1))
                .Throws(new DataInvalidException("Repository data was invalid."));

            var controller = new CompetitionController(mockRepository.Object);
            var (result, error) = controller.GetCompetitions(1);

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Repository data was invalid.");
            error!.Code.Should().Be("DATA_INVALID");
        }

        [Test]
        public void GetCompetitions_should_return_result()
        {
            var mockRepository = new Mock<ICompetitionRepository>();
            var matchModels = new List<CompetitionModel>
            {
                new(
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
                    FailedReElectionPosition: null),
                new(
                    Id: 2,
                    Name: "Championship",
                    SeasonId: 1,
                    StartYear: 2000,
                    EndYear: 2001,
                    Tier: 2,
                    Region: null,
                    Comment: null,
                    PointsForWin: 3,
                    TotalPlaces: 24,
                    PromotionPlaces: 2,
                    RelegationPlaces: 3,
                    PlayOffPlaces: 4,
                    RelegationPlayOffPlaces: 0,
                    ReElectionPlaces: 0,
                    FailedReElectionPosition: null)
            };

            mockRepository
                .Setup(x => x.GetCompetitionsInSeason(1))
                .Returns(matchModels);

            var controller = new CompetitionController(mockRepository.Object);
            var (result, error) = controller.GetCompetitions(1);

            var competitionDtos = new List<CompetitionDto>
            {
                new(Id: 1,
                    Name: "Premier League",
                    Season: new (
                        Id: 1,
                        StartYear: 2000,
                        EndYear: 2001),
                    Level: "1",
                    Comment: null,
                    Rules: new (
                        PointsForWin: 3,
                        TotalPlaces: 20,
                        PromotionPlaces: 0,
                        RelegationPlaces: 3,
                        PlayOffPlaces: 0,
                        RelegationPlayOffPlaces: 0,
                        ReElectionPlaces: 0,
                        FailedReElectionPosition: null)),
                new(Id: 2,
                    Name: "Championship",
                    Season: new (
                        Id: 1,
                        StartYear: 2000,
                        EndYear: 2001),
                    Level: "2",
                    Comment: null,
                    Rules: new (
                        PointsForWin: 3,
                        TotalPlaces: 24,
                        PromotionPlaces: 2,
                        RelegationPlaces: 3,
                        PlayOffPlaces: 4,
                        RelegationPlayOffPlaces: 0,
                        ReElectionPlaces: 0,
                        FailedReElectionPosition: null))
            };
            
            mockRepository.VerifyAll();
            result.Should().BeEquivalentTo(competitionDtos);
            error.Should().BeNull();
        }
    }
}