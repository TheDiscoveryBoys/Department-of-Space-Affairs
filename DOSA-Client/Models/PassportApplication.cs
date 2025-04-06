using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSA_Client.Models
{

    public class PassportApplication
    {
        public Status Status { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }

        public PassportApplication(Status status, DateTime submittedAt)
        {
            Status = status;
            SubmittedAt = submittedAt;
        }
        public PassportApplication(Status status, DateTime submittedAt, DateTime processedAt)
        {
            Status = status;
            SubmittedAt = submittedAt;
            ProcessedAt = processedAt;
        }
    }
}
