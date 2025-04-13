using System;
using IntergalacticPassportAPI.Models;

namespace IntergalacticPassportAPI.Data
{
    public interface IRolesRepository : IBaseRepository<Roles>
    {
        Task<Roles> GetRolesByName(string name);
    }

}
