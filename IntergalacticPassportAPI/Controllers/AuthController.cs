using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly UsersRepository _usersRepository;
        private readonly UserRolesRepository _userRolesRepository;

        public AuthController(UsersRepository usersRepository, UserRolesRepository userRolesRepository)
        {
            _usersRepository = usersRepository;
            _userRolesRepository = userRolesRepository; 
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginPostBody body){
            Console.WriteLine(body.GoogleAuthCode);
            var googleTokenResp = await GoogleAuthHelpers.getJwt(body.GoogleAuthCode);
            if(googleTokenResp.id_token == null){
                return Unauthorized(new {message = "Failed to retrieve the jwt from Google"});
            }
            return Ok(new LoginResponse{Token = googleTokenResp.id_token});
        }


        [Authorize]
        [HttpPost("register")]
        public async Task<ActionResult<Users>> Register([FromBody] Users user)
        {
            try
            {
                var googleId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine("This is the googleID: " + googleId);
                if (googleId == null)
                    return Unauthorized("Missing Google ID in JWT.");

                var existingUser = await _usersRepository.GetById(googleId);
                if (existingUser is not null)
                    return Ok(existingUser);

                user.GoogleId = googleId;
                var createdUser = await _usersRepository.Create(user);
                var newUserRole = new UserRoles
                {
                    UserId = googleId,
                    RoleId = 1
                };
                var createdUserRole = await _userRolesRepository.CreateUserRole(newUserRole);
                return CreatedAtAction(nameof(Register), new { id = createdUser.GoogleId }, createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server error", details = ex.Message });
            }
        }
    }

    
    }
