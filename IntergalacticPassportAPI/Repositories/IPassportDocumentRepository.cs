using System;
using IntergalacticPassportAPI.Models;

namespace IntergalacticPassportAPI.Data
{
    public interface IPassportDocumentRepository : IBaseRepository<PassportDocument>
    {
        Task<IEnumerable<PassportDocument>> GetByPassportApplicationIdAsync(int id);
    }

}
