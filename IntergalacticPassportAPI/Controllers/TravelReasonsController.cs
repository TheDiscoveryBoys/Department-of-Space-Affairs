using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/travel_reasons")]
    [Authorize(Roles="APPLICANT, OFFICER")]

    public class TravelReasonsController : BaseController<TravelReasons, ITravelReasonsRepository>
    {
        public TravelReasonsController(ITravelReasonsRepository repo) : base(repo) { }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<TravelReasons>>> All()
        {
            return await BaseRequest(async () =>
            {
                return Ok(await _repo.GetAll());
            });
        }
    }
}