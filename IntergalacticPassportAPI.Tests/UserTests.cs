using IntergalacticPassportAPI.Models;
using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Controllers;
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;

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
                Google_Id = "zarg123",
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

        var controller = new UserController(mockRepo.Object);

        var result = await controller.GetAll();

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull("because the request should return Ok when users exist");

        var actualUsers = okResult!.Value as IEnumerable<Users>;
        actualUsers.Should().NotBeNull();
        actualUsers.Should().HaveCount(3);
        actualUsers.Should().BeEquivalentTo(expectedUsers);
    }

    [Fact]
    public async Task GetUserByUserId_ReturnsOkWithUser_WhenUserWithIdExists() 
    {
        var mockRepo = new Mock<IUsersRepository>();

        var userId = "zarg123";
        var expectedUser = new Users
        {
            GoogleId = "zarg123",
            Email = "zarg@nebula7.gov",
            Name = "Zarg the Wise",
            Species = "Nebulon",
            PlanetOfOrigin = "Nebula-7",
            PrimaryLanguage = "Quantum Dialect",
            DateOfBirth = new DateTime(3100, 5, 12)
        };

        mockRepo.Setup(repo => repo.GetById(userId)).ReturnsAsync(expectedUser);

        var controller = new UserController(mockRepo.Object);

        var result = await controller.GetById(userId);

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull("because the user with id provided exists");

        var actualUser = okResult!.Value as Users;
        actualUser.Should().NotBeNull();
        actualUser.Should().BeEquivalentTo(expectedUser);
    }

    [Fact]
    public async Task GetUserByUserId_ReturnsNotFound_WhenUserWithIdDoesNotExist()
    {
        var mockRepo = new Mock<IUsersRepository>();

        var imaginaryId = "userId123!!";

        mockRepo.Setup(repo => repo.GetById(imaginaryId)).ReturnsAsync((Users?)null);

        var controller = new UserController(mockRepo.Object);

        var result = await controller.GetById(imaginaryId);

        result.Result.Should().BeOfType<NoContentResult>("because user with no ID exists");
    }

    [Fact]
    public async Task Create_ReturnsOk_WhenUserIsCreated()
    {
        var mockRepo = new Mock<IUsersRepository>();
        var newUser = new Users { GoogleId = "z123", Email = "z@galactic.com", Name = "Zarg", Species="Nebulon", DateOfBirth=new DateTime(3100, 5, 12),PlanetOfOrigin="Nebula-7",PrimaryLanguage="Quantum Dialect" };

        mockRepo.Setup(r => r.Exists(newUser)).ReturnsAsync(false);
        mockRepo.Setup(r => r.Create(newUser)).ReturnsAsync(newUser);

        var controller = new UserController(mockRepo.Object);

        var result = await controller.Create(newUser);

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(newUser);
    }

    [Fact]
    public async Task Create_ReturnsConflict_WhenUserAlreadyExists()
    {
        var mockRepo = new Mock<IUsersRepository>();
        var existingUser = new Users { GoogleId = "z123", Email = "z@galactic.com", Name = "Zarg" };

        mockRepo.Setup(r => r.Exists(existingUser)).ReturnsAsync(true);

        var controller = new UserController(mockRepo.Object);

        var result = await controller.Create(existingUser);

        var conflictResult = result.Result as ConflictObjectResult;
        conflictResult.Should().NotBeNull();
        conflictResult!.StatusCode.Should().Be(409);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
    {

        var mockRepo = new Mock<IUsersRepository>();
        var invalidUser = new Users(); // Assuming the required fields have not been entered

        var controller = new UserController(mockRepo.Object);
        controller.ModelState.AddModelError("email", "Email is required");

        var result = await controller.Create(invalidUser);

        var badRequest = result.Result as BadRequestObjectResult;
        badRequest.Should().NotBeNull();
        badRequest!.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Put_ReturnsOk_WhenUserIsUpdated()
    {
        var mockRepo = new Mock<IUsersRepository>();
        var userToUpdate = new Users
        {
            GoogleId = "earthling007",
            Email = "j.doe@earthmail.com",
            Name = "John Doe",
            Species = "Human",
            PlanetOfOrigin = "Earth",
            PrimaryLanguage = "English",
            DateOfBirth = new DateTime(1995, 2, 28)
        };

        mockRepo.Setup(r => r.Update(userToUpdate)).ReturnsAsync(userToUpdate);

        var controller = new UserController(mockRepo.Object);

        var result = await controller.Put(userToUpdate);

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(userToUpdate);
    }

    [Fact]
    public async Task Put_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var mockRepo = new Mock<IUsersRepository>();
        var userToUpdate = new Users
        {
            GoogleId = "earthling007",
            Email = "j.doe@earthmail.com",
            Name = "John Doe",
            Species = "Human",
            PlanetOfOrigin = "Earth",
            PrimaryLanguage = "English",
            DateOfBirth = new DateTime(1995, 2, 28)
        };

        mockRepo.Setup(r => r.Update(userToUpdate)).ReturnsAsync((Users?)null);

        var controller = new UserController(mockRepo.Object);

        var result = await controller.Put(userToUpdate);

        var notFoundResult = result.Result as NotFoundResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Put_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        var mockRepo = new Mock<IUsersRepository>();
        var invalidUser = new Users(); 

        var controller = new UserController(mockRepo.Object);
        controller.ModelState.AddModelError("email", "Email is required");

        var result = await controller.Put(invalidUser);

        var badRequest = result.Result as BadRequestObjectResult;
        badRequest.Should().NotBeNull();
        badRequest!.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenUserIsDeleted()
    {
        var mockRepo = new Mock<IUsersRepository>();
        var userId = "zarg123";

        mockRepo.Setup(r => r.Delete(userId)).ReturnsAsync(true);

        var controller = new UserController(mockRepo.Object);

        var result = await controller.Delete(userId);

        result.Should().BeOfType<OkResult>();
        ((OkResult)result).StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenUserDoesNotExist()
    {
        var mockRepo = new Mock<IUsersRepository>();
        var userId = "nonexistent-user";

        mockRepo.Setup(r => r.Delete(userId)).ReturnsAsync(false);

        var controller = new UserController(mockRepo.Object);

        var result = await controller.Delete(userId);

        result.Should().BeOfType<NotFoundResult>();
        ((NotFoundResult)result).StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetUserByEmail_ReturnsUser_WhenUserExists()
    {
        var mockRepo = new Mock<IUsersRepository>();
        var testEmail = "zarg@nebula7.gov";
        var expectedUser = new Users
        {
            GoogleId = "zarg123",
            Email = testEmail,
            Name = "Zarg the Wise",
            Species = "Nebulon",
            PlanetOfOrigin = "Nebula-7",
            PrimaryLanguage = "Quantum Dialect",
            DateOfBirth = new DateTime(3100, 5, 12)
        };

        mockRepo.Setup(r => r.GetUserByEmail(testEmail))
                .ReturnsAsync(expectedUser);

        var controller = new UserController(mockRepo.Object);

        var result = await controller.GetUserByEmail(testEmail);

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(expectedUser);
    }

    [Fact]
    public async Task GetUserRoles_ReturnsOkWithRoles_WhenRolesExist()
    {
        var userId = "test-user";
        var expectedRoles = new List<Roles>
    {
        new Roles { Id = 1, Role = "Officer" },
        new Roles { Id = 2, Role = "Admin" }
    };

        var mockRepo = new Mock<IUsersRepository>();
        mockRepo.Setup(r => r.GetUserRoles(userId)).ReturnsAsync(expectedRoles);

        var controller = new UserController(mockRepo.Object);

        var result = await controller.GetUserRoles(userId);
        var okResult = result.Result as OkObjectResult;

        okResult.Should().NotBeNull("because roles exist and should return Ok");
        okResult!.Value.Should().BeEquivalentTo(expectedRoles);
    }

    [Fact]
    public async Task GetUserRoles_ReturnsNoContent_WhenNoRolesExist()
    {
        var userId = "unknown-user";
        var mockRepo = new Mock<IUsersRepository>();
        mockRepo.Setup(r => r.GetUserRoles(userId)).ReturnsAsync(new List<Roles>());

        var controller = new UserController(mockRepo.Object);

        var result = await controller.GetUserRoles(userId);

        result.Result.Should().BeOfType<NoContentResult>("because no roles exist for this user and it should return NoContent.");
    }

    [Fact]
    public async Task AssignRoleToUser_ReturnsOk_WhenAssignmentSuccessful()
    {
        var mockRepo = new Mock<IUsersRepository>();
        var userId = "zarg123";
        var roleId = 1;

        mockRepo.Setup(r => r.AssignRoleToUser(userId, roleId)).ReturnsAsync(true);
        var controller = new UserController(mockRepo.Object);

        var result = await controller.AssignRoleToUser(userId, roleId);

        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task AssignRoleToUser_ReturnsConflict_WhenAssignmentFails()
    {
        var mockRepo = new Mock<IUsersRepository>();
        var userId = "zarg123";
        var roleId = 1;

        mockRepo.Setup(r => r.AssignRoleToUser(userId, roleId)).ReturnsAsync(false);
        var controller = new UserController(mockRepo.Object);

        var result = await controller.AssignRoleToUser(userId, roleId);

        result.Should().BeOfType<ConflictResult>();
    }

    [Fact]
    public async Task Exists_ReturnsTrue_WhenUserExists()
    {
        var mockRepo = new Mock<UsersRepository>(null);
        var testUser = new Users { GoogleId = "zarg123" };

        mockRepo.Setup(r => r.GetById("zarg123"))
                .ReturnsAsync(new Users { GoogleId = "zarg123" });

        mockRepo.CallBase = true;

        var result = await mockRepo.Object.Exists(testUser);

        result.Should().BeTrue("because the user exists");
    }

    [Fact]
    public async Task Exists_ReturnsFalse_WhenUserDoesNotExist()
    {
        var mockRepo = new Mock<UsersRepository>(null);
        var testUser = new Users { GoogleId = "nonexistent123" };

        mockRepo.Setup(r => r.GetById("nonexistent123"))
                .ReturnsAsync((Users?)null);

        mockRepo.CallBase = true;

        var result = await mockRepo.Object.Exists(testUser);

        result.Should().BeFalse("because the user does not exist");
    }

    [Fact]
    public async Task Exists_ThrowsException_WhenGoogleIdIsNull()
    {
        var mockRepo = new Mock<UsersRepository>(null);
        var testUser = new Users { GoogleId = null };

        mockRepo.CallBase = true;

        var act = async () => await mockRepo.Object.Exists(testUser);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("No google id");
    }

}
