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

        public static async Task<String> ExchangeAuthCodeForJWT(string authCode){
            return await RestClient.GetJwt(authCode);
        }

        public static async Task<User> GetUserProfile(string googleId){
            return await RestClient.GetUserByGoogleId(googleId);
        }

        public static async Task<List<Role>> GetRoles(string googleId){
            return await RestClient.GetRolesByGoogleId(googleId);
        }

        public static async Task UploadFiles(List<string> fileNames){
            List<Task> tasks = [];
            foreach(var file in fileNames){
                tasks.Add(RestClient.PostFile(file));
            }
            await Task.WhenAll(tasks);
        }
    }
}
