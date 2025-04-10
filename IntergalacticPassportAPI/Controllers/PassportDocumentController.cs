using IntergalacticPassportAPI.Controllers;
using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using IntergalacticPassportAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportportAPI.Controllers
{
	[ApiController]
	[Route("api/passport-documents")]
	public class PassportDocumentController : BaseController<PassportDocument, PassportDocumentRepository>
	{

		public PassportDocumentController(PassportDocumentRepository repo) : base(repo) { }
		//private readonly PassportDocumentService _service;

		public PassportDocumentController(PassportDocumentService service, PassportDocumentRepository repo) : base(repo)
		{
			_service = service;
		}

		[HttpGet("passport-application/{id}")]
		// TODO: Add authorization.
		public async Task<ActionResult<IEnumerable<PassportDocument>>> GetByPassportApplicationId(int id)
		{
			return await BaseRequest(async () =>
			{
				var passportApplicationDocuments = await _service.GetByPassportApplicationIdAsync(id);

				if(passportApplicationDocuments.Any()){
					return Ok(passportApplicationDocuments);
				} else{
					return NoContent();
				}
				
			});

		}

		// [HttpGet]
		// // TODO: Add authorization
		// public async Task<ActionResult<IEnumerable<PassportDocument>>> GetAll()
		// { 
		// 	var passportApplicationDocuments = await _service.GetAllAsync();
		// 	return Ok(passportApplicationDocuments);
		// }

		// [HttpGet("{id}")]
		// public async Task<ActionResult<IEnumerable<PassportDocument>>> GetById(int id)
		// {
		//     var passportApplicationDocuments = await _service.GetByIdAsync(id);
		//     return Ok(passportApplicationDocuments);
		// }

		// [HttpPost]
		// // TODO: AUTHORIZE
		// public async Task<ActionResult<int>> Create(PassportDocument passportDocument)
		// {
		//     var newId = await _service.CreateAsync(passportDocument);
		//     return CreatedAtAction(nameof(GetById), new { id = newId }, newId);
		// }
	}

}

