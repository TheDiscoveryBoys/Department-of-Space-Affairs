using Amazon.S3.Encryption.Internal;
using Dapper;
using IntergalacticPassportAPI.Models;
using Npgsql;
using System.Data;
using System.Text.Json;

namespace IntergalacticPassportAPI.Data
{
    public class PassportRepository(IConfiguration config) : BaseRepository<Passport>(config, "passport_applications")
    {
        //     private readonly IConfiguration _configuration;

        //     public PassportRepository(IConfiguration configuration)
        //     {
        //         _configuration = configuration;
        //     }

        //     private IDbConnection CreateConnection()
        //     {
        //         return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        //     }

        //     public async Task<IEnumerable<Passport>> GetAllAsync()
        //     {
        //         using var connection = CreateConnection();
        //         return await connection.QueryAsync<Passport>("SELECT id, user_id, status_id,  submitted_at,  processed_at,  officer_id FROM passport_applications ORDER BY id;");
        //     }
        public async override Task<bool> Exists(Passport passport)
        {
            var existingPassport = await GetById((passport.Id).ToString());
            if (existingPassport == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<IEnumerable<Passport>> GetPassportApplicationsByGoogleId(string googleId){
            using (var db = CreateDBConnection()){
                var sql = $"SELECT * FROM passport_applications WHERE user_id = '{googleId}';";
                return await db.QueryAsync<Passport>(sql);
            }
        }

        public async Task<Passport?> GetPassportApplicationByOfficerId(string officerId){
             using (var db = CreateDBConnection()){
                var sql= $"SELECT * FROM passport_applications WHERE officer_id = '{officerId}' OR officer_id IS NULL ORDER BY submitted_at ASC;";
                Console.WriteLine(sql);
                var applications = await db.QueryAsync<Passport>(sql);
                Console.WriteLine($"Applications: {JsonSerializer.Serialize(applications)}");
                var openApplications =  applications.Where(app => app.OfficerId != null);
                Console.WriteLine($"Open applications: {JsonSerializer.Serialize(openApplications)}");
                foreach(var app in openApplications){
                    var statusSql = $"SELECT * FROM statuses WHERE id = {app.StatusId}";
                    Console.WriteLine(statusSql);
                    var status = await db.QueryFirstAsync<Status>(statusSql);
                    Console.WriteLine($"Status: {JsonSerializer.Serialize(status)}: {status.Name == "PENDING"}");
                    if(status.Name == "PENDING"){
                        Console.WriteLine($"Returning application: {JsonSerializer.Serialize(app)}");
                        return app; 
                    }
                }
                // just return the first application without an officerId
                return new List<Passport>(applications.Where(app => app.OfficerId == null)).FirstOrDefault();
            }
        } 

//private readonly IConfiguration _configuration;


        //     public async Task<Passport?> GetByIdAsync(int id)
        //     {
        //         using var connection = CreateConnection();
        //         return await connection.QueryFirstOrDefaultAsync<Passport>(
        //             "SELECT * FROM passport_applications WHERE id = @Id;", new { Id = id });
        //     }

        //     public async Task<int> CreateAsync(Passport passport)
        //     {
        //         try
        //         {
        //             using var connection = CreateConnection();
        //             var sql = @"INSERT INTO passport_applications (user_id, status_id, submitted_at, processed_at, officer_id)
        //                     VALUES (@UserId, @StatusId, @SubmittedAt, @ProcessedAt)
        //                     RETURNING id;";
        //             return await connection.ExecuteScalarAsync<int>(sql, passport);
        //         }

        //         catch (Exception ex) 
        //         {
        //             throw new Exception("An error occurred while creating the passport.", ex);
        //         }

        //    catch (Exception ex) 
        //    {
        //        throw new Exception("An error occurred while creating the passport.", ex);
        //    }

        //}
    }
}
