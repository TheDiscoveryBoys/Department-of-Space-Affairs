using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/travel_reasons")]
    [Authorize(Roles="APPLICANT")]

    public class TravelReasonsController : BaseController<TravelReasons, ITravelReasonsRepository>
    {
        public TravelReasonsController(ITravelReasonsRepository repo) : base(repo) { }

        [HttpGet]
        [Authorize(Roles="APPLICANT")]
        public async override Task<ActionResult<IEnumerable<TravelReasons>>> GetAll()
        {
            return await BaseRequest(async () =>
            {
                return Ok(await _repo.GetAll());
            });
        }
    }
}