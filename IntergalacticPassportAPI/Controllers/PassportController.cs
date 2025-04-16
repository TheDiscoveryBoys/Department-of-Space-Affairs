using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/passport")]
    [Authorize(Roles = "APPLICANT, OFFICER")]
    public class PassportController : BaseController<Passport, IPassportRepository>
    {
        IApplicationStatusRepository applicationStatusRepo;
        public PassportController(IPassportRepository repo, IApplicationStatusRepository applicationStatusRepo) : base(repo)
        {
            this.applicationStatusRepo = applicationStatusRepo;
        }

        [HttpPost]
        public override async Task<ActionResult<Passport>> Create(Passport passport)
        {
            return await BaseRequest(async () =>
            {
                passport.StatusId = 1;
                var passportDB = await _repo.Create(passport);
                return Ok(passportDB);
            });
        }

        [HttpGet]
        [Route("user")]
        public async Task<ActionResult<IEnumerable<Passport>>> GetPassportApplicationById(string google_id)
        {
            return await BaseRequest(async () =>
            {
                return Ok(await _repo.GetPassportApplicationsByGoogleId(google_id));
            });
        }

        [HttpGet]
        [Authorize(Roles = "OFFICER")]
        [Route("getnext")]
        public async Task<ActionResult<Passport>> GetPassportApplicationByOfficerId(string officerId)
        {
            return await BaseRequest(async () =>
            {
                return Ok(await _repo.GetPassportApplicationByOfficerId(officerId));
            });
        }
    }


}
