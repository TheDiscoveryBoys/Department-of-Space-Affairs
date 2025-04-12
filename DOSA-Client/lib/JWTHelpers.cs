using System.Text;
using System.Text.Json;

public static class JWTHelpers{


    public static Dictionary<string, object>? DecodeClaims(string jwtToken)
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