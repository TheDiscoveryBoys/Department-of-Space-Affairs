using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    public abstract class BaseController<Model, ModelRepo> : ControllerBase
        where ModelRepo : IBaseRepository<Model>
    {
        protected readonly ModelRepo _repo;

        protected BaseController(ModelRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("{id}")]
        // [Authorize(Roles = "Citizen")]
        public async Task<ActionResult<Model>> GetById(string id)
        {

            if (typeof(Model) == typeof(Users))
            {
                return await BaseRequest(async () =>
                {
                    var model = await _repo.GetById(id);
                    return model == null ? NoContent() : Ok(model);

                });
            }

            else 
            {
                return await BaseRequest(async () =>
                {
                    var model = await _repo.GetById(Int32.Parse(id));
                    return model == null ? NoContent() : Ok(model);

                });
            }
        }

        [HttpGet]
        // [Authorize(Roles = "Citizen")]
        public async Task<ActionResult<IEnumerable<Model>>> GetAll()
        {
            return await BaseRequest(async () =>
            {
                var models = await _repo.GetAll();
                return models.Any() ? Ok(models) : NoContent();
            });
        }

        [HttpPost]
        public virtual async Task<ActionResult<Model>> Create([FromBody] Model model)
        {
            return await BaseRequest(async () =>
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var exists = await _repo.Exists(model);

                if (!exists)
                {
                    var createdModel = await _repo.Create(model);
                    return Ok(createdModel);
                }
                else
                {
                    return Conflict($"This {model.GetType().Name} already exists.");
                }
            });
        }

        [HttpPut]
        public async Task<ActionResult<Model>> Put([FromBody] Model model)
        {
            return await BaseRequest(async () =>
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedModel = await _repo.Update(model);
                if (updatedModel != null)
                {
                    return Ok(updatedModel);
                }
                else
                {
                    return NotFound();
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            return await BaseRequest(async () =>
            {
                bool deleted = await _repo.Delete(id);

                return deleted ? Ok() : NotFound();
            });
        }
        protected async Task<ActionResult> BaseRequest(Func<Task<ActionResult>> controllerLogic)
        {
            try
            {
                return await controllerLogic();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error Occurred!", details = ex.Message });
            }
        }
    }
}