using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/users")]

    public class UserController(UsersRepository repo) : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUserByGoogleId(string id)
        {
            var user = await repo.GetUserByGoogleId(id);
            return user == null ? NoContent() : Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers()
        {
            var users = await repo.GetAllUsers();
            return users.Any() ? Ok(users) : NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Users>> RegisterOrLoginUser([FromBody] Users user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await repo.GetUserByGoogleId(user.google_id);

            if (existingUser == null)
            {
                var registeredUser = await repo.RegisterUser(user);
                return Ok(registeredUser);
            }
            else
            {
                return Ok(user);
            }
        }
    }
}

