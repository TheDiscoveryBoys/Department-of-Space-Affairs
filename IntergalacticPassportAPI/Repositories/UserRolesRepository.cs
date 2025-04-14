using IntergalacticPassportAPI.Models;

namespace IntergalacticPassportAPI.Data
{
    public class UserRolesRepository(IConfiguration config) : BaseRepository<UserRoles>(config), IUserRolesRepository
    
    {
        public override Task<bool> Exists(UserRoles model)
        {
            throw new NotImplementedException();
        }
    }

}