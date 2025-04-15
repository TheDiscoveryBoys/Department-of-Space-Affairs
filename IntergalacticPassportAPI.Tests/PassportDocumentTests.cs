using IntergalacticPassportAPI.Models;
using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Controllers;
using IntergalacticPassportportAPI.Controllers;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
namespace IntergalacticPassportAPI.Tests;

public class PassportDocumentTests
{
    [Fact]
    public async Task GetPassportDocumentById_ReturnsOk_WhenDocumentExists()
    {
        // Arrange
        var mockRepo = new Mock<IPassportDocumentRepository>();
        var controller = new PassportDocumentController(mockRepo.Object);

        string documentId = "1";
        var passportDocument = new PassportDocument
        {
            Id = 1,
            Filename = "document.pdf",
            S3Url = "https://s3.amazonaws.com/bucket/document.pdf",
            PassportApplicationId = 123
        };

        mockRepo.Setup(r => r.GetById(documentId)).ReturnsAsync(passportDocument);

        // Act
        var result = await controller.GetById(documentId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var actualDocument = okResult.Value as PassportDocument;
        actualDocument.Should().NotBeNull();
        actualDocument.Should().BeEquivalentTo(passportDocument);
    }

    [Fact]
    public async Task GetPassportDocumentById_ReturnsNoContent_WhenDocumentDoesNotExist()
    {
        // Arrange
        var mockRepo = new Mock<IPassportDocumentRepository>();
        var controller = new PassportDocumentController(mockRepo.Object);

        string documentId = "999"; // Non-existent document ID
        var passportDocument = (PassportDocument)null;

        mockRepo.Setup(r => r.GetById(documentId)).ReturnsAsync(passportDocument);

        // Act
        var result = await controller.GetById(documentId);

        // Assert
        var noContentResult = result.Result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WhenDocumentsExist()
    {
        // Arrange
        var mockRepo = new Mock<IPassportDocumentRepository>();
        var controller = new PassportDocumentController(mockRepo.Object);

        var documents = new List<PassportDocument>
    {
        new PassportDocument { Id = 1, Filename = "file1.pdf", S3Url = "url1", PassportApplicationId = 101 },
        new PassportDocument { Id = 2, Filename = "file2.pdf", S3Url = "url2", PassportApplicationId = 102 }
    };

        mockRepo.Setup(r => r.GetAll()).ReturnsAsync(documents);

        // Act
        var result = await controller.GetAll();

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var returnedDocs = okResult.Value as IEnumerable<PassportDocument>;
        returnedDocs.Should().BeEquivalentTo(documents);
    }

    [Fact]
    public async Task GetAll_ReturnsNoContent_WhenNoDocumentsExist()
    {
        // Arrange
        var mockRepo = new Mock<IPassportDocumentRepository>();
        var controller = new PassportDocumentController(mockRepo.Object);

        mockRepo.Setup(r => r.GetAll()).ReturnsAsync(new List<PassportDocument>());

        // Act
        var result = await controller.GetAll();

        // Assert
        var noContentResult = result.Result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult!.StatusCode.Should().Be(204);
    }
    [Fact]
    public async Task Put_ReturnsOk_WhenModelIsValidAndExists()
    {
        // Arrange
        var mockRepo = new Mock<IPassportDocumentRepository>();
        var controller = new PassportDocumentController(mockRepo.Object);

        var documentToUpdate = new PassportDocument
        {
            Id = 1,
            Filename = "updated.pdf",
            S3Url = "https://s3.amazonaws.com/bucket/updated.pdf",
            PassportApplicationId = 999
        };

        mockRepo.Setup(r => r.Update(documentToUpdate)).ReturnsAsync(documentToUpdate);

        // Act
        var result = await controller.Put(documentToUpdate);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);

        var returnedDoc = okResult.Value as PassportDocument;
        returnedDoc.Should().BeEquivalentTo(documentToUpdate);
    }
    [Fact]
    public async Task Put_ReturnsNotFound_WhenModelDoesNotExist()
    {
        // Arrange
        var mockRepo = new Mock<IPassportDocumentRepository>();
        var controller = new PassportDocumentController(mockRepo.Object);

        var nonExistentDoc = new PassportDocument
        {
            Id = 999,
            Filename = "missing.pdf",
            S3Url = "https://s3.amazonaws.com/bucket/missing.pdf",
            PassportApplicationId = 123
        };

        mockRepo.Setup(r => r.Update(nonExistentDoc)).ReturnsAsync((PassportDocument)null);

        // Act
        var result = await controller.Put(nonExistentDoc);

        // Assert
        var notFoundResult = result.Result as NotFoundResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
    }
    [Fact]
    public async Task Put_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var mockRepo = new Mock<IPassportDocumentRepository>();
        var controller = new PassportDocumentController(mockRepo.Object);
        controller.ModelState.AddModelError("Filename", "Required");

        var invalidDoc = new PassportDocument
        {
            Id = 1,
            // Missing Filename
            S3Url = "https://s3.amazonaws.com/bucket/file.pdf",
            PassportApplicationId = 123
        };

        // Act
        var result = await controller.Put(invalidDoc);

        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(400);
    }
    [Fact]
    public async Task Delete_ReturnsOk_WhenDeletionIsSuccessful()
    {
        // Arrange
        var mockRepo = new Mock<IPassportDocumentRepository>();
        var controller = new PassportDocumentController(mockRepo.Object);

        string idToDelete = "1";

        mockRepo.Setup(r => r.Delete(idToDelete)).ReturnsAsync(true);

        // Act
        var result = await controller.Delete(idToDelete);

        // Assert
        var okResult = result as OkResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
    }
    [Fact]
    public async Task Delete_ReturnsNotFound_WhenDocumentDoesNotExist()
    {
        // Arrange
        var mockRepo = new Mock<IPassportDocumentRepository>();
        var controller = new PassportDocumentController(mockRepo.Object);

        string nonExistentId = "999";

        mockRepo.Setup(r => r.Delete(nonExistentId)).ReturnsAsync(false);

        // Act
        var result = await controller.Delete(nonExistentId);

        // Assert
        var notFoundResult = result as NotFoundResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
    }
[Fact]
public async Task GetByPassportApplicationId_ReturnsOk_WhenDocumentsExist()
{
    // Arrange
    var mockRepo = new Mock<IPassportDocumentRepository>();
    var controller = new PassportDocumentController(mockRepo.Object);

    int applicationId = 42;
    var documents = new List<PassportDocument>
    {
        new PassportDocument
        {
            Id = 1,
            Filename = "doc1.pdf",
            S3Url = "https://s3.url/doc1.pdf",
            PassportApplicationId = applicationId
        },
        new PassportDocument
        {
            Id = 2,
            Filename = "doc2.pdf",
            S3Url = "https://s3.url/doc2.pdf",
            PassportApplicationId = applicationId
        }
    };

    mockRepo.Setup(r => r.GetByPassportApplicationIdAsync(applicationId)).ReturnsAsync(documents);

    // Act
    var result = await controller.GetByPassportApplicationId(applicationId);

    // Assert
    var okResult = result.Result as OkObjectResult;
    okResult.Should().NotBeNull();
    okResult!.StatusCode.Should().Be(200);

    var returnedDocs = okResult.Value as IEnumerable<PassportDocument>;
    returnedDocs.Should().NotBeNull();
    returnedDocs.Should().BeEquivalentTo(documents);
}
[Fact]
public async Task GetByPassportApplicationId_ReturnsNoContent_WhenNoDocumentsExist()
{
    // Arrange
    var mockRepo = new Mock<IPassportDocumentRepository>();
    var controller = new PassportDocumentController(mockRepo.Object);

    int applicationId = 999; // Assuming no documents exist for this ID
    var emptyList = new List<PassportDocument>();

    mockRepo.Setup(r => r.GetByPassportApplicationIdAsync(applicationId)).ReturnsAsync(emptyList);

    // Act
    var result = await controller.GetByPassportApplicationId(applicationId);

    // Assert
    var noContentResult = result.Result as NoContentResult;
    noContentResult.Should().NotBeNull();
    noContentResult!.StatusCode.Should().Be(204);
}


}