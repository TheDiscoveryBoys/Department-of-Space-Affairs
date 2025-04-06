using IntergalacticPassportAPI.Models;

namespace IntergalacticPassportAPI.Data
{
    public class UsersRepository(IConfiguration config) : BaseRepository<Users>(config, "users")
    {
        public async Task<Users> RegisterUser(Users user)
        {
            try
            {
                return await Create(user);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the user.", ex);
            }

        }

        public async Task<IEnumerable<Users>> GetAllUsers()
        {
            return await GetAll();
        }

        public async Task<Users> GetUserByGoogleId(string id)
        {
            return await GetById(id, "google_id");
        }
    }
}

