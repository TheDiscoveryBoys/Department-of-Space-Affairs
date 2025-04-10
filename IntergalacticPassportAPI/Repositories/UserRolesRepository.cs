using IntergalacticPassportAPI.Models;

namespace IntergalacticPassportAPI.Data
{
        public class UserRolesRepository(IConfiguration config) : BaseRepository<UserRoles>(config, "user_roles"){
            public async Task<UserRoles?> CreateUserRole(UserRoles userRoles){
                try
                {
                    return await Create(userRoles);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    return null;
                }
            }

        public override Task<bool> Exists(UserRoles model)
        {
            throw new NotImplementedException();
        }
    }

}