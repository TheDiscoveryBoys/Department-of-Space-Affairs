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
                google_id = "zarg123",
                email = "zarg@nebula7.gov",
                name = "Zarg the Wise",
                species = "Nebulon",
                planet_of_origin = "Nebula-7",
                primary_language = "Quantum Dialect",
                date_of_birth = new DateTime(3100, 5, 12)
            },
            new Users
            {
                google_id = "xena456",
                email = "xena@marsnet.space",
                name = "Xena of Mars",
                species = "Martian",
                planet_of_origin = "Mars",
                primary_language = "Martian Tongue",
                date_of_birth = new DateTime(2890, 11, 3)
            },
            new Users
            {
                google_id = "earthling007",
                email = "j.doe@earthmail.com",
                name = "John Doe",
                species = "Human",
                planet_of_origin = "Earth",
                primary_language = "English",
                date_of_birth = new DateTime(1995, 2, 28)
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
