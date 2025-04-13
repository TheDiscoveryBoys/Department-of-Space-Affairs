using System.Text.Json;
using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/visa")]
    public class VisaController : BaseController<Visa, IVisaRepository>
    {
        IStatusRepository statusRepo;
        public VisaController(IVisaRepository repo, IStatusRepository statusRepo) : base(repo) { 
            this.statusRepo = statusRepo;
        }

        [HttpGet]
        [Route("user")]
        public async Task<ActionResult<IEnumerable<Visa>>> GetVisaApplicationById(string google_id)
        {
            return await BaseRequest(async () =>
            {
                Console.WriteLine($"Trying to get visa applications for google id {google_id}");
                var result = await _repo.GetVisaApplicationsByGoogleId(google_id);
                Console.WriteLine("These are the VISA results");
                Console.WriteLine(JsonSerializer.Serialize(result));
                return Ok(result);
            });

        }

        [HttpPost]
        public override async Task<ActionResult<Visa>> Create(Visa visa)
        {
            return await BaseRequest(async () =>
            {
                Console.WriteLine($"Trying to create visa");
                var status = await statusRepo.Create(new Status("PENDING", null));
                visa.StatusId = status.Id;
                var visaDB = await _repo.Create(visa);
                Console.WriteLine($"Successfully created visa with id {visaDB}");
                return Ok(visaDB);
            });

        }

        [HttpGet]
        [Route("getnext")]
        public async Task<ActionResult<Visa>> GetVisaApplicationByOfficerId(string officerId)
        {
            return await BaseRequest(async () =>
           {
               Console.WriteLine($"Trying to get a visa applications for google id {officerId}");
               return Ok(await _repo.GetVisaApplicationByOfficerId(officerId));
           });

        }
    }
}
