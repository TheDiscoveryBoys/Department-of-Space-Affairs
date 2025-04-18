﻿using Dapper;
using IntergalacticPassportAPI.Models;
using Npgsql;
using System.Data;

namespace IntergalacticPassportAPI.Data
{
    public class ApplicationStatusRepository(IConfiguration config) : BaseRepository<ApplicationStatus>(config), IApplicationStatusRepository
    {
        public override async Task<bool> Exists(ApplicationStatus model)
        {
            var existingModel = await GetById(model.Id.ToString());
            if (existingModel != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
