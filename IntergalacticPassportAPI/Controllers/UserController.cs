using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/users")]

    public class UserController(UsersRepository repo) : ControllerBase
    {
        private readonly UsersRepository _repo = repo;

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUserByGoogleId(string id){
            var user = await _repo.GetUserByGoogleId(id);
            return user == null ? NoContent() : Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers(){
            var users = await _repo.GetAllUsers();
            return users.Any() ? Ok(users) : NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Users>> RegisterOrLoginUser([FromBody] Users user){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var registeredUser = await _repo.RegisterOrLoginUser(user);
            return registeredUser == null ? NotFound() : Ok(registeredUser);
        }
    }
}

