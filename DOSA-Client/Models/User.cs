using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSA_Client.Models
{
    public class User
    {
        public string GoogleId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string? Species { get; set; }
        public string? PlanetOfOrigin { get; set; }
        public string? PrimaryLanguage { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public User()
        {
        }

    }
}
