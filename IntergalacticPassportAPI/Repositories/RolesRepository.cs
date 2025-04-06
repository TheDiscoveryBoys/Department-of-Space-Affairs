using System.ComponentModel;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IntergalacticPassportAPI.Data
{
    public class RolesRepository(IConfiguration config) : BaseRepository<Roles>(config, "roles")
    {
        public async Task<Roles> GetRoleById(string id)
        {
            return await GetById(id, "id");
        }

        public async Task<IEnumerable<Roles>> GetAllRoles()
        {
            return await GetAll();
        }

        public async Task<Roles> CreateRole(Roles role)
        {
            try
            {
                return await Create(role);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the role.", ex);
            }
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            var existingRoles = await GetAllRoles();
            return existingRoles.Any(r => r.role.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));
        }

    }
}