using DOSA_Client.Models;

namespace DOSA_Client.lib.Constants{
public static class Constants{
    public const string googleAuthURI = "https://accounts.google.com/o/oauth2/auth?client_id=988182050054-vlcub1cr22892gc1e4uesj5d6sa3ji1v.apps.googleusercontent.com&redirect_uri=http://localhost:3000&response_type=code&scope=openid%20phone%20email%20profile";
    public const string BaseURI = "http://ec2-13-246-113-36.af-south-1.compute.amazonaws.com/";
    public const string StarWarsURI = "https://swapi.dev/api/";
    }

public static class ContextKeys
    {
        public const string User = "User";

    }
}
