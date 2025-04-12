using System.ComponentModel;
using System.Net;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using DOSA_Client.Models;
using System.Net.Http;
using System.Net.Http.Json;
using DOSA_Client.lib.Constants;
using System.Configuration;
using static DOSA_Client.ViewModels.UploadPassportDocumentsViewModel;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Windows;
using System.CodeDom;


public static class RestClient
{

    public static HttpClient HttpClient = new HttpClient();
    public static async Task<List<Role>> GetRolesByGoogleId(string googleId)
    {
        await Task.Delay(1000);
        return [
                new Role( 1, "APPLICANT")
        ];
    }

    public static string DynStatus = "APPROVED";

    public static async Task<User?> GetUserByGoogleId(string googleId)
    {
        var response = await HttpClient.GetAsync($"{Constants.BaseURI}api/users/{googleId}");
        if (response.IsSuccessStatusCode)
        {
            var user = await response.Content.ReadFromJsonAsync<User>();
            return user;
        }
        return null;
    }

    public static async Task<string?> GetJwt(string googleAuthCode)
    {
        var loginBody = new LoginPostBody(googleAuthCode);
        var response = await HttpClient.PostAsJsonAsync($"{Constants.BaseURI}auth/login", loginBody);

        if (response.IsSuccessStatusCode)
        {
            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return loginResponse?.Token;
        }else{
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        return null;
    }

    // TODO Change to return user
    public static async Task<bool> CreateUser(User user)
    {
        var response = await HttpClient.PostAsJsonAsync($"{Constants.BaseURI}api/users", user);
        if (response.IsSuccessStatusCode)
        {
            await response.Content.ReadFromJsonAsync<User>();
            return true;
        }
        Console.WriteLine(await response.Content.ReadAsStringAsync());
        return false;
    }

    public static async Task<bool> UpdateUser(User user)
    {
        var response = await HttpClient.PutAsJsonAsync($"{Constants.BaseURI}api/users", user);
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
        try
        {
            return await HttpClient.GetFromJsonAsync<List<PassportApplication>>($"{Constants.BaseURI}api/passport/user?google_id={googleId}") ?? throw new Exception("Failed to retrieve passport applications");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public static async Task<List<VisaApplication>> GetVisaApplicationsByGoogleId(string googleId)
    {
        try
        {
            return await HttpClient.GetFromJsonAsync<List<VisaApplication>>($"{Constants.BaseURI}api/visa/user?google_id={googleId}") ?? throw new Exception("Failed to retrieve visa applications");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public static async Task<bool> UpdateUserDetails(User user)
    {
        Console.WriteLine("Updating user details");
        await Task.Delay(1000);
        return true;
    }

    public static async Task<bool> UpdateApplicationStatus(Status status)
    {
        var response = await HttpClient.PutAsJsonAsync<Status>($"{Constants.BaseURI}api/status", status);
        if (response.IsSuccessStatusCode){
            return true;
        }
        return false;
    }

    public static async Task<bool> UpdatePassportApplication(PassportApplication passportApplication)
    {
        // update passport
        var response = await HttpClient.PutAsJsonAsync<PassportApplication>($"{Constants.BaseURI}api/passport", passportApplication);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        return false;
    }

    public static async Task<bool> UpdateVisaApplication(VisaApplication visaApplication)
    {
        // update passport
        var response = await HttpClient.PutAsJsonAsync<VisaApplication>($"{Constants.BaseURI}api/visa", visaApplication);
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        return false;
    }

    public static async Task<bool> UpdatePassportApplicationStatus(Status status, int applicationId)
    {
        Console.WriteLine("Updating Passport Application Status");
        await Task.Delay(1000);
        return true;
    }
    public static async Task<Status> GetStatusByStatusId(int statusId)
    {
        try{
            return await HttpClient.GetFromJsonAsync<Status>($"{Constants.BaseURI}api/status/{statusId}") ?? throw new Exception("Failed to fetch status");
        }catch(Exception e){
            Console.WriteLine(e);
            return null;
        }
    }
    public static async Task<bool?> PostFile(LocalFile filePath, int applicationId)
    {
        using var form = new MultipartFormDataContent();
        // Add the file content
        using var fileStream = File.OpenRead(filePath.localFilePath);
        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        form.Add(fileContent, "file", Path.GetFileName(filePath.localFilePath)); // name="file"

        // Add additional metadata
        form.Add(new StringContent(filePath.FileName), "filename"); // name="description"
        form.Add(new StringContent(applicationId.ToString()), "application_id"); // name="description"

        // hit the endpoint with the file to upload
        var response = await HttpClient.PostAsync($"{Constants.BaseURI}api/passport-documents", form);

        if (response.IsSuccessStatusCode)
        {
            // we succeeded here
            Console.WriteLine("YAYYYY WE MADE IT");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return true;
        }
        //we failed here
        Console.WriteLine(await response.Content.ReadAsStringAsync());
        return false;
    }

    public static async Task<VisaApplication> GetOfficerVisaApplicationByGoogleId(string googleId)
    {
        //return new VisaApplication(1, "1", "Hoth", "Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here Need to get a tan out here ", DateTime.Now, DateTime.Now.AddDays(3), null, new Status(1, "PENDING", null), DateTime.Now, DateTime.Now);
        try{
            return await HttpClient.GetFromJsonAsync<VisaApplication>($"{Constants.BaseURI}api/visa/getnext?officerId={googleId}") ?? throw new Exception("Failed to get a next step for you");
        }catch(Exception e){
            throw new Exception("There are no applications for you at this time, please try again later");
        }
    }

    public static async Task<PassportApplication?> GetOfficerPassportApplicationByGoogleId(string officerId)
    {
        try{
            return await HttpClient.GetFromJsonAsync<PassportApplication>($"{Constants.BaseURI}api/passport/getnext?officerId={officerId}") ?? throw new Exception("Failed to get a next step for you");
        }catch(Exception e){
            throw new Exception("There are no applications for you at this time, please try again later");
        }
    }

    public static async Task<PassportApplication?> CreatePassportApplication(PassportApplication passportApplication)
    {
        var response = await HttpClient.PostAsJsonAsync($"{Constants.BaseURI}api/passport", passportApplication);
        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<PassportApplication>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Failed to deserialise");
            }
        }
        Console.WriteLine(await response.Content.ReadAsStringAsync());
        return null;
    }

    public static async Task<VisaApplication?> CreateVisaApplication(VisaApplication visaApplication)
    {
        var response = await HttpClient.PostAsJsonAsync($"{Constants.BaseURI}api/visa", visaApplication);
        if (response.IsSuccessStatusCode)
        {
            try
            {
                Console.WriteLine("Yippie yippie we made a visa");
                return await response.Content.ReadFromJsonAsync<VisaApplication>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Failed to deserialise");
            }
        }
        Console.WriteLine(await response.Content.ReadAsStringAsync());
        return null;
    }

    public static async Task<List<ApplicationDocument>?> GetApplicationDocumentsByApplicationId(int applicationId)
    {
        try{
            return await HttpClient.GetFromJsonAsync<List<ApplicationDocument>>($"{Constants.BaseURI}api/passport-documents/passport-application/{applicationId}") ?? throw new Exception("Failed to retrieve a passport document");
        }catch(Exception ex){
            return null;
        }

    }
}