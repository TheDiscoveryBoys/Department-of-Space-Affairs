using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        public static string? Jwt { get; set; }
        public static async Task<List<Application>> GetApplications(string googleId)
        {
            var passportApplicationsTask = RestClient.GetPassportApplicationsByGoogleId(googleId);
            var visaApplicationsTask = RestClient.GetVisaApplicationsByGoogleId(googleId);
            await Task.WhenAll([passportApplicationsTask, visaApplicationsTask]);
            var passportApplications = passportApplicationsTask.Result;
            var visaApplications = visaApplicationsTask.Result;
            var applications = new List<Application>();
            foreach (var passportApplication in passportApplications)
            {
                var Status = await RestClient.GetStatusByStatusId(passportApplication.StatusId ?? throw new Exception("An application must have a status id"));
                applications.Add(new Application(Status, passportApplication.SubmittedAt, "PASSPORT", passportApplication.ProcessedAt));
            }
            foreach (var visaApplication in visaApplications)
            {
                var Status = await RestClient.GetStatusByStatusId(visaApplication.StatusId ?? throw new Exception("An application must have a status id"));
                var formattedStartDate = visaApplication.StartDate?.ToString("MM/dd/yy");
                var formattedEndDate = visaApplication.EndDate?.ToString("MM/dd/yy"); ;
                applications.Add(new Application(Status, visaApplication.SubmittedAt, $"VISA - {visaApplication.DestinationPlanet} ({formattedStartDate} - {formattedEndDate})", null));
            }
            Console.WriteLine(applications.Count);
            return [.. applications.OrderByDescending(app => app.SubmittedAt)];
        }

        public static async Task<bool> UpdateUser(User user)
        {
            return await RestClient.UpdateUser(user);
        }

        public static async Task<PassportApplication?> CreatePassportApplication(PassportApplication passportApplication)
        {
            return await RestClient.CreatePassportApplication(passportApplication);
        }

        public static async Task<VisaApplication> CreateVisaApplication(VisaApplication visaApplication)
        {
            return await RestClient.CreateVisaApplication(visaApplication);
        }

        public static async Task<string> ExchangeAuthCodeForJWT(string authCode)
        {
            return await RestClient.GetJwt(authCode);
        }

        public static async Task<User> GetUserProfile(string googleId)
        {
            return await RestClient.GetUserByGoogleId(googleId);
        }

        public static async Task<List<Role>> GetUserRoles(string googleId)
        {
            return await RestClient.GetRolesByGoogleId(googleId);
        }

        public static async Task<User?> GetUserByEmail(string email)
        {
            return await RestClient.GetUserByEmail(email);
        }

        public static async Task<bool> AddUserRole(UserRole userRole)
        {
            return await RestClient.AddUserRole(userRole);
        }

        public static async Task<bool> RemoveUserRole(UserRole userRole)
        {
            return await RestClient.RemoveUserRole(userRole);
        }

        public static async Task<List<Role>> GetAllRoles()
        {
            return await RestClient.GetRoles();
        }

        public static async Task UploadFiles(List<LocalFile> fileNames, int applicationID)
        {
            List<Task> tasks = [];

            foreach (var file in fileNames)
            {
                tasks.Add(RestClient.PostFile(file, applicationID));
            }
            await Task.WhenAll(tasks);
        }

        public static async Task<OfficerVisaApplication?> GetVisaApplication(string googleId)
        {
            try
            {
                var visaApplication = await RestClient.GetOfficerVisaApplicationByGoogleId(googleId);
                User? user = await RestClient.GetUserByGoogleId(visaApplication.UserId) ?? throw new Exception("An application with no user was returned");
                return new OfficerVisaApplication(visaApplication, user);
            }
            catch (Exception e)
            {
                Console.WriteLine("The issue is");
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static async Task<OfficerPassportApplication?> GetPassportApplication(string officerId)
        {
            try
            {
                var passportApplication = await RestClient.GetOfficerPassportApplicationByGoogleId(officerId) ?? throw new Exception("No more applications for you");
                User? user = await RestClient.GetUserByGoogleId(passportApplication.UserId) ?? throw new Exception("An application with no user was returned");
                List<ApplicationDocument>? documents;
                try
                {
                    documents = await RestClient.GetApplicationDocumentsByApplicationId(passportApplication.Id ?? 0);
                }
                catch (Exception e) { documents = []; }
                return new OfficerPassportApplication(passportApplication, user, documents ?? []);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static async Task<bool> UpdateUserDetails(User user)
        {
            return await RestClient.UpdateUserDetails(user);
        }
        public static async Task<bool> UpdateApplicationStatus(Status status)
        {
            return await RestClient.UpdateApplicationStatus(status);
        }


        public static async Task<bool> ProcessVisaApplication(VisaApplication visa, Status status)
        {
            // update status
            if (!await RestClient.UpdateApplicationStatus(status))
            {
                return false;
            }

            // update visa
            return await RestClient.UpdateVisaApplication(visa);
        }

        public static async Task<bool> ProcessPassportApplication(PassportApplication passport, Status status)
        {
            // update status
            if (!await RestClient.UpdateApplicationStatus(status))
            {
                return false;
            }

            // update passport
            return await RestClient.UpdatePassportApplication(passport);
        }

        public static async Task<bool> UpdatePassportApplication(PassportApplication passport, Status status)
        {
            // update passport
            return await RestClient.UpdatePassportApplication(passport);
        }

    }
}
