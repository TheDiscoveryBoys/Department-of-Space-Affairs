using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSA_Client.Models
{
    public class Status
    {
        public string Name { get; set; }
        public string? Reason { get; set; }

        public Status(string name)
        {
            this.Name = name;
        }

        public Status(string name, string reason)
        {
            this.Name = name;
            this.Reason = reason;
        }
    }
}
