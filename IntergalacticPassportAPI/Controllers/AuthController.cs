using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("auth/login")]
    public class AuthContoller : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(LoginPostBody body){
            Console.WriteLine(body.GoogleAuthCode);
            var googleTokenResp = await GoogleAuthHelpers.getJwt(body.GoogleAuthCode);
            if (googleTokenResp.id_token == null)
            {
                return Unauthorized(new { message = "Failed to retrieve the jwt from Google" });
            }
            return Ok(new LoginResponse{Token = googleTokenResp.id_token});
        }
    }
}
