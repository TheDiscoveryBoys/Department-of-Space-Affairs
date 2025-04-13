using System;
using IntergalacticPassportAPI.Models;

namespace IntergalacticPassportAPI.Data
{
    public interface IUserRolesRepository : IBaseRepository<UserRoles>
    {
        Task<UserRoles?> CreateUserRole(UserRoles userRoles);
    }

}
