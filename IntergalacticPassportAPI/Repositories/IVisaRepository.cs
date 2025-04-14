using System;
using IntergalacticPassportAPI.Models;

namespace IntergalacticPassportAPI.Data
{
    public interface IVisaRepository : IBaseRepository<Visa>
    {
        Task<IEnumerable<Visa>> GetVisaApplicationsByGoogleId(string googleId);
        Task<Visa?> GetVisaApplicationByOfficerId(string officerId);
    }

}
