using IntergalacticPassportAPI.Controllers;
using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportportAPI.Controllers
{
	[ApiController]
	[Route("api/passport-documents")]
	public class PassportDocumentController : BaseController<PassportDocument, PassportDocumentRepository>
	{

		public PassportDocumentController(PassportDocumentRepository repo) : base(repo) { }

		[HttpGet("passport-application/{id}")]
		// TODO: Add authorization.
		public async Task<ActionResult<IEnumerable<PassportDocument>>> GetByPassportApplicationId(int id)
		{
			return await BaseRequest(async () =>
			{
				var passportApplicationDocuments = await _repo.GetByPassportApplicationIdAsync(id);

				if(passportApplicationDocuments.Any()){
					return Ok(passportApplicationDocuments);
				} else{
					return NoContent();
				}
				
			});

		}
	}

}

