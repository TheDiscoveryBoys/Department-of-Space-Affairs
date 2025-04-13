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
        var mockStatusRepo = new Mock<IStatusRepository>();

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
    var mockStatusRepo = new Mock<IStatusRepository>();

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
        var mockStatusRepo = new Mock<IStatusRepository>();
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
        var mockStatusRepo = new Mock<IStatusRepository>();
        mockRepo.Setup(r => r.GetById("99")).ReturnsAsync((Passport?)null);

        var controller = new PassportController(mockRepo.Object, mockStatusRepo.Object);

        // Act
        var result = await controller.GetById("99");

        // Assert
        var noContentResult = result.Result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be(204);
    }

}