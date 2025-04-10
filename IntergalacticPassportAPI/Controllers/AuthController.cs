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

        [HttpPost]
        public async Task<IActionResult> Post(LoginPostBody body, UsersRepository repo)
        {
            var googleTokenResp = await GoogleAuthHelpers.getJwt(body.GoogleAuthCode);
            if (googleTokenResp.id_token == null)
            {
                return Unauthorized(new { message = "Failed to retrieve the jwt from Google" });
            }
            var claims = GoogleAuthHelpers.DecodeClaims(googleTokenResp.id_token);
            if (claims == null) return BadRequest(new {message = "Failed to decode the claims"});
           
            if (claims.ContainsKey("sub") && claims.ContainsKey("email") && claims.ContainsKey("name"))
            {
                return Ok(new LoginResponse { Token = googleTokenResp.id_token });
            }
            else
            {
                return BadRequest(new
                {
                    message = "JWT is missing one or more required claims: 'sub', 'email', or 'name'.",
                    availableClaims = claims.Keys
                });
            }
        }
    }
}
