using Dapper;
using IntergalacticPassportAPI.Models;
using Npgsql;
using System.Data;

namespace IntergalacticPassportAPI.Data
{
    public class PassportDocumentRepository(IConfiguration config) : BaseRepository<PassportDocument>(config), IPassportDocumentRepository
    {

        public async Task<IEnumerable<PassportDocument>> GetByPassportApplicationIdAsync(int id)
        {
            using var connection = CreateDBConnection();
            return await connection.QueryAsync<PassportDocument>(
                "SELECT * FROM passport_application_documents WHERE passport_application_id = @Id;", new { Id = id });
        }
        public override async Task<bool> Exists(PassportDocument model)
        {
            var existingRoles = await GetAll();
            return existingRoles.Any(r => r.Id == model.Id);
        }
    }
}
