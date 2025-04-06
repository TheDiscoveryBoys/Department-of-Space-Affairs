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
        public static List<Application> getApplications(string googleId)
        {
            var applications = new List<Application>();
            var passport2 = new Application(new Status("REJECTED", "Applicant did not provide the correct documents. Not clear, REJECTED. Applicant did not provide the correct documents. Not clear, REJECTED. Applicant did not provide the correct documents. Not clear, REJECTED. Applicant did not provide the correct documents. Not clear, REJECTED. Applicant did not provide the correct documents. Not clear, REJECTED."), DateTime.Now.AddDays(-2), DateTime.Now, ApplicationType.Passport);
            var passport1 = new Application(new Status("APPROVED"), DateTime.Now.AddDays(-1), ApplicationType.Passport);
            var visa1 = new Application(new Status("PENDING"), DateTime.Now, ApplicationType.Visa);
            applications.Add(passport1);
            applications.Add(passport2);
            applications.Add(visa1);

            return applications.OrderByDescending(app => app.SubmittedAt).ToList();
        }
    }
}
