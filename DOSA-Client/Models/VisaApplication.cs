using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSA_Client.Models
{
    public enum ApplicationStatus
    {
        Approved,
        Pending,
        Rejected,
        Busy
    }

    class VisaApplication
    {
        public User Applicant { get; set; }
        public string DestinationPlanet { get; set; }
        public string TravelReason { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int OfficerId { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }

        public VisaApplication()
        {

        }
    }
}
