using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/roles")]
    [Authorize(Roles="APPLICANT")]

    public class RolesController : BaseController<Roles, IRolesRepository>
    {
        public RolesController(IRolesRepository repo) : base(repo) { }
    }
}