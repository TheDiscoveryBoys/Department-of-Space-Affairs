﻿using System.Text.Json;
using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntergalacticPassportAPI.Controllers
{
    [ApiController]
    [Route("api/visa")]
    [Authorize(Roles = "APPLICANT, OFFICER")]
    public class VisaController : BaseController<VisaApplication, IVisaRepository>
    {
        IApplicationStatusRepository applicationStatusRepo;
        public VisaController(IVisaRepository repo, IApplicationStatusRepository applicationStatusRepo) : base(repo)
        {
            this.applicationStatusRepo = applicationStatusRepo;
        }

        [HttpGet]
        [Route("user")]
        public async Task<ActionResult<IEnumerable<VisaApplication>>> GetVisaApplicationById(string google_id)
        {
            return await BaseRequest(async () =>
            {
                Console.WriteLine($"Trying to get visa applications for google id {google_id}");
                var result = await _repo.GetVisaApplicationsByGoogleId(google_id);
                Console.WriteLine("These are the VISA results");
                Console.WriteLine(JsonSerializer.Serialize(result));
                return Ok(result);
            });

        }

        [HttpPost]
        public override async Task<ActionResult<VisaApplication>> Create(VisaApplication visa)
        {
        
                Console.WriteLine($"Trying to create visa");
                //check if there exists a VISA for same DestinationPlanet, StatusId = 2 -> cannot create because pending VISA application exists for user
                var userCurrentVisas = await _repo.GetVisaApplicationsByGoogleId(visa.UserId);
                var hasPendingVisas = userCurrentVisas.Any(v =>
                    v.DestinationPlanet == visa.DestinationPlanet &&
                    applicationStatusRepo.GetById(v.StatusId).Result?.Name == "PENDING");


                if (hasPendingVisas)
                {
                    if (hasPendingVisas)
                    {
                        return Conflict("Could not create VISA. A pending VISA already exists for this planet.");
                    }
                }

                // Check if there exists an APPROVED VISA for the same DestinationPlanet and date range
                var hasApprovedVisaWithSameDetails = userCurrentVisas.Any(v =>
                    v.DestinationPlanet == visa.DestinationPlanet &&
                    v.StartDate.Date == visa.StartDate.Date &&
                    v.EndDate.Date == visa.EndDate.Date &&
                    applicationStatusRepo.GetById(v.StatusId).Result?.Name == "APPROVED");

                if (hasApprovedVisaWithSameDetails)
                {
                    return Conflict("Could not create VISA. An approved VISA already exists for this planet for this time.");
                }

                // var status = await statusRepo.Create(new Status("PENDING", null));
                visa.StatusId = 1;
                var visaDB = await _repo.Create(visa);
                Console.WriteLine($"Successfully created visa with id {visaDB}");
                return Ok(visaDB);
    

        }

        [HttpGet]
        [Authorize(Roles = "OFFICER")]
        [Route("getnext")]
        public async Task<ActionResult<VisaApplication>> GetVisaApplicationByOfficerId(string officerId)
        {
            return await BaseRequest(async () =>
           {
               Console.WriteLine($"Trying to get a visa applications for google id {officerId}");
               return Ok(await _repo.GetVisaApplicationByOfficerId(officerId));
           });

        }
    }
}
