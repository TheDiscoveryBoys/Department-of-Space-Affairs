using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/visa")]
    public class VisaController : ControllerBase
    {
        private readonly VisaRepository _repo;

        public VisaController(VisaRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Visa>>> GetAll()
        {
            var visas = await _repo.GetAllAsync();
            return Ok(visas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Visa>>> GetById(int id)
        {
            var visa = await _repo.GetByIdAsync(id);
            return Ok(visa);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(Visa visa)
        {
            var newId = await _repo.CreateAsync(visa);
            return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
        }

    }
}
