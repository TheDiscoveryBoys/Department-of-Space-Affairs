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
    var mockRepo = new Mock<IStatusRepository>();
    var controller = new StatusController(mockRepo.Object);

    string statusId = "1";
    var expectedStatus = new Status
    {
        Id = 1,
        Name = "APPROVED",
        Reason = "All good"
    };

    mockRepo.Setup(r => r.GetById(statusId)).ReturnsAsync(expectedStatus);

    // Act
    var result = await controller.GetById(statusId);

    // Assert
    var okResult = result.Result as OkObjectResult;
    okResult.Should().NotBeNull();
    okResult!.StatusCode.Should().Be(200);

    var returnedStatus = okResult.Value as Status;
    returnedStatus.Should().NotBeNull();
    returnedStatus.Should().BeEquivalentTo(expectedStatus);
}
[Fact]
public async Task GetById_ReturnsNoContent_WhenStatusDoesNotExist()
{
    // Arrange
    var mockRepo = new Mock<IStatusRepository>();
    var controller = new StatusController(mockRepo.Object);

    string statusId = "999";
    mockRepo.Setup(r => r.GetById(statusId)).ReturnsAsync((Status?)null);

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
    var mockRepo = new Mock<IStatusRepository>();
    var controller = new StatusController(mockRepo.Object);

    var statuses = new List<Status>
    {
        new Status { Id = 1, Name = "PENDING", Reason = null },
        new Status { Id = 2, Name = "APPROVED", Reason = "All documents verified" }
    };

    mockRepo.Setup(r => r.GetAll()).ReturnsAsync(statuses);

    // Act
    var result = await controller.GetAll();

    // Assert
    var okResult = result.Result as OkObjectResult;
    okResult.Should().NotBeNull();
    okResult!.StatusCode.Should().Be(200);

    var returnedStatuses = okResult.Value as IEnumerable<Status>;
    returnedStatuses.Should().NotBeNull();
    returnedStatuses.Should().BeEquivalentTo(statuses);
}
[Fact]
public async Task GetAll_ReturnsNoContent_WhenNoStatusesExist()
{
    // Arrange
    var mockRepo = new Mock<IStatusRepository>();
    var controller = new StatusController(mockRepo.Object);

    mockRepo.Setup(r => r.GetAll()).ReturnsAsync(new List<Status>());

    // Act
    var result = await controller.GetAll();

    // Assert
    var noContentResult = result.Result as NoContentResult;
    noContentResult.Should().NotBeNull();
    noContentResult!.StatusCode.Should().Be(204);
}
[Fact]
public async Task Create_ReturnsOk_WhenModelIsValidAndDoesNotExist()
{
    // Arrange
    var mockRepo = new Mock<IStatusRepository>();
    var controller = new StatusController(mockRepo.Object);

    var newStatus = new Status("REVIEWED", "Reviewed by officer");

    mockRepo.Setup(r => r.Exists(newStatus)).ReturnsAsync(false);
    mockRepo.Setup(r => r.Create(newStatus)).ReturnsAsync(newStatus);

    // Act
    var result = await controller.Create(newStatus);

    // Assert
    var okResult = result.Result as OkObjectResult;
    okResult.Should().NotBeNull();
    okResult!.StatusCode.Should().Be(200);
    okResult.Value.Should().BeEquivalentTo(newStatus);
}
[Fact]
public async Task Create_ReturnsConflict_WhenModelAlreadyExists()
{
    // Arrange
    var mockRepo = new Mock<IStatusRepository>();
    var controller = new StatusController(mockRepo.Object);

    var existingStatus = new Status("PENDING", null);

    mockRepo.Setup(r => r.Exists(existingStatus)).ReturnsAsync(true);

    // Act
    var result = await controller.Create(existingStatus);

    // Assert
    var conflictResult = result.Result as ConflictObjectResult;
    conflictResult.Should().NotBeNull();
    conflictResult!.StatusCode.Should().Be(409);
    conflictResult.Value.Should().Be($"This {existingStatus.GetType().Name} already exists.");
}
[Fact]
public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
{
    // Arrange
    var mockRepo = new Mock<IStatusRepository>();
    var controller = new StatusController(mockRepo.Object);
    controller.ModelState.AddModelError("Name", "Required");

    var invalidStatus = new Status { Reason = "Missing Name" };

    // Act
    var result = await controller.Create(invalidStatus);

    // Assert
    var badRequestResult = result.Result as BadRequestObjectResult;
    badRequestResult.Should().NotBeNull();
    badRequestResult!.StatusCode.Should().Be(400);
}
[Fact]
public async Task Put_ReturnsOk_WhenModelIsValidAndUpdated()
{
    // Arrange
    var mockRepo = new Mock<IStatusRepository>();
    var controller = new StatusController(mockRepo.Object);

    var statusToUpdate = new Status(1, "APPROVED", "Approved after review");

    mockRepo.Setup(r => r.Update(statusToUpdate)).ReturnsAsync(statusToUpdate);

    // Act
    var result = await controller.Put(statusToUpdate);

    // Assert
    var okResult = result.Result as OkObjectResult;
    okResult.Should().NotBeNull();
    okResult!.StatusCode.Should().Be(200);
    okResult.Value.Should().BeEquivalentTo(statusToUpdate);
}
[Fact]
public async Task Put_ReturnsBadRequest_WhenModelStateIsInvalid()
{
    // Arrange
    var mockRepo = new Mock<IStatusRepository>();
    var controller = new StatusController(mockRepo.Object);
    controller.ModelState.AddModelError("Name", "Required");

    var invalidStatus = new Status { Id = 1, Reason = "No name" };

    // Act
    var result = await controller.Put(invalidStatus);

    // Assert
    var badRequestResult = result.Result as BadRequestObjectResult;
    badRequestResult.Should().NotBeNull();
    badRequestResult!.StatusCode.Should().Be(400);
}
[Fact]
public async Task Put_ReturnsNotFound_WhenUpdateReturnsNull()
{
    // Arrange
    var mockRepo = new Mock<IStatusRepository>();
    var controller = new StatusController(mockRepo.Object);

    var statusToUpdate = new Status(999, "REJECTED", "Non-existent");

    mockRepo.Setup(r => r.Update(statusToUpdate)).ReturnsAsync((Status)null);

    // Act
    var result = await controller.Put(statusToUpdate);

    // Assert
    var notFoundResult = result.Result as NotFoundResult;
    notFoundResult.Should().NotBeNull();
    notFoundResult!.StatusCode.Should().Be(404);
}
[Fact]
public async Task Delete_ReturnsOk_WhenDeletionIsSuccessful()
{
    // Arrange
    var mockRepo = new Mock<IStatusRepository>();
    var controller = new StatusController(mockRepo.Object);

    var id = "1";
    mockRepo.Setup(r => r.Delete(id)).ReturnsAsync(true);

    // Act
    var result = await controller.Delete(id);

    // Assert
    var okResult = result as OkResult;
    okResult.Should().NotBeNull();
    okResult!.StatusCode.Should().Be(200);
}
[Fact]
public async Task Delete_ReturnsNotFound_WhenDeletionFails()
{
    // Arrange
    var mockRepo = new Mock<IStatusRepository>();
    var controller = new StatusController(mockRepo.Object);

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