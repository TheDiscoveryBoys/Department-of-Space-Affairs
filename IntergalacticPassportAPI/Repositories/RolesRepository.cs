using Dapper;
using IntergalacticPassportAPI.Models;

namespace IntergalacticPassportAPI.Data
{
    public class RolesRepository(IConfiguration config) : BaseRepository<Roles>(config, "roles")
    {

        public override async Task<bool> Exists(Roles model)
        {
            var existingRoles = await GetAll();
            return existingRoles.Any(r => r.Role.Equals(model.Role, StringComparison.CurrentCultureIgnoreCase));
        }

        public async Task<Roles> GetRolesByName(string name)
        {
            using (var db = CreateDBConnection())
            {
                var sql = $"SELECT * FROM roles where role = '{name}'";
                return db.QueryFirst<Roles>(sql);
            }
        }
    }
}