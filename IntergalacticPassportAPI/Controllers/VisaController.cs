using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/visa")]
    public class VisaController : BaseController<Visa, VisaRepository>
    {
        StatusRepository statusRepo;
        public VisaController(VisaRepository repo, StatusRepository statusRepo) : base(repo) { 
            this.statusRepo = statusRepo;
        }

        [HttpGet]
        [Route("user")]
        public async Task<ActionResult<IEnumerable<Passport>>> GetVisaApplicationById(string google_id){
            Console.WriteLine($"Trying to get visa applications for google id {google_id}");
            return Ok(await _repo.GetVisaApplicationsByGoogleId(google_id));
        }

        [HttpPost]
        public  override async Task<ActionResult<Visa>> Create(Visa visa){
            Console.WriteLine($"Trying to create visa");
            var status = await statusRepo.Create(new Status("PENDING", null));
            visa.StatusId = status.Id;
            var visaDB = await _repo.Create(visa);
            Console.WriteLine($"Successfully created visa with id {visaDB}");
            return visaDB;
        }
    }
}
