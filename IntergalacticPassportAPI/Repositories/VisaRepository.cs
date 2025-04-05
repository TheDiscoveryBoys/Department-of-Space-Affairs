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
            return await connection.QueryAsync<Visa>("SELECT * FROM visa ORDER BY id;");
        }

        public async Task<Visa?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Visa>(
                "SELECT * FROM visa WHERE id = @Id;", new { Id = id });
        }

        public async Task<int> CreateAsync(Visa visa)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"INSERT INTO visa (UserId, DestinationPlanet, TravelReason, StartDate, EndDate, StatusId, SubmittedAt, ProcessedAt, OfficerId) 
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
