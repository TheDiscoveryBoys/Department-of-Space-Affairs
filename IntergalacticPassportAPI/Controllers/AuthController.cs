using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
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

        public AuthController(IUsersRepository repo, IUserRolesRepository userRolesRepository, IRolesRepository rolesRepo){
            this.UserRepo = repo;
            this.UserRolesRepo = userRolesRepository;
            this.RolesRepo = rolesRepo;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(LoginPostBody body){
            Console.WriteLine(body.GoogleAuthCode);
            var googleTokenResp = await GoogleAuthHelpers.getJwt(body.GoogleAuthCode);

            if (googleTokenResp.id_token == null)
            {
                return Unauthorized(new { message = "Failed to retrieve the jwt from Google" });
            }
            // we have a jwt, get the desired claims
            var claims = GoogleAuthHelpers.DecodeClaims(googleTokenResp.id_token);
            var googleID = claims["sub"];
            var email = claims["email"];
            var name = claims["name"];

            if(googleID != null && email != null && name != null){
                Console.WriteLine("Trying to create a new user");
                var user = new Users{GoogleId = claims["sub"].ToString(), Email=claims["email"].ToString(), Name=claims["name"].ToString()};
                if(! await UserRepo.Exists(user)){
                    await UserRepo.Create(user);
                    var applicantRole = RolesRepo.GetRolesByName("APPLICANT");
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
