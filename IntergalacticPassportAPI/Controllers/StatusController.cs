using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/status")]
    // TODO: ADD AUTHORIZATION
    public class StatusController : BaseController<Status, StatusRepository>
    {

        public StatusController(StatusRepository repo) : base(repo) { }
       

    }
}
