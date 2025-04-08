using Dapper;
using IntergalacticPassportAPI.Models;
using Npgsql;
using System.Data;
using System.Runtime.CompilerServices;

namespace IntergalacticPassportAPI.Data
{
	public class VisaRepository(IConfiguration config) : BaseRepository<Visa>(config, "visa_applications")
    {

        public async override Task<bool> Exists(Visa visa)
        {

            using var db = CreateDBConnection();
            var sql = "SELECT * FROM visa_applications WHERE user_id = @UserId AND destination_planet = @DestinationPlanet AND status_id = 1";
            var existingVisa = await db.QueryFirstOrDefaultAsync<Visa>(sql, new { UserId = visa.UserId, DestinationPlanet = visa.DestinationPlanet });
            if (existingVisa == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //public async Task<IEnumerable<Visa>> GetAllAsync()
        //{
        //    using var connection = CreateConnection();
        //    return await connection.QueryAsync<Visa>("SELECT * FROM visa_applications ORDER BY id;");
        //}

        //public async Task<Visa?> GetByIdAsync(int id)
        //{
        //    using var connection = CreateConnection();
        //    return await connection.QueryFirstOrDefaultAsync<Visa>(
        //        "SELECT * FROM visa_applications WHERE id = @Id;", new { Id = id });
        //}

        //public async Task<int> CreateAsync(Visa visa)
        //{
        //    try
        //    {
        //        using var connection = CreateConnection();
        //        var sql = @"INSERT INTO visa_applications (user_id, destination_planet, travel_reason, start_date, end_date, status_id, submitted_at, processed_at, officer_id) 
        //          VALUES (@UserId, @DestinationPlanet, @TravelReason, @StartDate, @EndDate, @StatusId, @SubmittedAt, @ProcessedAt, @OfficerId)
        //          RETURNING id;";
        //        return await connection.ExecuteScalarAsync<int>(sql, visa);
        //    }
        //    catch (Exception ex) 
        //    {
        //        throw new Exception("An error occurred while creating the visa.", ex);
        //    }

        //}
    }
}
