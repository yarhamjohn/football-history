using System;
using System.Collections.Generic;
using FluentAssertions;
using football.history.api.Builders;
using football.history.api.Controllers;
using football.history.api.Dtos;
using football.history.api.Exceptions;
using football.history.api.Repositories.Competition;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Controllers
{
    [TestFixture]
    public class LeagueTableControllerTests
    {
        [Test]
        public void GetLeagueTable_given_competitionId_should_return_message_for_unhandled_error()
        {
            var mockBuilder = new Mock<ILeagueTableBuilder>();
            var mockRepository = new Mock<ICompetitionRepository>();
            mockRepository
                .Setup(x => x.GetCompetition(1))
                .Throws(new Exception("Unhandled error occurred."));

            var controller = new LeagueTableController(mockRepository.Object, mockBuilder.Object);
            var (result, error) = controller.GetLeagueTable(1);

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Something went wrong. Unhandled error occurred.");
            error!.Code.Should().Be("UNKNOWN_ERROR");
        }

        [Test]
        public void GetLeagueTable_given_competitionId_should_return_message_for_handled_error()
        {
            var mockBuilder = new Mock<ILeagueTableBuilder>();
            var mockRepository = new Mock<ICompetitionRepository>();
            mockRepository
                .Setup(x => x.GetCompetition(1))
                .Throws(new DataInvalidException("Repository data was invalid."));

            var controller = new LeagueTableController(mockRepository.Object, mockBuilder.Object);
            var (result, error) = controller.GetLeagueTable(1);

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Repository data was invalid.");
            error!.Code.Should().Be("DATA_INVALID");
        }

        [Test]
        public void GetLeagueTable_given_competitionId_should_return_result()
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

            var mockBuilder = new Mock<ILeagueTableBuilder>();
            var leagueTable = new LeagueTable(new List<LeagueTableRowDto>());
            mockBuilder
                .Setup(x => x.BuildFullLeagueTable(competitionModel))
                .Returns(leagueTable);

            var controller = new LeagueTableController(mockRepository.Object, mockBuilder.Object);
            var (result, error) = controller.GetLeagueTable(1);

            var expectedCompetitionDto = new CompetitionDto(Id: 1,
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
            
            var teamDtos = new LeagueTableDto(
                Table: leagueTable.GetRows(), 
                Competition: expectedCompetitionDto);
            
            mockBuilder.VerifyAll();
            mockRepository.VerifyAll();
            result!.Table.Should().BeEquivalentTo(teamDtos.Table);
            result!.Competition.Should().BeEquivalentTo(expectedCompetitionDto);
            error.Should().BeNull();
        }
        
        [Test]
        public void GetLeagueTable_given_seasonId_and_teamId_should_return_message_for_unhandled_error()
        {
            var mockBuilder = new Mock<ILeagueTableBuilder>();
            var mockRepository = new Mock<ICompetitionRepository>();
            mockRepository
                .Setup(x => x.GetCompetitionForSeasonAndTeam(1, 1L))
                .Throws(new Exception("Unhandled error occurred."));

            var controller = new LeagueTableController(mockRepository.Object, mockBuilder.Object);
            var (result, error) = controller.GetLeagueTable(1, 1);

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Something went wrong. Unhandled error occurred.");
            error!.Code.Should().Be("UNKNOWN_ERROR");
        }

        [Test]
        public void GetLeagueTable_given_seasonId_and_teamId_should_return_message_for_handled_error()
        {
            var mockBuilder = new Mock<ILeagueTableBuilder>();
            var mockRepository = new Mock<ICompetitionRepository>();
            mockRepository
                .Setup(x => x.GetCompetitionForSeasonAndTeam(1, 1L))
                .Throws(new DataInvalidException("Repository data was invalid."));

            var controller = new LeagueTableController(mockRepository.Object, mockBuilder.Object);
            var (result, error) = controller.GetLeagueTable(1, 1);

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Repository data was invalid.");
            error!.Code.Should().Be("DATA_INVALID");
        }
        
        [Test]
        public void GetLeagueTable_given_seasonId_and_teamId_should_return_result()
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
                .Setup(x => x.GetCompetitionForSeasonAndTeam(1, 1L))
                .Returns(competitionModel);

            var mockBuilder = new Mock<ILeagueTableBuilder>();
            var leagueTable = new LeagueTable(new List<LeagueTableRowDto>());
            mockBuilder
                .Setup(x => x.BuildFullLeagueTable(competitionModel))
                .Returns(leagueTable);

            var controller = new LeagueTableController(mockRepository.Object, mockBuilder.Object);
            var (result, error) = controller.GetLeagueTable(1, 1);

            var expectedCompetitionDto = new CompetitionDto(
                Id: 1,
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
            var teamDtos = new LeagueTableDto(
                Table: leagueTable.GetRows(), 
                Competition: expectedCompetitionDto);
            
            mockBuilder.VerifyAll();
            mockRepository.VerifyAll();
            result!.Table.Should().BeEquivalentTo(teamDtos.Table);
            result!.Competition.Should().BeEquivalentTo(teamDtos.Competition);
            error.Should().BeNull();
        }
    }}