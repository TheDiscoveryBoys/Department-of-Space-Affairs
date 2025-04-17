using Dapper;
using IntergalacticPassportAPI.Models;
using System.Data;

namespace IntergalacticPassportAPI.Data
{
    public class VisaRepository(IConfiguration config) : BaseRepository<VisaApplication>(config), IVisaRepository
    {

        public async Task<IEnumerable<VisaApplication>> GetVisaApplicationsByGoogleId(string googleId)
        {
            using (var db = CreateDBConnection())
            {
                var sql = $"SELECT * FROM visa_applications WHERE user_id = '{googleId}'";
                Console.WriteLine(sql);
                return await db.QueryAsync<VisaApplication>(sql);
            }
        }

        public async Task<VisaApplication?> GetVisaApplicationByOfficerId(string officerId)
        {
            using (var db = CreateDBConnection())
            {
                var sql = $"SELECT * FROM visa_applications WHERE officer_id = '{officerId}' OR officer_id IS NULL ORDER BY submitted_at ASC;";
                var applications = await db.QueryAsync<VisaApplication>(sql);
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
                return new List<VisaApplication>(applications.Where(app => app.OfficerId == null)).FirstOrDefault();
            }
        }

        public async override Task<bool> Exists(VisaApplication model)
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
