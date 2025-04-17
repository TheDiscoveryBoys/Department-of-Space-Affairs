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
    }
}