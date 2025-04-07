using IntergalacticPassportAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    public abstract class BaseController<Model, ModelRepo> : ControllerBase
        where ModelRepo : BaseRepository<Model>
    {
        protected readonly ModelRepo _repo;

        protected BaseController(ModelRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Model>> GetById(string id)
        {
            return await BaseRequest(async () =>
            {
                var user = await _repo.GetById(id);
                return user == null ? NoContent() : Ok(user);
            });

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Model>>> GetAllUsers()
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
                    return Conflict();
                }
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