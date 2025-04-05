using System.Threading.Tasks;
using System.Web;

public static class GoogleAuthHelpers
{
    public static async Task<GoogleTokenExchangeResponse> getJwt(String authCode)
    {
        var client = new HttpClient();
        var parameters = new Dictionary<string, string>
        {
            { "code", HttpUtility.UrlDecode(authCode)},
            { "client_id", "988182050054-vlcub1cr22892gc1e4uesj5d6sa3ji1v.apps.googleusercontent.com" },
            { "client_secret", Environment.GetEnvironmentVariable("CLIENT_SECRET");
            { "redirect_uri", "http://localhost:3000" },
            { "grant_type", "authorization_code" }
        };

        var content = new FormUrlEncodedContent(parameters);
        return await client
                .PostAsync("https://oauth2.googleapis.com/token", content)
                .GetAwaiter()
                .GetResult()
                .Content
                .ReadFromJsonAsync<GoogleTokenExchangeResponse>() ?? new GoogleTokenExchangeResponse();
    }
}