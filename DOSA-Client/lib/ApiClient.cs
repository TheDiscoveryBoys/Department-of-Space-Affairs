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
        // TODO: Probably make this private and change it through a function?
        public static string jwt{get; set;}
        public static List<Application> GetApplications(string googleId)
        {
            var applications = new List<Application>();
            var passport1 = new Application(new Status("REJECTED", "Applicant did not provide the correct documents. Not clear, REJECTED. Applicant did not provide the correct documents. Not clear, REJECTED. Applicant did not provide the correct documents. Not clear, REJECTED. Applicant did not provide the correct documents. Not clear, REJECTED. Applicant did not provide the correct documents. Not clear, REJECTED."), DateTime.Now.AddDays(-2), DateTime.Now, ApplicationType.Passport);
            var passport2 = new Application(new Status("APPROVED"), DateTime.Now.AddDays(-1), ApplicationType.Passport);
            var visa1 = new Application(new Status("PENDING"), DateTime.Now, ApplicationType.Visa);
            applications.Add(passport1);
            // applications.Add(passport2);
            // applications.Add(visa1);

            return applications.OrderByDescending(app => app.SubmittedAt).ToList();
        }

        public static String ExchangeAuthCodeForJWT(string authCode){
            return "This is a fake jwt";
        }

        public static User GetUserProfile(string googleId){
            return new User{
                GoogleId = googleId,
                DateOfBirth = DateTime.Now.AddYears(-1000),
                Email = "cadesayner@gmail.com",
                Name = "Cade Sayner",
                PlanetOfOrigin = "Earth",
                PrimaryLanguage = "English",
                Species = "Homo Sapien Sapien"
            };
        }

        public static List<Role> GetRoles(string googleId){
            return new List<Role>{
                new Role{id = 1, role="APPLICANT"}
            };
        }

    }
}
