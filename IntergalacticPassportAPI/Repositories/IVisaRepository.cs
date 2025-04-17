using System;
using IntergalacticPassportAPI.Models;

namespace IntergalacticPassportAPI.Data
{
    public interface IVisaRepository : IBaseRepository<VisaApplication>
    {
        Task<IEnumerable<VisaApplication>> GetVisaApplicationsByGoogleId(string googleId);
        Task<VisaApplication?> GetVisaApplicationByOfficerId(string officerId);
    }

}
