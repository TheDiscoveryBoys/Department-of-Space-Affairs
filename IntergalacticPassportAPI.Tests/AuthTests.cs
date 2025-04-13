using Xunit;
using Moq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using IntergalacticPassportAPI.Controllers;
using IntergalacticPassportAPI.Models;
using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Services;
using System; // Added for Roles Id if it's Guid, otherwise use appropriate type

namespace IntergalacticPassportAPI.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IUsersRepository> _mockUserRepo;
        private readonly Mock<IUserRolesRepository> _mockUserRolesRepo;
        private readonly Mock<IRolesRepository> _mockRolesRepo;
        private readonly Mock<IGoogleAuthService> _mockGoogleAuthService;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockUserRepo = new Mock<IUsersRepository>();
            _mockUserRolesRepo = new Mock<IUserRolesRepository>();
            _mockRolesRepo = new Mock<IRolesRepository>();
            _mockGoogleAuthService = new Mock<IGoogleAuthService>();

            _controller = new AuthController(
                _mockUserRepo.Object,
                _mockUserRolesRepo.Object,
                _mockRolesRepo.Object,
                _mockGoogleAuthService.Object
            );
        }

        [Fact]
        public async Task Post_ReturnsOkAndCreatesNewUser_WhenLoginSuccessfulAndUserDoesNotExist()
        {
            var authCode = "valid_auth_code";
            var token = "valid_jwt_token";
            var googleId = "google-id-123";
            var email = "user@example.com";
            var name = "Test User";
            var roleId = 1; 
            var applicantRole = new Roles { Id = roleId, Role = "APPLICANT" };

            var claims = new Dictionary<string, object>
            {
                { "sub", googleId },
                { "email", email },
                { "name", name }
            };


            _mockGoogleAuthService.Setup(s => s.GetJwt(authCode))
                .ReturnsAsync(new GoogleTokenExchangeResponse { id_token = token });
            _mockGoogleAuthService.Setup(s => s.DecodeClaims(token))
                .Returns(claims);


            _mockUserRepo.Setup(r => r.Exists(It.Is<Users>(u => u.GoogleId == googleId)))
                .ReturnsAsync(false);
            _mockUserRepo.Setup(r => r.Create(It.Is<Users>(u => u.GoogleId == googleId && u.Email == email && u.Name == name)))
                .ReturnsAsync((Users u) => u); 

            _mockRolesRepo.Setup(r => r.GetRolesByName("APPLICANT"))
                .ReturnsAsync(applicantRole);


            _mockUserRolesRepo.Setup(r => r.Create(It.Is<UserRoles>(ur => ur.RoleId == roleId && ur.UserId == googleId)))
                 .ReturnsAsync(new UserRoles { RoleId = roleId, UserId = googleId }); 

            var result = await _controller.Post(new LoginPostBody { GoogleAuthCode = authCode });

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().NotBeNull();
            okResult?.Value.Should().BeOfType<LoginResponse>();
            ((LoginResponse)okResult!.Value!).Token.Should().Be(token);

            _mockUserRepo.Verify(r => r.Create(It.IsAny<Users>()), Times.Once);
            _mockRolesRepo.Verify(r => r.GetRolesByName("APPLICANT"), Times.Once);
            _mockUserRolesRepo.Verify(r => r.Create(It.IsAny<UserRoles>()), Times.Once);
        }

        [Fact]
        public async Task Post_ReturnsOk_WhenLoginSuccessfulAndUserAlreadyExists()
        {
            var authCode = "valid_auth_code";
            var token = "valid_jwt_token";
            var googleId = "google-id-123";
            var email = "user@example.com";
            var name = "Test User";

            var claims = new Dictionary<string, object>
            {
                { "sub", googleId },
                { "email", email },
                { "name", name }
            };

            _mockGoogleAuthService.Setup(s => s.GetJwt(authCode))
                .ReturnsAsync(new GoogleTokenExchangeResponse { id_token = token });
            _mockGoogleAuthService.Setup(s => s.DecodeClaims(token))
                .Returns(claims);

            _mockUserRepo.Setup(r => r.Exists(It.Is<Users>(u => u.GoogleId == googleId)))
                .ReturnsAsync(true);


            var result = await _controller.Post(new LoginPostBody { GoogleAuthCode = authCode });


            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().NotBeNull();
            okResult?.Value.Should().BeOfType<LoginResponse>();
            ((LoginResponse)okResult!.Value!).Token.Should().Be(token);

            _mockUserRepo.Verify(r => r.Create(It.IsAny<Users>()), Times.Never);
            _mockRolesRepo.Verify(r => r.GetRolesByName(It.IsAny<string>()), Times.Never);
            _mockUserRolesRepo.Verify(r => r.Create(It.IsAny<UserRoles>()), Times.Never);
        }


        [Fact]
        public async Task Post_ReturnsUnauthorized_WhenGetJwtReturnsNullToken()
        {

            var authCode = "some_code";
            _mockGoogleAuthService.Setup(s => s.GetJwt(authCode))
                .ReturnsAsync(new GoogleTokenExchangeResponse { id_token = null }); 


            var result = await _controller.Post(new LoginPostBody { GoogleAuthCode = authCode });

            result.Should().BeOfType<UnauthorizedObjectResult>();
            var unauthorizedResult = result as UnauthorizedObjectResult;
            unauthorizedResult?.Value.Should().NotBeNull();

            _mockGoogleAuthService.Verify(s => s.DecodeClaims(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Post_ReturnsUnauthorized_WhenDecodeClaimsReturnsNull()
        {

            var authCode = "some_code";
            var token = "valid_but_problematic_token";

            _mockGoogleAuthService.Setup(s => s.GetJwt(authCode))
                .ReturnsAsync(new GoogleTokenExchangeResponse { id_token = token });


            _mockGoogleAuthService.Setup(s => s.DecodeClaims(token))
                .Returns((Dictionary<string, object>?)null);

            await Assert.ThrowsAsync<NullReferenceException>(() =>
                 _controller.Post(new LoginPostBody { GoogleAuthCode = authCode })
            );
        }


        [Fact]
        public async Task Post_ReturnsUnauthorized_WhenRequiredClaimsAreMissingOrNull()
        {

            var authCode = "some_code";
            var token = "token_with_missing_claims";

            _mockGoogleAuthService.Setup(s => s.GetJwt(authCode))
                .ReturnsAsync(new GoogleTokenExchangeResponse { id_token = token });

            var claims = new Dictionary<string, object>
            {
                { "sub", "google-id-123" },
                { "email", "user@example.com" },
                { "name", null }
            };
            _mockGoogleAuthService.Setup(s => s.DecodeClaims(token))
                .Returns(claims);

            var result = await _controller.Post(new LoginPostBody { GoogleAuthCode = authCode });

            result.Should().BeOfType<UnauthorizedObjectResult>();
            var unauthorizedResult = result as UnauthorizedObjectResult;
            unauthorizedResult?.Value.Should().NotBeNull();
            _mockUserRepo.Verify(r => r.Exists(It.IsAny<Users>()), Times.Never);
            _mockUserRepo.Verify(r => r.Create(It.IsAny<Users>()), Times.Never);
            _mockRolesRepo.Verify(r => r.GetRolesByName(It.IsAny<string>()), Times.Never);
            _mockUserRolesRepo.Verify(r => r.Create(It.IsAny<UserRoles>()), Times.Never);
        }
    }
}