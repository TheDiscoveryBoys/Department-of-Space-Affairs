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

        public VisaController(VisaRepository repo) : base(repo) { }
        
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Visa>>> GetUserVisas(string id)
        {
            return await BaseRequest(async () =>
            {
                var userVisas = await _repo.GetUserVisas(id);
                return userVisas.Any() ? Ok(userVisas) : NoContent();
            });
        }

    }
}
