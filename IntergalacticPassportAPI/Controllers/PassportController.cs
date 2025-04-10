using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/passport")]
    public class PassportController : BaseController<Passport, PassportRepository>
    {
        StatusRepository statusRepo;
        public PassportController(PassportRepository repo, StatusRepository statusRepo) : base(repo) {
            this.statusRepo = statusRepo;
         }

        [HttpPost]
        public  override async Task<ActionResult<Passport>> Create(Passport passport){
            Console.WriteLine($"Trying to create passport");
            var status = await statusRepo.Create(new Status("PENDING", null));
            passport.StatusId = status.Id;
            var passportDB = await _repo.Create(passport);
            Console.WriteLine($"Successfully created passport with id {passportDB}");
            return passportDB;
        }
    }

    
}
