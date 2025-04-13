using IntergalacticPassportAPI.Models;
using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Controllers;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace IntergalacticPassportAPI.Tests;

public class UserControllerTests
{
    [Fact]
    public async Task GetAllUsers_ReturnsOkWithUsers_WhenUsersExist()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        var mockRepo = new Mock<IUsersRepository>();

        var expectedUsers = new List<Users>
        {
            new Users
            {
                GoogleId = "zarg123",
                Email = "zarg@nebula7.gov",
                Name = "Zarg the Wise",
                Species = "Nebulon",
                PlanetOfOrigin = "Nebula-7",
                PrimaryLanguage = "Quantum Dialect",
                DateOfBirth = new DateTime(3100, 5, 12)
            },
            new Users
            {
                GoogleId = "xena456",
                Email = "xena@marsnet.space",
                Name = "Xena of Mars",
                Species = "Martian",
                PlanetOfOrigin = "Mars",
                PrimaryLanguage = "Martian Tongue",
                DateOfBirth = new DateTime(2890, 11, 3)
            },
            new Users
            {
                GoogleId = "earthling007",
                Email = "j.doe@earthmail.com",
                Name = "John Doe",
                Species = "Human",
                PlanetOfOrigin = "Earth",
                PrimaryLanguage = "English",
                DateOfBirth = new DateTime(1995, 2, 28)
            }

        };

        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(expectedUsers);

        // We pass the mock IUsersRepository to the UserController
        var controller = new UserController(mockRepo.Object);

        // Act
        var result = await controller.GetAll();

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull("because the request should return Ok when users exist");

        var actualUsers = okResult!.Value as IEnumerable<Users>;
        actualUsers.Should().NotBeNull();
        actualUsers.Should().HaveCount(3);
        actualUsers.Should().BeEquivalentTo(expectedUsers);
    }
}
