using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    public abstract class BaseController<Model, ModelRepo> : ControllerBase
    {
        protected readonly ModelRepo _repo;

        protected BaseController(ModelRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("{id}")]
        protected abstract Task<ActionResult<Model>> FetchById(string id);

        [HttpGet]
        protected abstract Task<ActionResult<IEnumerable<Model>>> FetchAll();

        [HttpPost]
        protected abstract Task<ActionResult<Model>> Create([FromBody] Model model);
    }
}