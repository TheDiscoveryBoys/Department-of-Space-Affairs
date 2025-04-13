using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/passport")]
    public class PassportController : BaseController<Passport, PassportRepository>
    {
        StatusRepository statusRepo;
        public PassportController(PassportRepository repo, StatusRepository statusRepo) : base(repo)
        {
            this.statusRepo = statusRepo;
        }

        [HttpPost]
        public override async Task<ActionResult<Passport>> Create(Passport passport)
        {
            return await BaseRequest(async () =>
            {
                var status = await statusRepo.Create(new Status("PENDING", null));
                passport.StatusId = status.Id;
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
        [Route("getnext")]
        public async Task<ActionResult<Passport>> GetPassportApplicationByOfficerId(string officerId)
        {
            return await BaseRequest(async () => {
                return Ok(await _repo.GetPassportApplicationByOfficerId(officerId));
            });
           
        }
    }


}
