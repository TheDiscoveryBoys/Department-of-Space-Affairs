using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/application_status")]
    [Authorize(Roles = "APPLICANT, OFFICER")]
    // TODO: ADD AUTHORIZATION
    public class ApplicationStatusController : BaseController<ApplicationStatus, IApplicationStatusRepository>
    {

        public ApplicationStatusController(IApplicationStatusRepository repo) : base(repo) { }


    }
}
