using System;
using IntergalacticPassportAPI.Models;

namespace IntergalacticPassportAPI.Data
{
    public interface IPassportRepository : IBaseRepository<Passport>
    {
        Task<IEnumerable<Passport>> GetPassportApplicationsByGoogleId(string googleId);
        Task<Passport?> GetPassportApplicationByOfficerId(string officerId);
    }

}
