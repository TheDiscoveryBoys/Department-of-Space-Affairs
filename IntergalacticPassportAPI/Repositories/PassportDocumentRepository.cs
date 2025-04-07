using Dapper;
using IntergalacticPassportAPI.Models;
using Npgsql;
using System.Data;

namespace IntergalacticPassportAPI.Data
{
    public class PassportDocumentRepository
    {
        private readonly IConfiguration _configuration;

        public PassportDocumentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<PassportDocument>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<PassportDocument>("SELECT * FROM passport_application_documents ORDER BY id;");
        }

        public async Task<PassportDocument?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<PassportDocument>(
                "SELECT * FROM passport_application_documents WHERE id = @Id;", new { Id = id });
        }

        public async Task<IEnumerable<PassportDocument>> GetByPassportApplicationIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<PassportDocument>(
                "SELECT * FROM passport_application_documents WHERE passport_application_id = @Id;", new { Id = id });
        }

        public async Task<int> CreateAsync(PassportDocument passportDocument)
        {
            try
            {
                using var connection = CreateConnection();
                var sql = @"INSERT INTO passport_application_documents (id, filename, passport_application_id)
                        VALUES (@Id, @Filename, @PassportApplicationId)
                        RETURNING id;";
                return await connection.ExecuteScalarAsync<int>(sql, passportDocument);
            }

            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the passport application document.", ex);
            }

        }
    }
}
