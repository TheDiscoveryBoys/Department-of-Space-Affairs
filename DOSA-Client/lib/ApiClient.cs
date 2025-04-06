using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DOSA_Client.Models;

namespace DOSA_Client.lib
{
    static class ApiClient
    {
        public static List<PassportApplication> getPassportApplications(string googleId)
        {
            var passports = new List<PassportApplication>();
            var passport1 = new PassportApplication(new Status("PENDING"), DateTime.Now);
            var passport2 = new PassportApplication(new Status("REJECTED", "Applicant did not provide the correct documents. Not clear, REJECTED. Applicant did not provide the correct documents. Not clear, REJECTED. Applicant did not provide the correct documents. Not clear, REJECTED. Applicant did not provide the correct documents. Not clear, REJECTED. Applicant did not provide the correct documents. Not clear, REJECTED."), DateTime.Now.AddDays(-1));
            passports.Add(passport1);
            passports.Add(passport2);

            return passports;
        }
    }
}
