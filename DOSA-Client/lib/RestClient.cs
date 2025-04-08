using System.ComponentModel;
using System.Windows.Documents;
using DOSA_Client.Models;

public static class RestClient{
    public static async Task<List<Role>> GetRolesByGoogleId(string googleId){
        await Task.Delay(1000);
        return [
                new Role( 1, "APPLICANT")
        ];
    }

    public static string DynStatus = "NOSTATUS";

    public static async Task<User> GetUserByGoogleId(string googleId){
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

    public static async Task<string> GetJwt(string googleAuthCode){
        await Task.Delay(1000);
        return "This is a fake jwt";
    }

    public static async Task<List<PassportApplication>> GetPassportApplicationsByGoogleId(string googleId){
        await Task.Delay(1000);
        var passport1 = new PassportApplication(1, "googleID", new Status(DynStatus), DateTime.Now, DateTime.Now.AddDays(-1), null);
        // var passport2 = new PassportApplication(1, "googleID", new Status("APPROVED"), DateTime.Now, DateTime.Now.AddDays(-1), null);
        return DynStatus == "PENDING" ?  [passport1] : [];
    }

    public static async Task<List<VisaApplication>> GetVisaApplicationsByGoogleId(string googleId){
        await Task.Delay(1000);
        var visa1 = new VisaApplication("Mars", "Better Jobs", DateTime.Now, DateTime.Now.AddDays(2), null, new Status("PENDING"), DateTime.Now.AddDays(-10), null);
        return [];
    }

    public static async Task PostFile(string filePath){
        // hit the endpoint with the file to upload
        await Task.Delay(10);
        Console.WriteLine($"Posting file: {filePath}");
    }

    public static async Task<VisaApplication> GetOfficerVisaApplicationByGoogleId(string googleId){
        return new VisaApplication("Hoth", "Need to get a tan out here", DateTime.Now, DateTime.Now.AddDays(3), null, new Status("PENDING", null) , DateTime.Now, DateTime.Now);
    }
}