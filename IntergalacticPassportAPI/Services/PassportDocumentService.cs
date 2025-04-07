using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;

namespace IntergalacticPassportAPI.Services
{
    public class PassportDocumentService
    {
        private readonly PassportDocumentRepository _repo;

        public PassportDocumentService(PassportDocumentRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<PassportDocument>> GetAllAsync()
        {
            return _repo.GetAllAsync();
        }

        public Task<IEnumerable<PassportDocument>> GetByPassportApplicationIdAsync(int id)
        { 
            return _repo.GetByPassportApplicationIdAsync(id);
        }

        public Task<PassportDocument?> GetByIdAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }

        public Task<int> CreateAsync(PassportDocument passportDocument)
        {
            return _repo.CreateAsync(passportDocument);
        }
    }
}

