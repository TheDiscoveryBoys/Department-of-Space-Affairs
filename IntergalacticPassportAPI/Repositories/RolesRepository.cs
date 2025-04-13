using IntergalacticPassportAPI.Models;

namespace IntergalacticPassportAPI.Data
{
    public class RolesRepository(IConfiguration config) : BaseRepository<Roles>(config)
    {

        public override async Task<bool> Exists(Roles model)
        {
           var existingRoles = await GetAll();
            return existingRoles.Any(r => r.Role.Equals(model.Role, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}