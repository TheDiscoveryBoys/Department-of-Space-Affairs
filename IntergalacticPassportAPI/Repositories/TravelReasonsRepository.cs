using Dapper;
using IntergalacticPassportAPI.Models;

namespace IntergalacticPassportAPI.Data
{
    public class TravelReasonsRepository(IConfiguration config) : BaseRepository<TravelReasons>(config), ITravelReasonsRepository
    {
        public override async Task<bool> Exists(TravelReasons model)
        {
            var travelReasons = await GetAll();
            return travelReasons.Any(tr => tr.Reason.Equals(model.Reason, StringComparison.CurrentCultureIgnoreCase));
        }

        public override async Task<IEnumerable<TravelReasons>> GetAll()
        {
            return await base.GetAll();
        }

    }
}