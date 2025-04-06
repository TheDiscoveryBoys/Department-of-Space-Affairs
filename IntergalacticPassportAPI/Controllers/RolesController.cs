using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/roles")]

    public class RolesController(RolesRepository repo) : ControllerBase
    {
        [HttpGet("{id}")]
         public async Task<ActionResult<Roles>> GetRoleById(string id)
         {
            var role = await repo.GetRoleById(id);
            return role == null ? NoContent() : Ok(role);
         }

         [HttpGet]
        public async Task<ActionResult<IEnumerable<Roles>>> GetAllRoles()
        {
            var roles = await repo.GetAllRoles();
            return roles.Any() ? Ok(roles) : NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Roles>> CreateRole(Roles role){
             if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var roleExists = await repo.RoleExistsAsync(role.role);
            if(!roleExists){
               return Ok(await repo.CreateRole(role));
            } else{
                return Conflict("Role Exists");
            }
        }

    }
}