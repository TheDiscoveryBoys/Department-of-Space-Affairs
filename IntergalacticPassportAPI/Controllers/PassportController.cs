using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/passport")]
    public class PassportController : ControllerBase
    {
        private readonly PassportRepository _repo;

        public PassportController(PassportRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Passport>>> GetAll()
        {
            var passports = await _repo.GetAllAsync();
            return Ok(passports);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Passport>> GetById(int id)
        {
            var passport = await _repo.GetByIdAsync(id);
            return passport == null ? NotFound() : Ok(passport);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<int>> Create(Passport passport)
        {
            var newId = await _repo.CreateAsync(passport);
            return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
        }
    }
}
