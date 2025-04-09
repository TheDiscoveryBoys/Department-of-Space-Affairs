using System.ComponentModel;
using System.Net;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using DOSA_Client.Models;
using System.Net.Http;
using System.Net.Http.Json;
using DOSA_Client.lib.Constants;


public static class RestClient
{

    private static HttpClient HttpClient = new HttpClient();
    public static async Task<List<Role>> GetRolesByGoogleId(string googleId)
    {
        await Task.Delay(1000);
        return [
                new Role( 1, "OFFICER")
        ];
    }

    public static string DynStatus = "NOSTATUS";

    public static async Task<User> GetUserByGoogleId(string googleId)
    {
        await Task.Delay(1000);
        return new User(
                googleId,
                "cadesayner@gmail.com",
                "Cade Sayner",
                "Homo Sapien Sapien",
                "Earth",
                "English",
                DateTime.Now.AddYears(-1000)
        );
    }

    public static async Task<string?> GetJwt(string googleAuthCode)
    {
        var loginBody = new LoginPostBody(googleAuthCode);
        var response = await HttpClient.PostAsJsonAsync($"{Constants.BaseURI}auth/login", loginBody);
    
        if (response.IsSuccessStatusCode)
        {
            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
            Console.WriteLine("The token" + loginResponse?.Token);
            return loginResponse?.Token;
        }
        return null;
    }

    public static async Task<bool> CreateUser(User user){
        var response = await HttpClient.PostAsJsonAsync($"{Constants.BaseURI}api/users", user);
        Console.WriteLine("The content" + await response.Content.ReadAsStringAsync());
        if (response.IsSuccessStatusCode)
        {
            await response.Content.ReadFromJsonAsync<User>();
            return true;
        }
        Console.WriteLine(await response.Content.ReadAsStringAsync());
        return false;
    }

    public static async Task<List<PassportApplication>> GetPassportApplicationsByGoogleId(string googleId)
    {
        await Task.Delay(1000);
        var passport1 = new PassportApplication(1, "googleID", new Status(DynStatus), DateTime.Now, DateTime.Now.AddDays(-1), null);
        // var passport2 = new PassportApplication(1, "googleID", new Status("APPROVED"), DateTime.Now, DateTime.Now.AddDays(-1), null);
        return DynStatus == "PENDING" ? [passport1] : [];
    }

    public static async Task<bool> UpdateUserDetails(User user)
    {
        Console.WriteLine("Updating user details");
        await Task.Delay(1000);
        return true;
    }

    public static async Task<bool> UpdateVisaApplicationStatus(Status status, int applicationId)
    {
        Console.WriteLine("Updating Visa application status");
        await Task.Delay(1000);
        return true;
    }

    public static async Task<bool> UpdatePassportApplicationStatus(Status status, int applicationId)
    {
        Console.WriteLine("Updating Passport Application Status");
        await Task.Delay(1000);
        return true;
    }

    public static async Task<List<VisaApplication>> GetVisaApplicationsByGoogleId(string googleId)
    {
        await Task.Delay(1000);
        var visa1 = new VisaApplication(1, "1", "Mars", "Better Jobs", DateTime.Now, DateTime.Now.AddDays(2), null, new Status("PENDING"), DateTime.Now.AddDays(-10), null);
        return [];
    }

    public static async Task PostFile(string filePath)
    {
        // hit the endpoint with the file to upload
        await Task.Delay(10);
        Console.WriteLine($"Posting file: {filePath}");
    }

    public static async Task<VisaApplication> GetOfficerVisaApplicationByGoogleId(string googleId)
    {
        await Task.Delay(1000);
        return new VisaApplication(1, "1", "Hoth", "Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here ", DateTime.Now, DateTime.Now.AddDays(3), null, new Status("PENDING", null), DateTime.Now, DateTime.Now);
    }

    public static async Task<PassportApplication> GetOfficerPassportApplicationByGoogleId(string officerId)
    {
        await Task.Delay(1000);
        return new PassportApplication(1, "1", new Status("PENDING", null), DateTime.Now, DateTime.Now, null);
    }

    public static async Task<List<ApplicationDocument>> GetApplicationDocumentsByApplicationId(int applicationId)
    {
        await Task.Delay(1000);
        return [new ApplicationDocument(3,"ID Document", "https://google.com", 1) ,
         new ApplicationDocument(4, "Proof of Address", "https://google.com", 1)];
    }
}