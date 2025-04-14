using IntergalacticPassportAPI.Models;
using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Controllers;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

public class VisaControllerTests
{
    [Fact]
    public async Task GetAllVisas_ReturnsOkWithVisas_WhenVisasExist()
    {
        // Arrange
        var mockRepo = new Mock<IVisaRepository>();
        var controller = new VisaController(mockRepo.Object, Mock.Of<IStatusRepository>());

        var testVisas = new List<Visa>
        {
            new Visa
            {
                Id = 1,
                UserId = "zarg123",
                StatusId = 1,
                SubmittedAt = new DateTime(4025, 4, 1),
                ProcessedAt = null,
                OfficerId = "officer42",
                DestinationPlanet = "Earth",
                TravelReason = "Diplomatic Mission",
                StartDate = new DateTime(4025, 5, 1),
                EndDate = new DateTime(4025, 5, 10)
            },
            new Visa
            {
                Id = 2,
                UserId = "nova88",
                StatusId = 2,
                SubmittedAt = new DateTime(4025, 3, 15),
                ProcessedAt = new DateTime(4025, 3, 20),
                OfficerId = "officer17",
                DestinationPlanet = "Mars",
                TravelReason = "Scientific Research",
                StartDate = new DateTime(4025, 6, 1),
                EndDate = new DateTime(4025, 7, 1)
            }
        };

        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(testVisas);

        // Act
        var result = await controller.GetAll();

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull("because visas exist");
        okResult!.StatusCode.Should().Be(200);

        var actualVisas = okResult.Value as IEnumerable<Visa>;
        actualVisas.Should().NotBeNull();
        actualVisas.Should().HaveCount(2);
        actualVisas.Should().BeEquivalentTo(testVisas);
    }

    [Fact]
    public async Task GetAllVisas_ReturnsNoContent_WhenNoVisasExist()
    {
        // Arrange
        var mockRepo = new Mock<IVisaRepository>();
        var controller = new VisaController(mockRepo.Object, Mock.Of<IStatusRepository>());

        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Visa>());

        // Act
        var result = await controller.GetAll();

        // Assert
        var noContentResult = result.Result as NoContentResult;
        noContentResult.Should().NotBeNull("because no visas exist");
        noContentResult!.StatusCode.Should().Be(204);
    }
}
