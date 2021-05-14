using System;
using System.Collections.Generic;
using FluentAssertions;
using football.history.api.Builders.Team;
using football.history.api.Controllers;
using football.history.api.Exceptions;
using football.history.api.Repositories.Team;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Controllers
{
    [TestFixture]
    public class TeamControllerTests
    {
        [Test]
        public void GetAllTeams_should_return_message_for_unhandled_error()
        {
            var mockRepository = new Mock<ITeamRepository>();
            mockRepository
                .Setup(x => x.GetAllTeams())
                .Throws(new Exception("Unhandled error occurred."));

            var controller = new TeamController(mockRepository.Object);
            var (result, error) = controller.GetAllTeams();

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Something went wrong. Unhandled error occurred.");
            error!.Code.Should().Be("UNKNOWN_ERROR");
        }

        [Test]
        public void GetAllTeams_should_return_message_for_handled_error()
        {
            var mockRepository = new Mock<ITeamRepository>();
            mockRepository
                .Setup(x => x.GetAllTeams())
                .Throws(new DataInvalidException("Repository data was invalid."));

            var controller = new TeamController(mockRepository.Object);
            var (result, error) = controller.GetAllTeams();

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Repository data was invalid.");
            error!.Code.Should().Be("DATA_INVALID");
        }

        [Test]
        public void GetAllTeams_should_return_result()
        {
            var mockRepository = new Mock<ITeamRepository>();
            var teamModels = new List<TeamModel>
            {
                new(1, "Norwich City", "NOR", Notes: null),
                new(2, "Newcastle United", "NEW", Notes: null)
            };

            mockRepository
                .Setup(x => x.GetAllTeams())
                .Returns(teamModels);

            var controller = new TeamController(mockRepository.Object);
            var (result, error) = controller.GetAllTeams();

            var teamDtos = new List<TeamDto>
            {
                new(1, "Norwich City", "NOR", Notes: null),
                new(2, "Newcastle United", "NEW", Notes: null)
            };
            
            mockRepository.VerifyAll();
            result.Should().BeEquivalentTo(teamDtos);
            error.Should().BeNull();
        }

        [Test]
        public void GetTeam_should_return_message_for_unhandled_error()
        {
            var mockRepository = new Mock<ITeamRepository>();
            mockRepository
                .Setup(x => x.GetTeam(1))
                .Throws(new Exception("Unhandled error occurred."));

            var controller = new TeamController(mockRepository.Object);
            var (result, error) = controller.GetTeam(1);

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Something went wrong. Unhandled error occurred.");
            error!.Code.Should().Be("UNKNOWN_ERROR");
        }

        [Test]
        public void GetTeam_should_return_message_for_handled_error()
        {
            var mockRepository = new Mock<ITeamRepository>();
            mockRepository
                .Setup(x => x.GetTeam(1))
                .Throws(new DataInvalidException("Repository data was invalid."));

            var controller = new TeamController(mockRepository.Object);
            var (result, error) = controller.GetTeam(1);

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Repository data was invalid.");
            error!.Code.Should().Be("DATA_INVALID");
        }

        [Test]
        public void GetTeam_should_return_result()
        {
            var mockRepository = new Mock<ITeamRepository>();
            var teamModel = new TeamModel(1, "Norwich City", "NOR", Notes: null);

            mockRepository
                .Setup(x => x.GetTeam(1))
                .Returns(teamModel);

            var controller = new TeamController(mockRepository.Object);
            var (result, error) = controller.GetTeam(1);
            
            var teamDto = new TeamDto(1, "Norwich City", "NOR", Notes: null);
            
            mockRepository.VerifyAll();
            result.Should().Be(teamDto);
            error.Should().BeNull();
        }
    }}