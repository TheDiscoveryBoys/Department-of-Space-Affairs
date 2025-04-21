using IntergalacticPassportAPI.Models;
using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Controllers;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace IntergalacticPassportAPI.Tests;

public class StatusTests
{
    [Fact]
    public async Task GetById_ReturnsOk_WhenStatusExists()
    {
        // Arrange
        var mockRepo = new Mock<IApplicationStatusRepository>();
        var controller = new ApplicationStatusController(mockRepo.Object);

        string statusId = "1";
        var expectedStatus = new ApplicationStatus
        {
            Id = 1,
            Name = "APPROVED"
        };

        mockRepo.Setup(r => r.GetById(statusId)).ReturnsAsync(expectedStatus);

        // Act
        var result = await controller.GetById(statusId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var returnedStatus = okResult.Value as ApplicationStatus;
        returnedStatus.Should().NotBeNull();
        returnedStatus.Should().BeEquivalentTo(expectedStatus);
    }
    [Fact]
    public async Task GetById_ReturnsNoContent_WhenStatusDoesNotExist()
    {
        // Arrange
        var mockRepo = new Mock<IApplicationStatusRepository>();
        var controller = new ApplicationStatusController(mockRepo.Object);

        string statusId = "999";
        mockRepo.Setup(r => r.GetById(statusId)).ReturnsAsync((ApplicationStatus?)null);

        // Act
        var result = await controller.GetById(statusId);

        // Assert
        var noContentResult = result.Result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WhenStatusesExist()
    {
        // Arrange
        var mockRepo = new Mock<IApplicationStatusRepository>();
        var controller = new ApplicationStatusController(mockRepo.Object);

        var statuses = new List<ApplicationStatus>
    {
        new ApplicationStatus { Id = 1, Name = "PENDING"},
        new ApplicationStatus { Id = 2, Name = "APPROVED"}
    };

        mockRepo.Setup(r => r.GetAll()).ReturnsAsync(statuses);

        // Act
        var result = await controller.GetAll();

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var returnedStatuses = okResult.Value as IEnumerable<ApplicationStatus>;
        returnedStatuses.Should().NotBeNull();
        returnedStatuses.Should().BeEquivalentTo(statuses);
    }
    [Fact]
    public async Task GetAll_ReturnsNoContent_WhenNoStatusesExist()
    {
        // Arrange
        var mockRepo = new Mock<IApplicationStatusRepository>();
        var controller = new ApplicationStatusController(mockRepo.Object);

        mockRepo.Setup(r => r.GetAll()).ReturnsAsync(new List<ApplicationStatus>());

        // Act
        var result = await controller.GetAll();

        // Assert
        var noContentResult = result.Result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be(204);
    }
    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var mockRepo = new Mock<IApplicationStatusRepository>();
        var controller = new ApplicationStatusController(mockRepo.Object);
        controller.ModelState.AddModelError("Name", "Required");

        var invalidStatus = new ApplicationStatus { };

        // Act
        var result = await controller.Create(invalidStatus);

        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(400);
    }
    [Fact]
    public async Task Put_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var mockRepo = new Mock<IApplicationStatusRepository>();
        var controller = new ApplicationStatusController(mockRepo.Object);
        controller.ModelState.AddModelError("Name", "Required");

        var invalidStatus = new ApplicationStatus { Id = 1 };

        // Act
        var result = await controller.Put(invalidStatus);

        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(400);
    }
    [Fact]
    public async Task Delete_ReturnsNotFound_WhenDeletionFails()
    {
        // Arrange
        var mockRepo = new Mock<IApplicationStatusRepository>();
        var controller = new ApplicationStatusController(mockRepo.Object);

        var id = "999"; // Assume this ID doesn't exist
        mockRepo.Setup(r => r.Delete(id)).ReturnsAsync(false);

        // Act
        var result = await controller.Delete(id);

        // Assert
        var notFoundResult = result as NotFoundResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
    }

}