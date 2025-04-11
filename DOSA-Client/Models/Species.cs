using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSA_Client.Models
{
    public class SpeciesResponse
    {
        public List<Species> Results { get; set; }
    }

    public record Species(
        string Name
    );
}
