using IntergalacticPassportAPI.Models;

namespace IntergalacticPassportAPI.Data
{
    public class UsersRepository(IConfiguration config) : BaseRepository<Users>(config, "users")
    {
        public async Task<Users> RegisterOrLoginUser(Users user)
        {
            try
            {
                var existingUser = GetUserByGoogleId(user.google_id);

                if (existingUser.Result == null)
                {
                    return await Create(user);
                }
                else
                {
                    return existingUser.Result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the passport.", ex);
            }

        }

        public async Task<IEnumerable<Users>> GetAllUsers(){
           return await GetAll();
        }

        public async Task<Users> GetUserByGoogleId(string id)
        {
             return await GetById(id, "google_id");
        }
    }
}

