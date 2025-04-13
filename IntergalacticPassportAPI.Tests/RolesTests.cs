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

public class RolesControllerTests
{
    [Fact]
    public async Task GetAllRoles_ReturnsOkWithRoles_WhenRolesExist()
    {
        var mockRepo = new Mock<IRolesRepository>();

        var expectedRoles = new List<Roles>
        {
            new Roles
            {
                Id = 1,
                Role = "APPLICANT"
            },
            new Roles
            {
                Id = 2,
                Role = "OFFICER"
            },
            new Roles
            {
                Id = 3,
                Role = "ADMIN"
            },
        };

        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(expectedRoles);

        var controller = new RolesController(mockRepo.Object);

        var result = await controller.GetAll();

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull("because the request should return Ok when roles exist");

        var actualRoles = okResult!.Value as IEnumerable<Roles>;
        actualRoles.Should().NotBeNull();
        actualRoles.Should().HaveCount(3);
        actualRoles.Should().BeEquivalentTo(expectedRoles);
    }

    [Fact]
    public async Task GetRoleByRoleId_ReturnsOkWithRole_WhenRoleWithIdExists() 
    {
        var mockRepo = new Mock<IRolesRepository>();

        var roleId = "1";// Needs to be string for generic use case
        var expectedRole = new Roles
        {
            Id = 1,
            Role = "APPLICANT"
        };

        mockRepo.Setup(repo => repo.GetById(roleId)).ReturnsAsync(expectedRole);

        var controller = new RolesController(mockRepo.Object);

        var result = await controller.GetById(roleId);

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull("because the role with id provided exists");

        var actualRole = okResult!.Value as Roles;
        actualRole.Should().NotBeNull();
        actualRole.Should().BeEquivalentTo(expectedRole);
    }

    [Fact]
    public async Task GetRoleByRoleId_ReturnsNotFound_WhenRoleWithIdDoesNotExist()
    {
        var mockRepo = new Mock<IRolesRepository>();

        var imaginaryId = "9999"; // Needs to be string for generic use case

        mockRepo.Setup(repo => repo.GetById(imaginaryId)).ReturnsAsync((Roles?)null);

        var controller = new RolesController(mockRepo.Object);

        var result = await controller.GetById(imaginaryId);

        result.Result.Should().BeOfType<NoContentResult>("because role with no ID exists");
    }

    [Fact]
    public async Task Create_ReturnsOk_WhenRoleIsCreated()
    {
        var mockRepo = new Mock<IRolesRepository>();
        var newRole = new Roles
        {
            Id = 1,
            Role = "APPLICANT"
        };

        mockRepo.Setup(r => r.Exists(newRole)).ReturnsAsync(false);
        mockRepo.Setup(r => r.Create(newRole)).ReturnsAsync(newRole);

        var controller = new RolesController(mockRepo.Object);

        var result = await controller.Create(newRole);

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(newRole);
    }

    [Fact]
    public async Task Create_ReturnsConflict_WhenUserAlreadyExists()
    {
        var mockRepo = new Mock<IRolesRepository>();
        var existingRole = new Roles
        {
            Id = 1,
            Role = "APPLICANT"
        };

        mockRepo.Setup(r => r.Exists(existingRole)).ReturnsAsync(true);

        var controller = new RolesController(mockRepo.Object);

        var result = await controller.Create(existingRole);

        var conflictResult = result.Result as ConflictObjectResult;
        conflictResult.Should().NotBeNull();
        conflictResult!.StatusCode.Should().Be(409);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
    {

        var mockRepo = new Mock<IRolesRepository>();
        var invalidRole = new Roles(); // Assuming the required fields have not been entered

        var controller = new RolesController(mockRepo.Object);
        controller.ModelState.AddModelError("role", "role is required");

        var result = await controller.Create(invalidRole);

        var badRequest = result.Result as BadRequestObjectResult;
        badRequest.Should().NotBeNull();
        badRequest!.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Put_ReturnsOk_WhenRoleIsUpdated()
    {
        var mockRepo = new Mock<IRolesRepository>();
        var roleToUpdate = new Roles
        {
            Id = 1,
            Role = "APPLICANT"
        };

        mockRepo.Setup(r => r.Update(roleToUpdate)).ReturnsAsync(roleToUpdate);

        var controller = new RolesController(mockRepo.Object);

        var result = await controller.Put(roleToUpdate);

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(roleToUpdate);
    }

    [Fact]
    public async Task Put_ReturnsNotFound_WhenRoleDoesNotExist()
    {
        var mockRepo = new Mock<IRolesRepository>();
        var roleToUpdate = new Roles
        {
            Id = 1,
            Role = "APPLICANT"
        };

        mockRepo.Setup(r => r.Update(roleToUpdate)).ReturnsAsync((Roles?)null);

        var controller = new RolesController(mockRepo.Object);

        var result = await controller.Put(roleToUpdate);

        var notFoundResult = result.Result as NotFoundResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Put_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        var mockRepo = new Mock<IRolesRepository>();
        var invalidRole = new Roles(); 

        var controller = new RolesController(mockRepo.Object);
        controller.ModelState.AddModelError("role", "Role is required");

        var result = await controller.Put(invalidRole);

        var badRequest = result.Result as BadRequestObjectResult;
        badRequest.Should().NotBeNull();
        badRequest!.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenRoleIsDeleted()
    {
        var mockRepo = new Mock<IRolesRepository>();
        var roleId = "1"; // Needs to be string for generic use case

        mockRepo.Setup(r => r.Delete(roleId)).ReturnsAsync(true);

        var controller = new RolesController(mockRepo.Object);

        var result = await controller.Delete(roleId);

        result.Should().BeOfType<OkResult>();
        ((OkResult)result).StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenRoleDoesNotExist()
    {
        var mockRepo = new Mock<IRolesRepository>();
        var roleId = "99999"; // Needs to be string for generic use case

        mockRepo.Setup(r => r.Delete(roleId)).ReturnsAsync(false);

        var controller = new RolesController(mockRepo.Object);

        var result = await controller.Delete(roleId);

        result.Should().BeOfType<NotFoundResult>();
        ((NotFoundResult)result).StatusCode.Should().Be(404);
    }

}
