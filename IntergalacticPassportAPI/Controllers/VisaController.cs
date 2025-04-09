using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/visa")]
    [Authorize]
    public class VisaController : BaseController<Visa, VisaRepository>
    {
        //private readonly VisaRepository _repo;

        //public VisaController(VisaRepository repo)
        //{
        //    _repo = repo;
        //}

        public VisaController(VisaRepository repo) : base(repo) { }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Visa>>> GetAll()
        //{
        //    var visas = await _repo.GetAllAsync();
        //    return Ok(visas);
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<IEnumerable<Visa>>> GetById(int id)
        //{
        //    var visa = await _repo.GetByIdAsync(id);
        //    return Ok(visa);
        //}

        //[HttpPost]
        //public override async Task<ActionResult<Visa>> Create([FromBody] Visa visa)
        //{
        //    return await BaseRequest(async sync () =>
        //    {
        //        if (!ModelState.isValid)
        //            return BadRequest(ModelState);

        //        var existingVisa = await _repo.Exists(visa);

        //        if (!existingVisa)
        //        {
        //            var appliedVisa = await _repo.CreateAsync(visa);
        //            return CreatedAtAction(nameof(GetById), new { id = newId }, appliedVisa);
        //        }
        //        else 
        //        {
        //            return Conflict("Visa already exists.");
        //        }
        //    })
        //}

    }
}
