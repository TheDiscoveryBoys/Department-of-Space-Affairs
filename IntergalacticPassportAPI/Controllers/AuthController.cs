using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using IntergalacticPassportAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("auth/login")]
    public class AuthController : ControllerBase
    {
        IUsersRepository UserRepo;
        IUserRolesRepository UserRolesRepo;
        IRolesRepository RolesRepo;
        IGoogleAuthService GoogleAuthService;

        public AuthController(IUsersRepository repo, IUserRolesRepository userRolesRepository, IRolesRepository rolesRepo, IGoogleAuthService googleAuth)
        {
            this.UserRepo = repo;
            this.UserRolesRepo = userRolesRepository;
            this.RolesRepo = rolesRepo;
            this.GoogleAuthService = googleAuth;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(LoginPostBody body){
            Console.WriteLine(body.GoogleAuthCode);
            var googleTokenResp = await GoogleAuthService.GetJwt(body.GoogleAuthCode);
            if (googleTokenResp.id_token == null)
            {
                return Unauthorized(new { message = "Failed to retrieve the jwt from Google" });
            }
            // we have a jwt, get the desired claims
            var claims = GoogleAuthService.DecodeClaims(googleTokenResp.id_token);
            var googleID = claims["sub"];
            var email = claims["email"];
            var name = claims["name"];

            if(googleID != null && email != null && name != null){
                Console.WriteLine("Trying to create a new user");
                var user = new Users{GoogleId = claims["sub"].ToString(), Email=claims["email"].ToString(), Name=claims["name"].ToString()};
                if(! await UserRepo.Exists(user)){
                    await UserRepo.Create(user);
                    var applicantRole = await RolesRepo.GetRolesByName("APPLICANT");
                    var userRole = new UserRoles{RoleId=applicantRole.Id, UserId=user.GoogleId};
                    await UserRolesRepo.Create(userRole);
                }
                return Ok(new LoginResponse{Token = googleTokenResp.id_token});
            }else{
                return Unauthorized(new {message = "Google jwt did not have the necessary claims"});
            }
        }
    }
}
