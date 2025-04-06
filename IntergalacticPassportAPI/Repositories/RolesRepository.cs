using IntergalacticPassportAPI.Models;

namespace IntergalacticPassportAPI.Data
{
    public class RolesRepository(IConfiguration config) : BaseRepository<Users>(config, "roles")
    {

    }
}