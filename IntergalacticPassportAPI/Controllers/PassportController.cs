using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/passport")]
    [Authorize(Roles="")]
    public class PassportController : BaseController<Passport, PassportRepository>
    {

        public PassportController(PassportRepository repo) : base(repo) { }

    }
}
