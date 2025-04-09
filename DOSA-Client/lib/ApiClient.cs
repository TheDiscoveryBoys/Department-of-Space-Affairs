using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using DOSA_Client.Models;
using DOSA_Client.ViewModels;
using static DOSA_Client.ViewModels.UploadPassportDocumentsViewModel;

namespace DOSA_Client.lib
{
    static class ApiClient
    {
        // TODO: Probably make this private and change it through a function?
        public static string? Jwt{get; set;}
        public static async Task<List<Application>> GetApplications(string googleId)
        {
            var passportApplicationsTask = RestClient.GetPassportApplicationsByGoogleId(googleId);
            var visaApplicationsTask = RestClient.GetVisaApplicationsByGoogleId(googleId);
            await Task.WhenAll(passportApplicationsTask, visaApplicationsTask);
            var passportApplications = passportApplicationsTask.Result;
            var visaApplications = visaApplicationsTask.Result;
            var applications = new List<Application>();
            foreach (var passportApplication in passportApplications){
                applications.Add(new Application(passportApplication.Status, passportApplication.SubmittedAt, ApplicationType.Passport, passportApplication.ProcessedAt));
            }
            foreach (var visaApplication in visaApplications){
                applications.Add(new Application(visaApplication.Status, visaApplication.SubmittedAt, ApplicationType.Passport, null));
            }
            Console.WriteLine(applications.Count);
            return applications;
        }

        public static async Task<bool> UpdateUser(User user){
            return await RestClient.UpdateUser(user);
        }
    
        public static async Task<string> ExchangeAuthCodeForJWT(string authCode){
            return await RestClient.GetJwt(authCode);
        }

        public static async Task<bool> CreateUser(string sub, string email, string name){
            User user = new User(sub, email, name, null, null,null,null);
            return await RestClient.CreateUser(user);
        }
        public static async Task<User> GetUserProfile(string googleId){
            return await RestClient.GetUserByGoogleId(googleId);
        }

        public static async Task<List<Role>> GetRoles(string googleId){
            return await RestClient.GetRolesByGoogleId(googleId);
        }

        public static async Task UploadFiles(List<LocalFile> fileNames){
            List<Task> tasks = [];

            foreach(var file in fileNames){
                tasks.Add(RestClient.PostFile(file));
            }
            await Task.WhenAll(tasks);
        }

        public static async Task<OfficerVisaApplication> GetVisaApplication(string googleId){
            var visaApplication = await RestClient.GetOfficerVisaApplicationByGoogleId(googleId);
            User user = await RestClient.GetUserByGoogleId(visaApplication.UserId);
            return new OfficerVisaApplication(visaApplication,user);
        }

        public static async Task<OfficerPassportApplication> GetPassportApplication(string officerId){
            var passportApplication = await RestClient.GetOfficerPassportApplicationByGoogleId(officerId);
            User user = await RestClient.GetUserByGoogleId(passportApplication.UserId);
            List<ApplicationDocument> documents = await RestClient.GetApplicationDocumentsByApplicationId(passportApplication.Id);
            
            return new OfficerPassportApplication(passportApplication, user, documents);
        }


        public static async Task<bool> UpdateUserDetails(User user){
            return await RestClient.UpdateUserDetails(user);
        }
        public static async Task<bool> UpdateVisaApplicationStatus (Status status, int applicationId){
            return await RestClient.UpdateVisaApplicationStatus(status, applicationId);
        }
        public static async Task<bool> UpdatePassportApplicationStatus (Status status, int applicationId){
            return await RestClient.UpdatePassportApplicationStatus(status, applicationId);
        }
    }
}
