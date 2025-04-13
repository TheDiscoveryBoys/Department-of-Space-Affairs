using Dapper;
using IntergalacticPassportAPI.Models;
using Npgsql;
using System.Data;

namespace IntergalacticPassportAPI.Data
{
    public class StatusRepository(IConfiguration config) : BaseRepository<Status>(config), IStatusRepository
    {
        public override async Task<bool> Exists(Status model)
        {
            var existingModel = await GetById(model.Id.ToString());
            if(existingModel != null){
                return true;
            } else{
                return false;
            }
        }
    }
}
