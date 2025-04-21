using IntergalacticPassportAPI.Models;
using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Controllers;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace IntergalacticPassportAPI.Tests;

public class PassportControllerTests
{
    [Fact]
    public async Task GetAllPassports_ReturnsOkWithPassports_WhenPassportsExist()
    {
        // Given
        var mockConfig = new Mock<IConfiguration>();
        var mockRepo = new Mock<IPassportRepository>();
        var mockStatusRepo = new Mock<IApplicationStatusRepository>();

        var testPassports = new List<Passport>
        {
            new Passport
            {
                Id = 1,
                UserId = "zarg123",
                StatusId = 1, // e.g., Submitted
                SubmittedAt = new DateTime(4025, 3, 1, 10, 30, 0),
                ProcessedAt = null,
                OfficerId = null
            },
            new Passport
            {
                Id = 2,
                UserId = "xena456",
                StatusId = 2, // e.g., In Review
                SubmittedAt = new DateTime(4025, 2, 15, 9, 0, 0),
                ProcessedAt = null,
                OfficerId = "officer99"
            },
            new Passport
            {
                Id = 3,
                UserId = "earthling007",
                StatusId = 3, // e.g., Approved
                SubmittedAt = new DateTime(4025, 1, 20, 14, 45, 0),
                ProcessedAt = new DateTime(4025, 2, 1, 16, 0, 0),
                OfficerId = "officer42"
            },
            new Passport
            {
                Id = 4,
                UserId = "novaqueen88",
                StatusId = 4, // e.g., Rejected
                SubmittedAt = new DateTime(4025, 3, 5, 8, 15, 0),
                ProcessedAt = new DateTime(4025, 3, 7, 11, 30, 0),
                OfficerId = "officer27"
            }
        };
        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(testPassports);
        var controller = new PassportController(mockRepo.Object, mockStatusRepo.Object);

        // When
        var result = await controller.GetAll();

        // Then
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull("because the request should return Ok when passports exist");

        var actualPassports = okResult!.Value as IEnumerable<Passport>;
        actualPassports.Should().NotBeNull();
        actualPassports.Should().HaveCount(4);
        actualPassports.Should().BeEquivalentTo(testPassports);
    }

    [Fact]
    public async Task GetAllPassports_ReturnsNoContent_WhenNoPassportsExist()
    {
        // Given
        var mockConfig = new Mock<IConfiguration>();
        var mockRepo = new Mock<IPassportRepository>();
        var mockStatusRepo = new Mock<IApplicationStatusRepository>();

        // Simulate no passports in the repository
        var emptyPassports = new List<Passport>();
        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(emptyPassports);

        var controller = new PassportController(mockRepo.Object, mockStatusRepo.Object);

        // When
        var result = await controller.GetAll();

        // Then
        var noContentResult = result.Result as NoContentResult;
        noContentResult.Should().NotBeNull("because the request should return NoContent when no passports exist");
        noContentResult!.StatusCode.Should().Be(204);
    }


    [Fact]
    public async Task GetById_ReturnsOk_WhenPassportExists()
    {
        // Arrange
        var mockRepo = new Mock<IPassportRepository>();
        var mockStatusRepo = new Mock<IApplicationStatusRepository>();
        var testPassport = new Passport
        {
            Id = 1,
            UserId = "zarg123",
            StatusId = 2,
            SubmittedAt = new DateTime(4025, 3, 1),
            ProcessedAt = null,
            OfficerId = null
        };

        mockRepo.Setup(r => r.GetById("1")).ReturnsAsync(testPassport);

        var controller = new PassportController(mockRepo.Object, mockStatusRepo.Object); // Assuming PassportController inherits from BaseController<Passport>

        // Act
        var result = await controller.GetById("1");

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(testPassport);
    }

    [Fact]
    public async Task GetById_ReturnsNoContent_WhenPassportDoesNotExist()
    {
        // Arrange
        var mockRepo = new Mock<IPassportRepository>();
        var mockStatusRepo = new Mock<IApplicationStatusRepository>();
        mockRepo.Setup(r => r.GetById("99")).ReturnsAsync((Passport?)null);

        var controller = new PassportController(mockRepo.Object, mockStatusRepo.Object);

        // Act
        var result = await controller.GetById("99");

        // Assert
        var noContentResult = result.Result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task Put_ReturnsOk_WhenModelIsUpdated()
    {
        // Arrange
        var mockRepo = new Mock<IPassportRepository>();
        var controller = new PassportController(mockRepo.Object, Mock.Of<IApplicationStatusRepository>());

        var model = new Passport
        {
            Id = 1,
            UserId = "zarg123",
            StatusId = 2,
            SubmittedAt = DateTime.Now
        };

        mockRepo.Setup(r => r.Update(model)).ReturnsAsync(model);

        // Act
        var result = await controller.Put(model);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(model);
    }

    [Fact]
    public async Task Put_ReturnsNotFound_WhenModelDoesNotExist()
    {
        // Arrange
        var mockRepo = new Mock<IPassportRepository>();
        var controller = new PassportController(mockRepo.Object, Mock.Of<IApplicationStatusRepository>());

        var model = new Passport
        {
            Id = 999, // non-existent ID
            UserId = "ghostuser",
            SubmittedAt = DateTime.Now
        };

        mockRepo.Setup(r => r.Update(model)).ReturnsAsync((Passport?)null);

        // Act
        var result = await controller.Put(model);

        // Assert
        var notFoundResult = result.Result as NotFoundResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Put_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var mockRepo = new Mock<IPassportRepository>();
        var controller = new PassportController(mockRepo.Object, Mock.Of<IApplicationStatusRepository>());

        // Simulate model validation failure
        controller.ModelState.AddModelError("UserId", "UserId is required");

        var invalidModel = new Passport
        {
            Id = 3,
            SubmittedAt = DateTime.Now
        };

        // Act
        var result = await controller.Put(invalidModel);

        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenItemIsDeleted()
    {
        // Arrange
        var mockRepo = new Mock<IPassportRepository>();
        var controller = new PassportController(mockRepo.Object, Mock.Of<IApplicationStatusRepository>());

        var idToDelete = "123";

        mockRepo.Setup(r => r.Delete(idToDelete)).ReturnsAsync(true);

        // Act
        var result = await controller.Delete(idToDelete);

        // Assert
        var okResult = result as OkResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenItemDoesNotExist()
    {
        // Arrange
        var mockRepo = new Mock<IPassportRepository>();
        var controller = new PassportController(mockRepo.Object, Mock.Of<IApplicationStatusRepository>());

        var nonExistentId = "999";

        mockRepo.Setup(r => r.Delete(nonExistentId)).ReturnsAsync(false);

        // Act
        var result = await controller.Delete(nonExistentId);

        // Assert
        var notFoundResult = result as NotFoundResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetPassportApplicationById_ReturnsOkWithApplications_WhenTheyExist()
    {
        // Arrange
        var mockRepo = new Mock<IPassportRepository>();
        var controller = new PassportController(mockRepo.Object, Mock.Of<IApplicationStatusRepository>());

        var googleId = "zarg123";
        var expectedPassports = new List<Passport>
    {
        new Passport
        {
            Id = 1,
            UserId = googleId,
            SubmittedAt = new DateTime(4025, 4, 12),
            StatusId = 1,
            OfficerId = "officer42"
        },
        new Passport
        {
            Id = 2,
            UserId = googleId,
            SubmittedAt = new DateTime(4025, 3, 1),
            StatusId = 2,
            OfficerId = null
        }
    };

        mockRepo.Setup(r => r.GetPassportApplicationsByGoogleId(googleId))
                .ReturnsAsync(expectedPassports);

        // Act
        var result = await controller.GetPassportApplicationById(googleId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var actualPassports = okResult.Value as IEnumerable<Passport>;
        actualPassports.Should().NotBeNull();
        actualPassports.Should().BeEquivalentTo(expectedPassports);
    }

    [Fact]
    public async Task GetPassportApplicationById_ReturnsOkWithEmptyList_WhenNoApplicationsExist()
    {
        // Arrange
        var mockRepo = new Mock<IPassportRepository>();
        var controller = new PassportController(mockRepo.Object, Mock.Of<IApplicationStatusRepository>());

        var googleId = "ghostuser";
        var emptyList = new List<Passport>();

        mockRepo.Setup(r => r.GetPassportApplicationsByGoogleId(googleId))
                .ReturnsAsync(emptyList);

        // Act
        var result = await controller.GetPassportApplicationById(googleId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var actualPassports = okResult.Value as IEnumerable<Passport>;
        actualPassports.Should().NotBeNull();
        actualPassports.Should().BeEmpty();
    }

    [Fact]
    public async Task GetPassportApplicationByOfficerId_ReturnsOkWithPassport_WhenExists()
    {
        // Arrange
        var mockRepo = new Mock<IPassportRepository>();
        var controller = new PassportController(mockRepo.Object, Mock.Of<IApplicationStatusRepository>());

        var officerId = "officer42";
        var expectedPassport = new Passport
        {
            Id = 1,
            UserId = "zarg123",
            StatusId = 2,
            SubmittedAt = new DateTime(4025, 4, 1),
            OfficerId = officerId,
            ProcessedAt = null
        };

        mockRepo.Setup(r => r.GetPassportApplicationByOfficerId(officerId))
                .ReturnsAsync(expectedPassport);

        // Act
        var result = await controller.GetPassportApplicationByOfficerId(officerId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedPassport);
    }

    [Fact]
    public async Task GetPassportApplicationByOfficerId_ReturnsOkWithNull_WhenNoPassportAssigned()
    {
        // Arrange
        var mockRepo = new Mock<IPassportRepository>();
        var controller = new PassportController(mockRepo.Object, Mock.Of<IApplicationStatusRepository>());

        var officerId = "unassignedOfficer";

        mockRepo.Setup(r => r.GetPassportApplicationByOfficerId(officerId))
                .ReturnsAsync((Passport?)null);

        // Act
        var result = await controller.GetPassportApplicationByOfficerId(officerId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeNull();
    }


}