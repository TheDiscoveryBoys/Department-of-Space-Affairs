using Amazon.S3.Encryption.Internal;
using Dapper;
using IntergalacticPassportAPI.Models;
using Npgsql;
using System.Data;
using System.Text.Json;

namespace IntergalacticPassportAPI.Data
{
    public class PassportRepository(IConfiguration config) : BaseRepository<Passport>(config), IPassportRepository
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

        public async Task<IEnumerable<Passport>> GetPassportApplicationsByGoogleId(string googleId)
        {
            using (var db = CreateDBConnection())
            {
                var sql = $"SELECT * FROM passport_applications WHERE user_id = '{googleId}';";
                return await db.QueryAsync<Passport>(sql);
            }
        }

        public async Task<Passport?> GetPassportApplicationByOfficerId(string officerId)
        {
            using (var db = CreateDBConnection())
            {
                var sql = $"SELECT * FROM passport_applications WHERE officer_id = '{officerId}' OR officer_id IS NULL ORDER BY submitted_at ASC;";

                var applications = await db.QueryAsync<Passport>(sql);

                var openApplications = applications.Where(app => app.OfficerId != null);

                foreach (var app in openApplications)
                {
                    var statusSql = $"SELECT * FROM application_statuses WHERE id = {app.StatusId}";

                    var status = await db.QueryFirstAsync<ApplicationStatus>(statusSql);

                    if (status.Name == "PENDING")
                    {

                        return app;
                    }
                }
                // just return the first application without an officerId
                return new List<Passport>(applications.Where(app => app.OfficerId == null)).FirstOrDefault();
            }
        }
    }
}
