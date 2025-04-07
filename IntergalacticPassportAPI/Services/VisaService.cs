using IntergalacticPassportAPI.Data;
using IntergalacticPassportAPI.Models;

namespace IntergalacticPassportAPI.Services
{
    public class VisaService
    {
        private readonly VisaRepository _repo;

        public VisaService(VisaRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Visa>> GetAllAsync()
        {
            return _repo.GetAllAsync();
        }

        public Task<Visa?> GetByIdAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }

        public Task<int> CreateAsync(Visa visa)
        {
            return _repo.CreateAsync(visa);
        }
    }
}

