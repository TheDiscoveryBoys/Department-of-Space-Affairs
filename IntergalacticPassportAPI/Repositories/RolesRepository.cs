using System.ComponentModel;
using IntergalacticPassportAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IntergalacticPassportAPI.Data
{
    public class RolesRepository(IConfiguration config) : BaseRepository<Roles>(config, "roles")
    {

        public override async Task<bool> Exists(Roles model)
        {
           var existingRoles = await GetAll();
            return existingRoles.Any(r => r.Role.Equals(model.Role, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}