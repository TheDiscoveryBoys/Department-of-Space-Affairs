﻿using Dapper;
using IntergalacticPassportAPI.Models;
using Npgsql;
using System.Data;

namespace IntergalacticPassportAPI.Data
{
	public class StatusRepository(IConfiguration config) : BaseRepository<Status>(config, "statuses")
    {
        public async override Task<bool> Exists(Status status)
        {
            var existingStatus = await GetById((status.Id).ToString());
            if (existingStatus == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //private readonly IConfiguration _configuration;

        //public StatusRepository(IConfiguration configuration) 
        //{
        //          _configuration = configuration;
        //      }

        //      private IDbConnection CreateConnection()
        //      {
        //          return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        //      }

        //      public async Task<IEnumerable<Status>> GetAllAsync()
        //      {
        //          using var connection = CreateConnection();
        //          return await connection.QueryAsync<Status>("SELECT * FROM statuses ORDER BY id;");
        //      }

        //      public async Task<Status?> GetByIdAsync(int id)
        //      {
        //          using var connection = CreateConnection();
        //          return await connection.QueryFirstOrDefaultAsync<Status>(
        //              "SELECT * FROM statuses WHERE id = @Id;", new { Id = id });
        //      }

        //      public async Task<int> CreateAsync(Status status)
        //      {
        //          try
        //          {
        //              using var connection = CreateConnection();
        //              var sql = @"INSERT INTO statuses (name, reason) 
        //                VALUES (@Name, @Reason)
        //                RETURNING id;";
        //              return await connection.ExecuteScalarAsync<int>(sql, status);
        //          }
        //          catch (Exception ex) 
        //          {
        //              throw new Exception("An error occurred while creating the status.", ex);
        //          }

        //      }
    }
}
