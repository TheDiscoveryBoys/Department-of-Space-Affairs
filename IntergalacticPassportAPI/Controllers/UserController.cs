using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles="APPLICANT")]
    public class UserController : BaseController<Users, IUsersRepository>
    {
        public UserController(IUsersRepository repo) : base(repo) { }

        [HttpGet]
        [Route("email/{email}")]
        [Authorize(Roles="OFFICER")]
        public async Task<ActionResult<Users>> GetUserByEmail(string email){
            Console.WriteLine($"Trying to get user by email {email}");
            return Ok(await _repo.GetUserByEmail(email));
        }

        [HttpGet("{id}/roles")]
        public async Task<ActionResult<IEnumerable<Roles>>> GetUserRoles(string id)
        {
            return await BaseRequest(async () =>
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
            });
        }

        [HttpPost("{userId}/roles/{roleId}")]
        [Authorize(Roles="OFFICER")]
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

