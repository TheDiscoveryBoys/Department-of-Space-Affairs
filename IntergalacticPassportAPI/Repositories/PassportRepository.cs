using Amazon.S3.Encryption.Internal;
using Dapper;
using IntergalacticPassportAPI.Models;
using Npgsql;
using System.Data;
using System.Text.Json;

namespace IntergalacticPassportAPI.Data
{
    public class PassportRepository(IConfiguration config) : BaseRepository<Passport>(config, "passport_applications"), IPassportRepository
    {
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
    }
}
