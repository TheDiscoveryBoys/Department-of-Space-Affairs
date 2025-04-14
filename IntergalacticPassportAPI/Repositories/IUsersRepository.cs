using System;
using IntergalacticPassportAPI.Models;

namespace IntergalacticPassportAPI.Data
{
    public interface IUsersRepository : IBaseRepository<Users>
    {
        Task<IEnumerable<Roles>> GetUserRoles(string googleId);
        Task<Users?> GetUserByEmail(string email);
        Task<bool> AssignRoleToUser(string googleId, int roleId);
    }
}
