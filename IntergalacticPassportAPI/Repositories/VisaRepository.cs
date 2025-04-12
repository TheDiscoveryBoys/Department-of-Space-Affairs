﻿using Dapper;
using IntergalacticPassportAPI.Models;
using Npgsql;
using System.Data;
using System.Runtime.CompilerServices;

namespace IntergalacticPassportAPI.Data
{
    public class VisaRepository(IConfiguration config) : BaseRepository<Visa>(config, "visa_applications")
    {
        // private readonly IConfiguration _configuration;

        // public VisaRepository(IConfiguration configuration) 
        // {
        //     _configuration = configuration;
        // }
        // public VisaRepository(IConfiguration configuration) 
        // {
        //     _configuration = configuration;
        // }

        // private IDbConnection CreateConnection()
        // {
        //     return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        // }
        // private IDbConnection CreateConnection()
        // {
        //     return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        // }

        // public async Task<IEnumerable<Visa>> GetAllAsync()
        // {
        //     using var connection = CreateConnection();
        //     return await connection.QueryAsync<Visa>("SELECT * FROM visa_applications ORDER BY id;");
        // }
        // public async Task<IEnumerable<Visa>> GetAllAsync()
        // {
        //     using var connection = CreateConnection();
        //     return await connection.QueryAsync<Visa>("SELECT * FROM visa_applications ORDER BY id;");
        // }

        // public async Task<Visa?> GetByIdAsync(int id)
        // {
        //     using var connection = CreateConnection();
        //     return await connection.QueryFirstOrDefaultAsync<Visa>(
        //         "SELECT * FROM visa_applications WHERE id = @Id;", new { Id = id });
        // }
        // public async Task<Visa?> GetByIdAsync(int id)
        // {
        //     using var connection = CreateConnection();
        //     return await connection.QueryFirstOrDefaultAsync<Visa>(
        //         "SELECT * FROM visa_applications WHERE id = @Id;", new { Id = id });
        // }

        // public async Task<int> CreateAsync(Visa visa)
        // {
        //     try
        //     {
        //         using var connection = CreateConnection();
        //         var sql = @"INSERT INTO visa_applications (user_id, destination_planet, travel_reason, start_date, end_date, status_id, submitted_at, processed_at, officer_id) 
        //           VALUES (@UserId, @DestinationPlanet, @TravelReason, @StartDate, @EndDate, @StatusId, @SubmittedAt, @ProcessedAt, @OfficerId)
        //           RETURNING id;";
        //         return await connection.ExecuteScalarAsync<int>(sql, visa);
        //     }
        //     catch (Exception ex) 
        //     {
        //         throw new Exception("An error occurred while creating the visa.", ex);
        //     }

        // }

        public async Task<IEnumerable<Visa>> GetVisaApplicationsByGoogleId(string googleId){
            using (var db = CreateDBConnection()){
                var sql = $"SELECT * FROM visa_applications WHERE user_id = '{googleId}'";
                return await db.QueryAsync<Visa>(sql);
            }
        }

        public async Task<Visa?> GetVisaApplicationByOfficerId(string officerId){
            using (var db = CreateDBConnection()){
                var sql= $"SELECT * FROM visa_applications WHERE officer_id = '{officerId}' OR officer_id IS NULL ORDER BY submitted_at ASC;";
                var applications = await db.QueryAsync<Visa>(sql);
                var openApplications =  applications.Where(app => app.OfficerId != null);
                foreach(var app in openApplications){
                    var statusSql = $"SELECT * FROM statuses WHERE status_id = {app.StatusId}";
                    var status = await db.QueryFirstAsync<Status>(statusSql);
                    if(status.Name == "PENDING"){
                        return app; 
                    }
                }
                // just return the first application without an officerId
                return new List<Visa>(applications.Where(app => app.OfficerId == null)).FirstOrDefault();
            }
        } 

        public async override Task<bool> Exists(Visa model)
        {
            using var db = CreateDBConnection();
            var sql = @"
            SELECT COUNT(1)
            FROM visa_applications
            WHERE user_id = @UserId
            AND destination_planet = @DestinationPlanet
            AND start_date = @StartDate
            AND end_date = @EndDate;
        ";

            var count = await db.ExecuteScalarAsync<int>(sql, new
            {
                UserId = model.UserId,
                DestinationPlanet = model.DestinationPlanet,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            });

            return count > 0;
        }
    }
}
