using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;


namespace IntergalacticPassportAPI.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        public async Task<GoogleTokenExchangeResponse> GetJwt(string authCode)
        {
            var client = new HttpClient();
            var parameters = new Dictionary<string, string>
        {
            { "code", HttpUtility.UrlDecode(authCode)},
            { "client_id", "988182050054-vlcub1cr22892gc1e4uesj5d6sa3ji1v.apps.googleusercontent.com" },
            { "client_secret", Environment.GetEnvironmentVariable("CLIENT_SECRET") ?? throw new Exception("The client secret environment variable has not been set")},
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

        public Dictionary<string, object>? DecodeClaims(string jwtToken)
        {
            if (string.IsNullOrWhiteSpace(jwtToken))
                return null;

            var parts = jwtToken.Split('.');
            if (parts.Length != 3)
                return null;

            var payload = parts[1];

            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            try
            {
                var jsonBytes = Convert.FromBase64String(payload.Replace('-', '+').Replace('_', '/'));
                var json = Encoding.UTF8.GetString(jsonBytes);

                var claims = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                return claims;
            }
            catch
            {
                return null;
            }
        }
    }
}