
using System.Text.Json.Serialization;

namespace IntergalacticPassportAPI.Models
{
    public class Users
    {
        [PrimaryKey(AutoGenerated = false)]
        public string google_id{ get; set; }

        public string email { get; set; }

        public string name { get; set; }

        public string species { get; set; }

        public string planet_of_origin { get; set; }

        public string primary_language { get; set; }

        public DateTime? date_of_birth { get; set; }
    }
}
