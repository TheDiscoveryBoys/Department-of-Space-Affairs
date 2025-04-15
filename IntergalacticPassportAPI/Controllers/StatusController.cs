using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/status")]
    [Authorize(Roles="APPLICANT")]
    // TODO: ADD AUTHORIZATION
    public class StatusController : BaseController<Status, IStatusRepository>
    {

        public StatusController(IStatusRepository repo) : base(repo) { }
       

    }
}
