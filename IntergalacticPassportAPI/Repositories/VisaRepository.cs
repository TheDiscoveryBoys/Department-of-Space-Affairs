using Dapper;
using IntergalacticPassportAPI.Models;
using Npgsql;
using System.Data;

namespace IntergalacticPassportAPI.Data
{
	public class VisaRepository
	{
		private readonly IConfiguration _configuration;

		public VisaRepository(IConfiguration configuration) 
		{
            _configuration = configuration;
        }

        private IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<Visa>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Visa>("SELECT * FROM visa_applications ORDER BY id;");

        }

        public async Task<Visa?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Visa>(
                "SELECT * FROM visa_applications WHERE id = @Id;", new { Id = id });
        }

        public async Task<int> CreateAsync(Visa visa)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"INSERT INTO visa_applications (user_id, destination_planet, travel_reason, start_date, end_date, status_id, submitted_at, processed_at, officer_id) 
                  VALUES (@UserId, @DestinationPlanet, @TravelReason, @StartDate, @EndDate, @StatusId, @SubmittedAt, @ProcessedAt, @OfficerId)
                  RETURNING id;";
                return await connection.ExecuteScalarAsync<int>(sql, visa);
            }
            catch (Exception ex) 
            {
                throw new Exception("An error occurred while creating the visa.", ex);
            }
            
        }
    }
}
