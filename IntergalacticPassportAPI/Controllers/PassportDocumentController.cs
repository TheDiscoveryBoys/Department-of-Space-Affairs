using IntergalacticPassportAPI.Controllers;
using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntergalacticPassportAPI.lib.S3Helpers;

namespace IntergalacticPassportportAPI.Controllers
{
	[ApiController]
	[Route("api/passport-documents")]
	[Authorize(Roles="APPLICANT, OFFICER")]
	public class PassportDocumentController : BaseController<PassportDocument, IPassportDocumentRepository>
	{
		public PassportDocumentController(IPassportDocumentRepository repo) : base(repo) { }

		[HttpGet("passport-application/{id}")]
		public async Task<ActionResult<IEnumerable<PassportDocument>>> GetByPassportApplicationId(int id)
		{
			return await BaseRequest(async () =>
			{
				var passportApplicationDocuments = await _repo.GetByPassportApplicationIdAsync(id);

				if (passportApplicationDocuments.Any())
				{
					return Ok(passportApplicationDocuments);
				}
				else
				{
					return NoContent();
				}
			});
		}

		[HttpPost("upload")]
		public async Task<ActionResult<PassportDocument>> CreateDocument(IFormFile file, [FromForm] string filename, [FromForm] int application_id)
		{
			return await BaseRequest(async () =>
			{
				Console.WriteLine("Trying to upload a file");
				if (file == null || file.Length == 0)
				{
					return BadRequest("No file uploaded.");
				}
				// get application for the application id
				//get the primary key
				// Log metadata
				Console.WriteLine($"Description: {filename}");
				Console.WriteLine($"File name: {file.FileName}");

				var fileUrl = await S3Helpers.UploadFileAsync(file, $"{filename} ({application_id})") ?? throw new Exception("Could not upload the file to S3");
				var doc = new PassportDocument { Filename = filename, PassportApplicationId = application_id, S3Url = fileUrl };
				// Optional: save to disk or process
				await _repo.Create(doc);
				Console.WriteLine("Uploaded the file successfully");
				return Ok(new { message = "File uploaded successfully!" });
			});

		}
	}
}

