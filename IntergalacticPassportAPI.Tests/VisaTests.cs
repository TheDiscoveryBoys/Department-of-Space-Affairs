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

    [Fact]
    public async Task GetVisaById_ReturnsOk_WhenVisaExists()
    {
        // Arrange
        var mockRepo = new Mock<IVisaRepository>();
        var controller = new VisaController(mockRepo.Object, Mock.Of<IStatusRepository>());

        var testVisa = new Visa
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
        };

        mockRepo.Setup(repo => repo.GetById("1")).ReturnsAsync(testVisa);

        // Act
        var result = await controller.GetById("1");

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var actualVisa = okResult.Value as Visa;
        actualVisa.Should().NotBeNull();
        actualVisa.Should().BeEquivalentTo(testVisa);
    }

    [Fact]
    public async Task GetVisaById_ReturnsNoContent_WhenVisaDoesNotExist()
    {
        // Arrange
        var mockRepo = new Mock<IVisaRepository>();
        var controller = new VisaController(mockRepo.Object, Mock.Of<IStatusRepository>());

        mockRepo.Setup(repo => repo.GetById("999")).ReturnsAsync((Visa?)null);

        // Act
        var result = await controller.GetById("999");

        // Assert
        var noContentResult = result.Result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task CreateVisa_ReturnsOk_WhenNoConflictsExist()
    {
        // Arrange
        var mockRepo = new Mock<IVisaRepository>();
        var mockStatusRepo = new Mock<IStatusRepository>();
        var controller = new VisaController(mockRepo.Object, mockStatusRepo.Object);

        var newVisa = new Visa
        {
            UserId = "test_user",
            DestinationPlanet = "Mars",
            TravelReason = "Research",
            StartDate = new DateTime(4025, 6, 1),
            EndDate = new DateTime(4025, 6, 30),
            SubmittedAt = DateTime.UtcNow
        };

        mockRepo.Setup(r => r.GetVisaApplicationsByGoogleId("test_user"))
                .ReturnsAsync(new List<Visa>()); // No existing visas

        mockStatusRepo.Setup(s => s.Create(It.IsAny<Status>()))
                      .ReturnsAsync(new Status { Id = 1, Name = "PENDING" });

        mockRepo.Setup(r => r.Create(It.IsAny<Visa>()))
                .ReturnsAsync(newVisa);

        // Act
        var result = await controller.Create(newVisa);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(newVisa);
    }

    [Fact]
    public async Task CreateVisa_ReturnsConflict_WhenPendingVisaExists()
    {
        // Arrange
        var mockRepo = new Mock<IVisaRepository>();
        var mockStatusRepo = new Mock<IStatusRepository>();
        var controller = new VisaController(mockRepo.Object, mockStatusRepo.Object);

        var newVisa = new Visa
        {
            UserId = "test_user",
            DestinationPlanet = "Jupiter",
            StartDate = new DateTime(4025, 7, 1),
            EndDate = new DateTime(4025, 7, 15),
            SubmittedAt = DateTime.UtcNow
        };

        var existingVisa = new Visa
        {
            DestinationPlanet = "Jupiter",
            StatusId = 2,
            StartDate = new DateTime(4025, 7, 1),
            EndDate = new DateTime(4025, 7, 15)
        };

        mockRepo.Setup(r => r.GetVisaApplicationsByGoogleId("test_user"))
                .ReturnsAsync(new List<Visa> { existingVisa });

        mockStatusRepo.Setup(s => s.GetById(2))
                      .ReturnsAsync(new Status { Id = 2, Name = "PENDING" });

        // Act
        var result = await controller.Create(newVisa);

        // Assert
        var conflictResult = result.Result as ConflictObjectResult;
        conflictResult.Should().NotBeNull();
        conflictResult!.StatusCode.Should().Be(409);
        conflictResult.Value.Should().Be("Could not create VISA. A pending VISA already exists for this planet.");
    }

    [Fact]
    public async Task CreateVisa_ReturnsConflict_WhenApprovedVisaExistsWithSameDetails()
    {
        // Arrange
        var mockRepo = new Mock<IVisaRepository>();
        var mockStatusRepo = new Mock<IStatusRepository>();
        var controller = new VisaController(mockRepo.Object, mockStatusRepo.Object);

        var newVisa = new Visa
        {
            UserId = "test_user",
            DestinationPlanet = "Venus",
            StartDate = new DateTime(4025, 5, 1),
            EndDate = new DateTime(4025, 5, 10),
            SubmittedAt = DateTime.UtcNow
        };

        var existingVisa = new Visa
        {
            DestinationPlanet = "Venus",
            StatusId = 3,
            StartDate = new DateTime(4025, 5, 1),
            EndDate = new DateTime(4025, 5, 10)
        };

        mockRepo.Setup(r => r.GetVisaApplicationsByGoogleId("test_user"))
                .ReturnsAsync(new List<Visa> { existingVisa });

        mockStatusRepo.Setup(s => s.GetById(3))
                      .ReturnsAsync(new Status { Id = 3, Name = "APPROVED" });

        // Act
        var result = await controller.Create(newVisa);

        // Assert
        var conflictResult = result.Result as ConflictObjectResult;
        conflictResult.Should().NotBeNull();
        conflictResult!.StatusCode.Should().Be(409);
        conflictResult.Value.Should().Be("Could not create VISA. An approved VISA already exists for this planet for this time.");
    }

    [Fact]
    public async Task Put_ReturnsOk_WhenModelIsValidAndUpdatedSuccessfully()
    {
        // Arrange
        var mockRepo = new Mock<IVisaRepository>();
        var controller = new VisaController(mockRepo.Object, Mock.Of<IStatusRepository>());

        var existingVisa = new Visa
        {
            Id = 1,
            UserId = "user123",
            DestinationPlanet = "Neptune",
            TravelReason = "Diplomacy",
            StartDate = DateTime.UtcNow.AddDays(10),
            EndDate = DateTime.UtcNow.AddDays(20),
            SubmittedAt = DateTime.UtcNow
        };

        mockRepo.Setup(r => r.Update(existingVisa))
                .ReturnsAsync(existingVisa);

        // Act
        var result = await controller.Put(existingVisa);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(existingVisa);
    }

    [Fact]
    public async Task Put_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var mockRepo = new Mock<IVisaRepository>();
        var controller = new VisaController(mockRepo.Object, Mock.Of<IStatusRepository>());
        controller.ModelState.AddModelError("DestinationPlanet", "Required");

        var invalidVisa = new Visa
        {
            Id = 2,
            UserId = "user456",
            TravelReason = "Adventure",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(5),
            SubmittedAt = DateTime.UtcNow
        };

        // Act
        var result = await controller.Put(invalidVisa);

        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Put_ReturnsNotFound_WhenUpdateFails()
    {
        // Arrange
        var mockRepo = new Mock<IVisaRepository>();
        var controller = new VisaController(mockRepo.Object, Mock.Of<IStatusRepository>());

        var visaToUpdate = new Visa
        {
            Id = 3,
            UserId = "ghost_user",
            DestinationPlanet = "Mercury",
            TravelReason = "Training",
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(10),
            SubmittedAt = DateTime.UtcNow
        };

        mockRepo.Setup(r => r.Update(visaToUpdate))
                .ReturnsAsync((Visa?)null); // Simulate failed update

        // Act
        var result = await controller.Put(visaToUpdate);

        // Assert
        var notFoundResult = result.Result as NotFoundResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenDeletionIsSuccessful()
    {
        // Arrange
        var mockRepo = new Mock<IVisaRepository>();
        var controller = new VisaController(mockRepo.Object, Mock.Of<IStatusRepository>());

        string visaId = "123";

        mockRepo.Setup(r => r.Delete(visaId)).ReturnsAsync(true);

        // Act
        var result = await controller.Delete(visaId);

        // Assert
        var okResult = result as OkResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenVisaDoesNotExist()
    {
        // Arrange
        var mockRepo = new Mock<IVisaRepository>();
        var controller = new VisaController(mockRepo.Object, Mock.Of<IStatusRepository>());

        string nonExistentVisaId = "999";

        mockRepo.Setup(r => r.Delete(nonExistentVisaId)).ReturnsAsync(false);

        // Act
        var result = await controller.Delete(nonExistentVisaId);

        // Assert
        var notFoundResult = result as NotFoundResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetVisaApplicationById_ReturnsOk_WhenVisaApplicationsExist()
    {
        // Arrange
        var mockRepo = new Mock<IVisaRepository>();
        var controller = new VisaController(mockRepo.Object, Mock.Of<IStatusRepository>());

        string googleId = "xena456";
        var visaApplications = new List<Visa>
    {
        new Visa
        {
            Id = 1,
            UserId = googleId,
            DestinationPlanet = "Mars",
            TravelReason = "Business",
            StartDate = DateTime.UtcNow.AddDays(10),
            EndDate = DateTime.UtcNow.AddDays(20),
            SubmittedAt = DateTime.UtcNow
        },
        new Visa
        {
            Id = 2,
            UserId = googleId,
            DestinationPlanet = "Venus",
            TravelReason = "Vacation",
            StartDate = DateTime.UtcNow.AddDays(5),
            EndDate = DateTime.UtcNow.AddDays(15),
            SubmittedAt = DateTime.UtcNow
        }
    };

        mockRepo.Setup(r => r.GetVisaApplicationsByGoogleId(googleId)).ReturnsAsync(visaApplications);

        // Act
        var result = await controller.GetVisaApplicationById(googleId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var actualVisaApplications = okResult.Value as IEnumerable<Visa>;
        actualVisaApplications.Should().NotBeNull();
        actualVisaApplications.Should().HaveCount(2);
        actualVisaApplications.Should().BeEquivalentTo(visaApplications);
    }

    [Fact]
    public async Task GetVisaApplicationByOfficerId_ReturnsOk_WhenVisaApplicationExists()
    {
        // Arrange
        var mockRepo = new Mock<IVisaRepository>();
        var controller = new VisaController(mockRepo.Object, Mock.Of<IStatusRepository>());

        string officerId = "officer123";
        var visaApplication = new Visa
        {
            Id = 1,
            UserId = "xena456",
            OfficerId = officerId,
            DestinationPlanet = "Mars",
            TravelReason = "Business",
            StartDate = DateTime.UtcNow.AddDays(10),
            EndDate = DateTime.UtcNow.AddDays(20),
            SubmittedAt = DateTime.UtcNow
        };

        mockRepo.Setup(r => r.GetVisaApplicationByOfficerId(officerId)).ReturnsAsync(visaApplication);

        // Act
        var result = await controller.GetVisaApplicationByOfficerId(officerId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var actualVisaApplication = okResult.Value as Visa;
        actualVisaApplication.Should().NotBeNull();
        actualVisaApplication.Should().BeEquivalentTo(visaApplication);
    }


}
