using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using IntergalacticPassportAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/status")]
    // TODO: ADD AUTHORIZATION
    public class StatusController : ControllerBase
    {
        private readonly StatusRepository _repo;

        public StatusController(StatusRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Status>>> GetAll()
        {
            var statuses = await _repo.GetAllAsync();
            return Ok(statuses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Status>>> GetById(int id)
        {
            var status = await _repo.GetByIdAsync(id);
            return Ok(status);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(Status status)
        {
            var newId = await _repo.CreateAsync(status);
            return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
        }

    }
}
