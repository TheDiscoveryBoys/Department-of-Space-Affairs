using Dapper;
using IntergalacticPassportAPI.Models;
using Npgsql;
using System.Data;

namespace IntergalacticPassportAPI.Data
{
    public class PassportRepository
    {
        private readonly IConfiguration _configuration;

        public PassportRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<Passport>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<Passport>("SELECT id, user_id, status_id,  submitted_at,  processed_at,  officer_id FROM passport_applications ORDER BY id;");
        }

        public async Task<Passport?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Passport>(
                "SELECT * FROM passport_applications WHERE id = @Id;", new { Id = id });
        }

        public async Task<int> CreateAsync(Passport passport)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"INSERT INTO passport_applications (user_id, status_id, submitted_at, processed_at, officer_id)
                        VALUES (@UserId, @StatusId, @SubmittedAt, @ProcessedAt)
                        RETURNING id;";
                return await connection.ExecuteScalarAsync<int>(sql, passport);
            }

            catch (Exception ex) 
            {
                throw new Exception("An error occurred while creating the passport.", ex);
            }
            
        }
    }
}
