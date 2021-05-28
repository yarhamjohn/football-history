using System;
using System.Collections.Generic;
using FluentAssertions;
using football.history.api.Builders;
using football.history.api.Controllers;
using football.history.api.Exceptions;
using football.history.api.Repositories.Competition;
using football.history.api.Repositories.Season;
using Moq;
using NUnit.Framework;

namespace football.history.api.Tests.Controllers
{
    [TestFixture]
    public class SeasonControllerTests
    {
        [Test]
        public void GetAllSeasons_should_return_message_for_unhandled_error()
        {
            var mockCompetitionRepository = new Mock<ICompetitionRepository>();
            var mockSeasonRepository = new Mock<ISeasonRepository>();
            mockSeasonRepository
                .Setup(x => x.GetAllSeasons())
                .Throws(new Exception("Unhandled error occurred."));

            var controller = new SeasonController(mockSeasonRepository.Object, mockCompetitionRepository.Object);
            var (result, error) = controller.GetAllSeasons();

            mockSeasonRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Something went wrong. Unhandled error occurred.");
            error!.Code.Should().Be("UNKNOWN_ERROR");
        }

        [Test]
        public void GetAllSeasons_should_return_message_for_handled_error()
        {
            var mockCompetitionRepository = new Mock<ICompetitionRepository>();
            var mockRepository = new Mock<ISeasonRepository>();
            mockRepository
                .Setup(x => x.GetAllSeasons())
                .Throws(new DataInvalidException("Repository data was invalid."));

            var controller = new SeasonController(mockRepository.Object, mockCompetitionRepository.Object);
            var (result, error) = controller.GetAllSeasons();

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Repository data was invalid.");
            error!.Code.Should().Be("DATA_INVALID");
        }

        [Test]
        public void GetAllSeasons_should_return_result()
        {
            var mockCompetitionRepository = new Mock<ICompetitionRepository>();
            var mockRepository = new Mock<ISeasonRepository>();
            var seasonModels = new List<SeasonModel>
            {
                new(1, 2000, 2001),
                new(2, 2001, 2002)
            };

            mockRepository
                .Setup(x => x.GetAllSeasons())
                .Returns(seasonModels);

            var controller = new SeasonController(mockRepository.Object, mockCompetitionRepository.Object);
            var (result, error) = controller.GetAllSeasons();

            var seasonDtos = new List<SeasonDto>
            {
                new(1, 2000, 2001),
                new(2, 2001, 2002)
            };
            
            mockRepository.VerifyAll();
            result.Should().BeEquivalentTo(seasonDtos);
            error.Should().BeNull();
        }

        [Test]
        public void GetSeason_should_return_message_for_unhandled_error()
        {
            var mockCompetitionRepository = new Mock<ICompetitionRepository>();
            var mockRepository = new Mock<ISeasonRepository>();
            mockRepository
                .Setup(x => x.GetSeason(1))
                .Throws(new Exception("Unhandled error occurred."));

            var controller = new SeasonController(mockRepository.Object, mockCompetitionRepository.Object);
            var (result, error) = controller.GetSeason(1);

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Something went wrong. Unhandled error occurred.");
            error!.Code.Should().Be("UNKNOWN_ERROR");
        }

        [Test]
        public void GetSeason_should_return_message_for_handled_error()
        {
            var mockCompetitionRepository = new Mock<ICompetitionRepository>();
            var mockRepository = new Mock<ISeasonRepository>();
            mockRepository
                .Setup(x => x.GetSeason(1))
                .Throws(new DataInvalidException("Repository data was invalid."));

            var controller = new SeasonController(mockRepository.Object, mockCompetitionRepository.Object);
            var (result, error) = controller.GetSeason(1);

            mockRepository.VerifyAll();
            result.Should().BeNull();
            error.Should().NotBeNull();
            error!.Message.Should().Be("Repository data was invalid.");
            error!.Code.Should().Be("DATA_INVALID");
        }

        [Test]
        public void GetSeason_should_return_result()
        {
            var mockCompetitionRepository = new Mock<ICompetitionRepository>();
            var mockRepository = new Mock<ISeasonRepository>();
            var seasonModel = new SeasonModel(1, 2000, 2001);

            mockRepository
                .Setup(x => x.GetSeason(1))
                .Returns(seasonModel);

            var controller = new SeasonController(mockRepository.Object, mockCompetitionRepository.Object);
            var (result, error) = controller.GetSeason(1);

            var seasonDto = new SeasonDto(1, 2000, 2001);

            mockRepository.VerifyAll();
            result.Should().Be(seasonDto);
            error.Should().BeNull();
        }
    }
}