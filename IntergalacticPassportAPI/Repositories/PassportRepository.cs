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
            return await connection.QueryAsync<Passport>("SELECT * FROM passports ORDER BY id;");
        }

        public async Task<Passport?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Passport>(
                "SELECT * FROM passports WHERE id = @Id;", new { Id = id });
        }

        public async Task<int> CreateAsync(Passport passport)
        {
            using var connection = CreateConnection();
            var sql = @"INSERT INTO passports (name, species, planet, issued_date)
                        VALUES (@Name, @Species, @Planet, @IssuedDate)
                        RETURNING id;";
            return await connection.ExecuteScalarAsync<int>(sql, passport);
        }
    }
}
