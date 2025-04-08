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
        //private readonly PassportService _service;

        //public PassportController(PassportService service)
        //{
        //    _service = service;
        //}

        public PassportController(PassportRepository repo) : base(repo) { }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Passport>>> GetAll()
        //{
        //    var passports = await _service.GetAllAsync();
        //    return Ok(passports);
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Passport>> GetById(int id)
        //{
        //    var passport = await _service.GetByIdAsync(id);
        //    return passport == null ? NotFound() : Ok(passport);
        //}

        //[Authorize]
        //[HttpPost]
        //public async Task<ActionResult<int>> Create(Passport passport)
        //{
        //    var newId = await _service.CreateAsync(passport);
        //    return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
        //}
    }
}
