using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/users")]

    public class UserController : BaseController<Users, UsersRepository>
    {

        public UserController(UsersRepository repo) : base(repo) { }


        [HttpPost]
        public override async Task<ActionResult<Users>> Create([FromBody] Users user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _repo.Exists(user);

            if (!existingUser)
            {
                var registeredUser = await _repo.Create(user);
                return Ok(registeredUser);
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpGet("{id}/roles")]
        public async Task<ActionResult<IEnumerable<Roles>>> GetUserRoles(string id)
        {
            var roles = await _repo.GetUserRoles(id);
            if (roles.Any())
            {
                return Ok(roles);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost("{userId}/roles/{roleId}")]
        public async Task<ActionResult> AssignRoleToUser(string userId, int roleId)
        {
            return await BaseRequest(async () =>
            {
                var succesfullyAssigned = await _repo.AssignRoleToUser(userId, roleId);
                if (succesfullyAssigned)
                {
                    return Ok();
                }
                else
                {
                    return Conflict();
                }
            });
        }
    }
}

